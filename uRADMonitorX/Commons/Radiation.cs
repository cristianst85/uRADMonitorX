namespace uRADMonitorX.Commons
{
    public static class Radiation
    {
        /// <summary>
        /// Converts from counts per minute (cpm) to microsieverts per hour (µSv/h).
        /// </summary>
        /// <param name="cpm"></param>
        /// <param name="conversionFactor"></param>
        /// <returns></returns>
        public static decimal CpmToMicroSvPerHour(decimal cpm, decimal conversionFactor)
        {
            return cpm * conversionFactor;
        }

        /// <summary>
        /// Converts from counts per minute (cpm) to microrem (roentgen equivalent in man) per hour (µrem/h).
        /// </summary>
        /// <param name="cpm"></param>
        /// <param name="conversionFactor"></param>
        /// <returns></returns>
        public static decimal CpmToMicroRemPerHour(decimal cpm, decimal conversionFactor)
        {
            return cpm * conversionFactor * 100;
        }

        public static decimal MicroSvPerHourToCpm(decimal uSvH, decimal conversionFactor)
        {
            return uSvH / conversionFactor;
        }

        public static decimal MicroRemPerHourToCpm(decimal uRemH, decimal conversionFactor)
        {
            return uRemH / (conversionFactor * 100);
        }

        public static decimal MicroRemPerHourToMicroSvPerHour(decimal uRemH)
        {
            return uRemH / 100;
        }

        public static decimal MicroSvPerHourToMicroRemPerHour(decimal uSvH)
        {
            return uSvH * 100;
        }
    }
}
