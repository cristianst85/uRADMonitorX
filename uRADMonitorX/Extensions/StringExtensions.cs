namespace uRADMonitorX.Extensions
{
    public static class StringExtensions
    {
        public static bool IsEmpty(this string str)
        {
            return str == null ? false : str.Length == 0;
        }

        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        public static bool IsNotNullOrEmpty(this string str)
        {
            return !string.IsNullOrEmpty(str);
        }
    }
}
