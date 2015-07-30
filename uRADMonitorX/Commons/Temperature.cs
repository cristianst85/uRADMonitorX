using System;
using System.Globalization;

namespace uRADMonitorX.Commons {

    public static class Temperature {

        public static decimal CelsiusToFahrenheit(decimal celsius) {
            decimal fahrenheit = (celsius * 9 / 5) + 32;
            return fahrenheit;
        }

        public static decimal FahrenheitToCelsius(decimal fahrenheit) {
            decimal celsius = (fahrenheit - 32) * 5 / 9;
            return celsius;
        }

        public static decimal CelsiusToKelvin(decimal celsius) {
            return celsius + Decimal.Parse("273.15", System.Globalization.NumberStyles.AllowDecimalPoint, new CultureInfo("en-US").NumberFormat);
        }

        public static decimal KelvinToCelsius(decimal kelvin) {
            return kelvin - Decimal.Parse("273.15", System.Globalization.NumberStyles.AllowDecimalPoint, new CultureInfo("en-US").NumberFormat);
        }
    }
}
