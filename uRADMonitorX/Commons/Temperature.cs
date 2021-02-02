using System.Globalization;

namespace uRADMonitorX.Commons
{
    public static class Temperature
    {
        public static decimal CelsiusToFahrenheit(decimal celsius)
        {
            decimal fahrenheit = (celsius * 9m / 5m) + 32;

            return fahrenheit;
        }

        public static decimal FahrenheitToCelsius(decimal fahrenheit)
        {
            decimal celsius = (fahrenheit - 32) * 5m / 9m;

            return celsius;
        }

        public static decimal CelsiusToKelvin(decimal celsius)
        {
            return celsius + decimal.Parse("273.15", NumberStyles.AllowDecimalPoint, new CultureInfo("en-US").NumberFormat);
        }

        public static decimal KelvinToCelsius(decimal kelvin)
        {
            return kelvin - decimal.Parse("273.15", NumberStyles.AllowDecimalPoint, new CultureInfo("en-US").NumberFormat);
        }
    }
}
