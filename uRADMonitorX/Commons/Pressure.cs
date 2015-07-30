using System;

namespace uRADMonitorX.Commons {

    public static class Pressure {

        public static decimal PascalToKiloPascal(decimal pascal) {
            decimal kPascal = (pascal / 1000);
            return kPascal;
        }

        public static decimal PascalToAtm(decimal pascal) {
            throw new NotImplementedException();
        }

        public static decimal PascalToBar(decimal pascal) {
            throw new NotImplementedException();
        }
    }
}
