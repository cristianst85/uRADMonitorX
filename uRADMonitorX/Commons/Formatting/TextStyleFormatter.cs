using System;
using System.Globalization;

namespace uRADMonitorX.Commons.Formatting {

    public static class TextStyleFormatter {

        public static String HyphenToPascalCase(String s) {
            if (String.IsNullOrEmpty(s)) {
                return s;
            }
            else {
                return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(s.ToLower().Replace('-', ' ')).Replace(" ", String.Empty);
            }
        }

        public static String PascalCaseToHyphen(String s) {
            if (String.IsNullOrEmpty(s)) {
                return s;
            }
            else {
                return System.Text.RegularExpressions.Regex.Replace(s, "([A-Z])", " $1", System.Text.RegularExpressions.RegexOptions.Compiled).Trim().Replace(' ', '-').ToLower();
            }
        }

        public static String UnderscoreToPascalCase(String s) {
            if (String.IsNullOrEmpty(s)) {
                return s;
            }
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(s.ToLower().Replace('_', ' ')).Replace(" ", String.Empty);
        }

        public static String PascalCaseToUnderscore(String s) {
            if (String.IsNullOrEmpty(s)) {
                return s;
            }
            return System.Text.RegularExpressions.Regex.Replace(s.ToString(), "([A-Z])", " $1", System.Text.RegularExpressions.RegexOptions.Compiled).Trim().Replace(' ', '_').ToLower();
        }

        public static String UppercaseFirst(String s) {
            if (String.IsNullOrEmpty(s)) {
                return s;
            }
            else {
                return String.Format("{0}{1}", char.ToUpper(s[0]), s.Substring(1));
            }
        }

        public static String LowercaseFirst(String s) {
            if (String.IsNullOrEmpty(s)) {
                return s;
            }
            else {
                return String.Format("{0}{1}", char.ToLower(s[0]), s.Substring(1));
            }
        }

        public static bool IsLowerCase(String s) {
            if (String.IsNullOrEmpty(s)) {
                return true;
            }
            else {
                return s.Equals(s.ToLower());
            }
        }
    }
}
