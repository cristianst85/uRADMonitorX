using System;
using System.Globalization;

namespace uRADMonitorX.Commons {

    public static class Temperature {

        public static double CelsiusToFahrenheit(double celsius) {
            double fahrenheit = (celsius * 9 / 5) + 32;
            return fahrenheit;
        }

        public static double FahrenheitToCelsius(double fahrenheit) {
            double celsius = (fahrenheit - 32) * 5 / 9;
            return celsius;
        }

        public static double CelsiusToKelvin(double celsius) {
            return celsius + Double.Parse("273.15", System.Globalization.NumberStyles.AllowDecimalPoint, new CultureInfo("en-US").NumberFormat);
        }

        public static double KelvinToCelsius(double kelvin) {
            return kelvin - Double.Parse("273.15", System.Globalization.NumberStyles.AllowDecimalPoint, new CultureInfo("en-US").NumberFormat);
        }
    }
}
