using System;
using System.Globalization;

namespace uRADMonitorX.Commons {

    public static class MathX {

        public static double Truncate(double number, int decimals) {
            double order = Math.Pow((double)10, (double)decimals);
            number = number * order;
            number = Math.Truncate(number);
            return number / order;
        }

        public static bool IsInteger(String number) {
            int integer = 0;
            return int.TryParse(number, out integer);
        }

        public static bool IsNatural(String number) {
            int value = 0;
            bool isInteger = int.TryParse(number, out value);
            return isInteger && value >= 0;
        }

        public static bool IsDecimal(String number, NumberFormatInfo numberFormatInfo) {
            double value = 0;
            return double.TryParse(number, NumberStyles.AllowDecimalPoint, numberFormatInfo, out value);
        }
    }
}