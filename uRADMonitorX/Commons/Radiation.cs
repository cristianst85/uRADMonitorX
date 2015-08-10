using System;

namespace uRADMonitorX.Commons {

    public static class Radiation {

        /// <summary>
        /// Converts from counts per minute (cpm) to microsieverts per hour (µSv/h).
        /// </summary>
        /// <param name="cpm"></param>
        /// <param name="conversionFactor"></param>
        /// <returns></returns>
        public static double CpmToMicroSvPerHour(double cpm, double conversionFactor) {
            return cpm * conversionFactor;
        }

        /// <summary>
        /// Converts from counts per minute (cpm) to microrem (roentgen equivalent in man) per hour (µrem/h).
        /// </summary>
        /// <param name="cpm"></param>
        /// <param name="conversionFactor"></param>
        /// <returns></returns>
        public static double CpmToMicroRemPerHour(double cpm, double conversionFactor) {
            return cpm * conversionFactor * 100;
        }

        public static double MicroSvPerHourToCpm(double uSvH, double conversionFactor) {
            return uSvH / conversionFactor;
        }

        public static double MicroRemPerHourToCpm(double uRemH, double conversionFactor) {
            return uRemH / (conversionFactor * 100);
        }

        public static double MicroRemPerHourToMicroSvPerHour(double uRemH) {
            return uRemH / 100;
        }

        public static double MicroSvPerHourToMicroRemPerHour(double uSvH) {
            return uSvH * 100;
        }
    }
}
