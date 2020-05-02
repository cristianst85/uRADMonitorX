using System;
using System.Globalization;

namespace uRADMonitorX.Commons
{
    public static class MathX
    {
        public static double Truncate(double number, int decimals)
        {
            var order = Math.Pow(10, decimals);
            number = number * order;
            number = Math.Truncate(number);

            return number / order;
        }

        public static bool IsInteger(string number)
        {
            return int.TryParse(number, out int integer);
        }

        public static bool IsNatural(string number)
        {
            bool isInteger = int.TryParse(number, out int value);

            return isInteger && value >= 0;
        }

        public static bool IsDecimal(string number, NumberFormatInfo numberFormatInfo)
        {
            return double.TryParse(number, NumberStyles.AllowDecimalPoint, numberFormatInfo, out double value);
        }
    }
}
