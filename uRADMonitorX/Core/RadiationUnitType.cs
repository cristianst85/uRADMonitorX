using System.ComponentModel;

namespace uRADMonitorX.Core
{
    public enum RadiationUnitType
    {
        /// <summary>
        /// Counts per minute (cpm).
        /// </summary>
        [Description("cpm")]
        Cpm,

        /// <summary>
        /// Microsieverts per hour (µSv/h).
        /// </summary>
        [Description("µSv/h")]
        uSvH,

        /// <summary>
        /// Microrem (roentgen equivalent in man) per hour (µrem/h).
        /// </summary>
        [Description("µrem/h")]
        uRemH
    }
}
