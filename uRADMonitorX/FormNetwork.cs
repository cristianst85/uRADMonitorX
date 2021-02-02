using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using uRADMonitorX.Commons.Controls;
using uRADMonitorX.Commons.Controls.Extensions;
using uRADMonitorX.Configuration;
using uRADMonitorX.Extensions;
using uRADMonitorX.Forms;
using uRADMonitorX.uRADMonitor.Domain;
using uRADMonitorX.uRADMonitor.Services;

namespace uRADMonitorX
{
    public partial class FormNetwork : Form
    {
        private readonly ISettings settings;
        private readonly IDeviceServiceFactory deviceServiceFactory;

        private readonly string Title;

        private readonly List<Device> devices;

        public FormNetwork(ISettings settings, IDeviceServiceFactory deviceServiceFactory)
        {
            InitializeComponent();

            this.settings = settings;
            this.deviceServiceFactory = deviceServiceFactory;

            this.Title = Text;

            this.devices = new List<Device>();

            this.listViewDevices.MultiSelect = false;
            this.listViewDevices.FullRowSelect = true;

            this.listViewDevices.SelectedIndexChanged += ListViewDevices_SelectedIndexChanged;
            this.authenticationToolStripMenuItem.Click += AuthenticationToolStripMenuItem_Click;
            this.buttonRefresh.Click += ButtonRefresh_Click;

            this.listViewDevices.MouseDoubleClick += ListViewDevices_MouseDoubleClick;
            this.checkBoxShowOnlineDevicesOnly.CheckedChanged += CheckBoxShowOnlineDevicesOnly_CheckedChanged;

            ToogleControls(state: ControlState.Enabled);
        }

        private void CheckBoxShowOnlineDevicesOnly_CheckedChanged(object sender, EventArgs e)
        {
            if (this.devices.IsEmpty())
            {
                return;
            }

            try
            {
                this.InvokeIfRequired(() =>
                {
                    ToogleControls(state: ControlState.Disabled);

                    Text = string.Format("{0}", Title);

                    listViewDevices.Items.Clear();
                    ListViewDevices_SelectedIndexChanged(null, null);
                });

                this.InvokeIfRequired(() =>
                {
                    foreach (var device in GetDevices())
                    {
                        listViewDevices.Items.Add(new DeviceListViewItem(device));
                    }

                    Text = string.Format("{0} ({1} {2} found)", Title, listViewDevices.Items.Count, listViewDevices.Items.Count == 1 ? "device" : "devices");
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, $"Error: {ex.Message}", Program.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                ToogleControls(state: ControlState.Enabled);
            }
        }

        private void ListViewDevices_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (sender is ListView listView)
            {
                var clickedItem = listView.HitTest(e.Location).Item;

                if (clickedItem != null)
                {
                    if (clickedItem is DeviceListViewItem deviceListViewItem)
                    {
                        uRADMonitorHelper.OpenDashboardUrl(deviceListViewItem.Device.Id);
                    }
                }
            }
        }

        private void AuthenticationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var form = new FormNetworkAuthentication(settings, deviceServiceFactory))
            {
                var result = form.ShowDialog(this);
                ToogleControls(state: ControlState.Enabled);
            }
        }

        private void ToogleControls(ControlState state)
        {
            buttonRefresh.Enabled = state.ToBoolean() && settings.uRADMonitorNetwork.UserId.IsNotNullOrEmpty();
            checkBoxShowOnlineDevicesOnly.Enabled = state.ToBoolean();
            configToolStripMenuItem.Enabled = state.ToBoolean();
        }

        private void ListViewDevices_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewDevices.SelectedItems.Count == 1)
            {
                if (listViewDevices.SelectedItems[0] is DeviceListViewItem selectedDevice)
                {
                    textBoxStatus.Text = selectedDevice.Device.GetStatus();
                    textBoxLatitude.Text = selectedDevice.Device.GeographicCoordinate?.Latitude.ToString();
                    textBoxLongitude.Text = selectedDevice.Device.GeographicCoordinate?.Longitude.ToString();
                    textBoxPlacement.Text = selectedDevice.Device.Placement?.ToString();
                    richTextBoxRaw.Text = selectedDevice.Device.RawData;
                }
            }
            else
            {
                textBoxStatus.Text = string.Empty;
                textBoxLatitude.Text = string.Empty;
                textBoxLongitude.Text = string.Empty;
                textBoxPlacement.Text = string.Empty;
                richTextBoxRaw.Text = string.Empty;
            }
        }

        private void ButtonRefresh_Click(object sender, EventArgs e)
        {
            UpdateDevicesListAsync();
        }

        private async void UpdateDevicesListAsync()
        {
            try
            {
                this.InvokeIfRequired(() =>
                {
                    ToogleControls(state: ControlState.Disabled);

                    Text = string.Format("{0}", Title);

                    listViewDevices.Items.Clear();
                    ListViewDevices_SelectedIndexChanged(null, null);
                });

                await RefreshDevices();

                this.InvokeIfRequired(() =>
                {
                    foreach (var device in GetDevices())
                    {
                        listViewDevices.Items.Add(new DeviceListViewItem(device));
                    }

                    Text = string.Format("{0} ({1} {2} found)", Title, listViewDevices.Items.Count, listViewDevices.Items.Count == 1 ? "device" : "devices");
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, $"Error: {ex.Message}", Program.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                ToogleControls(state: ControlState.Enabled);
            }
        }

        private Task RefreshDevices()
        {
            return Task.Run(() =>
            {
                devices.Clear();

                var deviceService = deviceServiceFactory.Create(settings.uRADMonitorNetwork.UserId, settings.uRADMonitorNetwork.UserKey);

                devices.AddRange(deviceService.GetAll().Devices.OrderBy(x => x.Id));
            });
        }

        private IEnumerable<Device> GetDevices()
        {
            var showOnlineDevicesOnly = this.checkBoxShowOnlineDevicesOnly.Checked;

            if (showOnlineDevicesOnly)
            {
                return devices.Where(x => x.IsOnline);
            };

            return this.devices;
        }
    }
}
