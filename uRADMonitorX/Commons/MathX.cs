using System;

namespace uRADMonitorX.Commons {
    
    public class MathX {

        public static double Truncate(double number, int decimals) {
            double order = Math.Pow((double)10, (double)decimals);
            number = number * order;
            number = Math.Truncate(number);
            return number / order;
        }
    }
}