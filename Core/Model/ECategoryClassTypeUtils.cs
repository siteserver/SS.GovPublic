using System;

namespace SS.GovPublic.Core.Model
{
    public enum ECategoryClassType
    {
        Channel,
        Form,
        Department,
        Service,
        UserDefined
    }

    public class ECategoryClassTypeUtils
    {
        public static string GetValue(ECategoryClassType type)
        {
            switch (type)
            {
                case ECategoryClassType.Channel:
                    return "Channel";
                case ECategoryClassType.Form:
                    return "Form";
                case ECategoryClassType.Department:
                    return "Department";
                case ECategoryClassType.Service:
                    return "Service";
                case ECategoryClassType.UserDefined:
                    return "UserDefined";
                default:
                    throw new Exception();
            }
        }

        public static string GetText(ECategoryClassType type)
        {
            switch (type)
            {
                case ECategoryClassType.Channel:
                    return "主题";
                case ECategoryClassType.Form:
                    return "体裁";
                case ECategoryClassType.Department:
                    return "机构";
                case ECategoryClassType.Service:
                    return "服务对象";
                case ECategoryClassType.UserDefined:
                    return "自定义";
                default:
                    throw new Exception();
            }
        }

        public static ECategoryClassType GetEnumType(string typeStr)
        {
            var retval = ECategoryClassType.Form;

            if (Equals(ECategoryClassType.Channel, typeStr))
            {
                retval = ECategoryClassType.Channel;
            }
            else if (Equals(ECategoryClassType.Form, typeStr))
            {
                retval = ECategoryClassType.Form;
            }
            else if (Equals(ECategoryClassType.Department, typeStr))
            {
                retval = ECategoryClassType.Department;
            }
            else if (Equals(ECategoryClassType.Service, typeStr))
            {
                retval = ECategoryClassType.Service;
            }
            else if (Equals(ECategoryClassType.UserDefined, typeStr))
            {
                retval = ECategoryClassType.UserDefined;
            }
            return retval;
        }

        public static bool Equals(ECategoryClassType type, string typeStr)
        {
            if (string.IsNullOrEmpty(typeStr)) return false;
            if (string.Equals(GetValue(type).ToLower(), typeStr.ToLower()))
            {
                return true;
            }
            return false;
        }

        public static bool Equals(string typeStr, ECategoryClassType type)
        {
            return Equals(type, typeStr);
        }
    }
}
