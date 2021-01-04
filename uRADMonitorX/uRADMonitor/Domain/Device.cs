using System.Device.Location;
using System.Text;
using uRADMonitorX.Core.Device;
using uRADMonitorX.Extensions;

namespace uRADMonitorX.uRADMonitor.Domain
{
    public class Device
    {
        public string Id { get; set; }

        public GeoCoordinate GeographicCoordinate { get; set; }

        public DeviceStatus? Status { get; set; }

        public DevicePlacement? Placement { get; set; }

        public RadiationDetector Detector { get; set; }

        public string CountryCode { get; set; }

        public string City { get; set; }

        public string GetLocation()
        {
            var location = new StringBuilder();

            if (City.IsNotNullOrEmpty())
            {
                location.Append(City);
            }

            if (CountryCode.IsNotNullOrEmpty())
            {
                if (location.Length > 0)
                {
                    location.AppendFormat(" ({0})", CountryCode);
                }
                else
                {
                    location.Append(CountryCode);
                }
            }

            return location.ToString().Trim();
        }

        public bool IsOnline
        {
            get
            {
                return Status.HasValue && Status.Value == DeviceStatus.Online;
            }
        }

        public string GetStatus()
        {
            return Status.HasValue ? Status.ToString() : "Offline";
        }

        public string GetDetectorName()
        {
            return Detector?.Name;
        }

        public string RawData { get; set; }
    }
}
