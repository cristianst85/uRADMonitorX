using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace uRADMonitorX.Commons {

    public static class EnumHelper {

        /// <summary>
        /// Takes an enum type and returns a generic list populated with each enum item.
        /// <para>
        /// Source code was taken from: 
        /// http://extensionmethod.net/csharp/enum/generic-enum-to-list-t-converter
        /// </para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<T> EnumToList<T>() {
            Type enumType = typeof(T);

            // Can't use type constraints on value types, so have to do check like this
            if (enumType.BaseType != typeof(Enum)) {
                throw new ArgumentException("T must be of type System.Enum");
            }

            return new List<T>(Enum.GetValues(enumType) as IEnumerable<T>);
        }

        /// <summary>
        /// Gets the description attribute assigned to an item in an Enum.
        /// <para>
        /// Source code was taken from: 
        /// http://extensionmethod.net/csharp/enum/getenumdescription
        /// </para>
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetEnumDescription<T>(string value) {
            Type type = typeof(T);
            String name = null;

            String[] enumNames = Enum.GetNames(type);//.Where(f => f.Equals(value, StringComparison.CurrentCultureIgnoreCase)).Select(d => d).FirstOrDefault();

            foreach (String enumName in enumNames) {
                if (enumName.Equals(value, StringComparison.CurrentCultureIgnoreCase)) {
                    name = enumName;
                    break;
                }
            }

            if (name == null) {
                return string.Empty;
            }

            var field = type.GetField(name);
            var customAttribute = field.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return customAttribute.Length > 0 ? ((DescriptionAttribute)customAttribute[0]).Description : name;
        }

        /// <summary>
        /// Gets the description attribute assigned to an item in an Enum.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetEnumDescription<T>(T value) {
            return GetEnumDescription<T>(value.ToString());
        }

        public static T GetEnumByDescription<T>(String description) {
            return GetEnumByDescription<T>(description, false);
        }

        public static T GetEnumByDescription<T>(String description, bool ignoreCase) {
            Type type = typeof(T);
            List<T> enums = EnumHelper.EnumToList<T>();
            foreach (T e in enums) {
                if (ignoreCase) {
                    if (EnumHelper.GetEnumDescription<T>(e.ToString()).Equals(description, StringComparison.OrdinalIgnoreCase)) {
                        return e;
                    }
                }
                else {
                    if (EnumHelper.GetEnumDescription<T>(e.ToString()).Equals(description)) {
                        return e;
                    }
                }
            }
            throw new Exception(String.Format("Could not find enum type {0} with description attribute value {1}.", type.Name, description));
        }
    }
}
