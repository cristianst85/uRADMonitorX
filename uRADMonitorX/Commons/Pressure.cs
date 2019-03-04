using System;

namespace uRADMonitorX.Commons {

    public static class Pressure {

        public static double PascalsToHectoPascals(double pascals) {
            return pascals / 100d;
        }

        public static double PascalsToKiloPascals(double pascals) {
            return pascals / 1000d;
        }

        public static double PascalsToMilliBars(double pascals) {
            return PascalsToHectoPascals(pascals);
        }
    }
}
