using System;
using System.Collections.Generic;

namespace SS.GovPublic.Core
{
    public static class TranslateUtils
    {
        public static T Get<T>(IDictionary<string, object> dict, string name, T defaultValue = default(T))
        {
            return Cast(Get(dict, name), defaultValue);
        }

        public static object Get(IDictionary<string, object> dict, string name)
        {
            if (string.IsNullOrEmpty(name)) return null;

            return dict.TryGetValue(name, out var extendValue) ? extendValue : null;
        }

        public static T Cast<T>(object value, T defaultValue = default(T))
        {
            switch (value)
            {
                case null:
                    return defaultValue;
                case T variable:
                    return variable;
                default:
                    try
                    {
                        return (T)Convert.ChangeType(value, typeof(T));
                    }
                    catch (InvalidCastException)
                    {
                        return defaultValue;
                    }
            }
        }

        public static int ToInt(string str)
        {
            int i;
            return int.TryParse(str, out i) ? i : 0;
        }

        public static bool ToBool(string str)
        {
            bool i;
            return bool.TryParse(str, out i) && i;
        }

        public static DateTime ToDateTime(string dateTimeStr)
        {
            DateTime dateTime;
            return DateTime.TryParse(dateTimeStr.Trim(), out dateTime) ? dateTime : DateTime.Now;
        }
    }
}
