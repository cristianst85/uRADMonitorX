using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace uRADMonitorX.Configuration
{
    public interface ISettings
    {
        bool IsPollingEnabled { get; set; }

        GeneralSettings General { get; }

        DisplaySettings Display { get; }

        NotificationsSettings Notifications { get; }

        LoggingSettings Logging { get; }

        MiscSettings Misc { get; }

        IList<DeviceSettings> Devices { get; }

        [SuppressMessage(category: "Style", checkId: "IDE1006")]
        uRADMonitorNetworkSettings uRADMonitorNetwork { get; }

        void Save();
    }
}
