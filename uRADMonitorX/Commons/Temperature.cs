using System;
using System.Globalization;
using uRADMonitorX.Core;

namespace uRADMonitorX.Commons
{
    public class Temperature
    {
        public decimal Value { get; private set; }

        public TemperatureUnitType MeasurementUnit { get; private set; }

        private Temperature(decimal value, TemperatureUnitType measurementUnit)
        {
            this.Value = value;
            this.MeasurementUnit = measurementUnit;
        }

        public static Temperature FromCelsius(decimal celsius)
        {
            return new Temperature(celsius, TemperatureUnitType.Celsius);
        }

        public Temperature ConvertTo(TemperatureUnitType measurementUnit)
        {
            if (MeasurementUnit == measurementUnit)
            {
                return this;
            }
            else if (MeasurementUnit == TemperatureUnitType.Celsius && measurementUnit == TemperatureUnitType.Fahrenheit)
            {
                return new Temperature(CelsiusToFahrenheit(Value), TemperatureUnitType.Fahrenheit);
            }
            else if (MeasurementUnit == TemperatureUnitType.Fahrenheit && measurementUnit == TemperatureUnitType.Celsius)
            {
                return new Temperature(FahrenheitToCelsius(Value), TemperatureUnitType.Celsius);
            }
            else
            {
                throw new NotSupportedException(string.Format("Temperature conversion from {0} to {1} is not supported.", MeasurementUnit.ToString("G"), measurementUnit.ToString("G")));
            }
        }

        public override string ToString()
        {
            return string.Format("{0:#0.0#} {1}", Value, MeasurementUnit == TemperatureUnitType.Celsius ? "°C" : MeasurementUnit == TemperatureUnitType.Fahrenheit ? "°F" : "K");
        }

        public static decimal CelsiusToFahrenheit(decimal celsius)
        {
            var fahrenheit = (celsius * 9m / 5m) + 32m;

            return fahrenheit;
        }

        public static decimal FahrenheitToCelsius(decimal fahrenheit)
        {
            var celsius = (fahrenheit - 32m) * 5m / 9m;

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
