using System;
using uRADMonitorX.Core;

namespace uRADMonitorX.Commons
{
    public class Radiation
    {
        public decimal Value { get; private set; }

        public RadiationUnitType MeasurementUnit { get; private set; }

        public Radiation(decimal value, RadiationUnitType measurementUnit)
        {
            this.Value = value;
            this.MeasurementUnit = measurementUnit;
        }

        public static Radiation FromCpm(decimal cpm)
        {
            return new Radiation(cpm, RadiationUnitType.Cpm);
        }

        public static Radiation FromCpm(decimal? cpm)
        {
            return new Radiation(cpm.Value, RadiationUnitType.Cpm);
        }

        public Radiation ConvertTo(RadiationUnitType unit, decimal conversionFactor)
        {
            if (MeasurementUnit == unit)
            {
                return this;
            }
            else if (MeasurementUnit == RadiationUnitType.Cpm && unit == RadiationUnitType.uSvH)
            {
                return new Radiation(CpmToMicroSvPerHour(Value, conversionFactor), RadiationUnitType.uSvH);
            }
            else if (MeasurementUnit == RadiationUnitType.Cpm && unit == RadiationUnitType.uRemH)
            {
                return new Radiation(CpmToMicroRemPerHour(Value, conversionFactor), RadiationUnitType.uRemH);
            }
            else
            {
                throw new NotSupportedException(string.Format("Radiation conversion from {0} to {1} is not supported.", MeasurementUnit.ToString("G"), unit.ToString("G")));
            }
        }

        public override string ToString()
        {
            return string.Format("{0} {1}", Value, EnumHelper.GetEnumDescription(this.MeasurementUnit));
        }

        public string ToString(int decimalDigitsNumber)
        {
            return new Radiation(MathX.Truncate(Value, decimalDigitsNumber), MeasurementUnit).ToString();
        }

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
            return cpm * conversionFactor * 100m;
        }

        public static decimal MicroSvPerHourToCpm(decimal uSvH, decimal conversionFactor)
        {
            return uSvH / conversionFactor;
        }

        public static decimal MicroRemPerHourToCpm(decimal uRemH, decimal conversionFactor)
        {
            return uRemH / (conversionFactor * 100m);
        }

        public static decimal MicroRemPerHourToMicroSvPerHour(decimal uRemH)
        {
            return uRemH / 100m;
        }

        public static decimal MicroSvPerHourToMicroRemPerHour(decimal uSvH)
        {
            return uSvH * 100m;
        }
    }
}
