using System;

namespace uRADMonitorX.Configuration
{
    [Flags]
    public enum DeviceCapability
    {
        Unknown = 0,

        Temperature = 1,

        Pressure = 2,

        Radiation = 4
    }
}
