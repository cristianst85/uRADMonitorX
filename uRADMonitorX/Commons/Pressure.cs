using System;

namespace uRADMonitorX.Commons {

    public static class Pressure {

        public static double PascalToKiloPascal(double pascal) {
            double kPascal = (pascal / 1000);
            return kPascal;
        }

        public static double PascalToAtm(double pascal) {
            throw new NotImplementedException();
        }

        public static double PascalToBar(double pascal) {
            throw new NotImplementedException();
        }
    }
}
