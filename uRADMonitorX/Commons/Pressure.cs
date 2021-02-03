using System;
using System.Globalization;
using uRADMonitorX.Core;

namespace uRADMonitorX.Commons
{
    public class Pressure
    {
        public decimal Value { get; private set; }

        public PressureUnitType MeasurementUnit { get; private set; }

        private Pressure(decimal value, PressureUnitType measurementUnit)
        {
            this.Value = value;
            this.MeasurementUnit = measurementUnit;
        }

        public static Pressure FromPascals(decimal pascals)
        {
            return new Pressure(pascals, PressureUnitType.Pa);
        }

        public static Pressure FromPascals(decimal? pascals)
        {
            return new Pressure(pascals.Value, PressureUnitType.Pa);
        }

        public Pressure ConvertTo(PressureUnitType measurementUnit)
        {
            if (MeasurementUnit == measurementUnit)
            {
                return this;
            }
            else if (MeasurementUnit == PressureUnitType.Pa && measurementUnit == PressureUnitType.hPa)
            {
                return new Pressure(PascalsToHectoPascals(Value), PressureUnitType.hPa);
            }
            else if (MeasurementUnit == PressureUnitType.Pa && measurementUnit == PressureUnitType.kPa)
            {
                return new Pressure(PascalsToKiloPascals(Value), PressureUnitType.kPa);
            }
            else if (MeasurementUnit == PressureUnitType.Pa && measurementUnit == PressureUnitType.mbar)
            {
                return new Pressure(PascalsToMilliBars(Value), PressureUnitType.mbar);
            }
            else
            {
                throw new NotSupportedException(string.Format("Pressure conversion from {0} to {1} is not supported.", MeasurementUnit.ToString("G"), measurementUnit.ToString("G")));
            }
        }

        public override string ToString()
        {
            return string.Format("{0:#0.###} {1}", Value, MeasurementUnit.ToString("G"));
        }

        public string ToString(int decimalDigitsNumber)
        {
            return string.Format(new NumberFormatInfo() { NumberDecimalDigits = decimalDigitsNumber }, "{0:#0.###} {1}", Value, MeasurementUnit.ToString("G"));
        }

        public static decimal PascalsToHectoPascals(decimal pascals)
        {
            return pascals / 100m;
        }

        public static decimal PascalsToKiloPascals(decimal pascals)
        {
            return pascals / 1000m;
        }

        public static decimal PascalsToMilliBars(decimal pascals)
        {
            return PascalsToHectoPascals(pascals);
        }
    }
}
