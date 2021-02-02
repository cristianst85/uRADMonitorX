using System;

namespace uRADMonitorX.Configuration
{
    public class SettingsChangedEventArgs : EventArgs
    {
        public ISettings Settings { get; private set; }

        public SettingsChangedEventArgs(ISettings settings)
        {
            this.Settings = settings;
        }
    }
}