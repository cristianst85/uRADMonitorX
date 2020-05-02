using System.Globalization;

namespace uRADMonitorX.Commons.Formatting
{
    public static class TextStyleFormatter
    {
        public static string HyphenToPascalCase(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return s;
            }

            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(s.ToLower().Replace('-', ' ')).Replace(" ", string.Empty);
        }

        public static string PascalCaseToHyphen(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return s;
            }

            return System.Text.RegularExpressions.Regex.Replace(s, "([A-Z])", " $1", System.Text.RegularExpressions.RegexOptions.Compiled).Trim().Replace(' ', '-').ToLower();
        }

        public static string UnderscoreToPascalCase(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return s;
            }

            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(s.ToLower().Replace('_', ' ')).Replace(" ", string.Empty);
        }

        public static string PascalCaseToUnderscore(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return s;
            }

            return System.Text.RegularExpressions.Regex.Replace(s.ToString(), "([A-Z])", " $1", System.Text.RegularExpressions.RegexOptions.Compiled).Trim().Replace(' ', '_').ToLower();
        }

        public static string UppercaseFirst(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return s;
            }

            return string.Format("{0}{1}", char.ToUpper(s[0]), s.Substring(1));
        }

        public static string LowercaseFirst(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return s;
            }

            return string.Format("{0}{1}", char.ToLower(s[0]), s.Substring(1));
        }

        public static bool IsLowerCase(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return true;
            }

            return s.Equals(s.ToLower());
        }
    }
}
