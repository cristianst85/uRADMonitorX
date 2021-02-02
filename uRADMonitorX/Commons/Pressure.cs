namespace uRADMonitorX.Commons
{
    public static class Pressure
    {
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
