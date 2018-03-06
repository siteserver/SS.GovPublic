using System;
using System.Web.UI.WebControls;

namespace SS.GovPublic.Model
{
    public enum EIdentifierType
    {
        Department,
        Channel,
        Attribute,
        Sequence
    }

    public class EIdentifierTypeUtils
    {
        public static string GetValue(EIdentifierType type)
        {
            if (type == EIdentifierType.Department)
            {
                return "Department";
            }
            else if (type == EIdentifierType.Channel)
            {
                return "Channel";
            }
            else if (type == EIdentifierType.Attribute)
            {
                return "Attribute";
            }
            else if (type == EIdentifierType.Sequence)
            {
                return "Sequence";
            }
            else
            {
                throw new Exception();
            }
        }

        public static string GetText(EIdentifierType type)
        {
            if (type == EIdentifierType.Department)
            {
                return "机构分类代码";
            }
            else if (type == EIdentifierType.Channel)
            {
                return "主题分类代码";
            }
            else if (type == EIdentifierType.Attribute)
            {
                return "字段值";
            }
            else if (type == EIdentifierType.Sequence)
            {
                return "顺序号";
            }
            else
            {
                throw new Exception();
            }
        }

        public static EIdentifierType GetEnumType(string typeStr)
        {
            var retval = EIdentifierType.Department;

            if (Equals(EIdentifierType.Department, typeStr))
            {
                retval = EIdentifierType.Department;
            }
            else if (Equals(EIdentifierType.Channel, typeStr))
            {
                retval = EIdentifierType.Channel;
            }
            else if (Equals(EIdentifierType.Attribute, typeStr))
            {
                retval = EIdentifierType.Attribute;
            }
            else if (Equals(EIdentifierType.Sequence, typeStr))
            {
                retval = EIdentifierType.Sequence;
            }
            return retval;
        }

        public static bool Equals(EIdentifierType type, string typeStr)
        {
            if (string.IsNullOrEmpty(typeStr)) return false;
            if (string.Equals(GetValue(type).ToLower(), typeStr.ToLower()))
            {
                return true;
            }
            return false;
        }

        public static bool Equals(string typeStr, EIdentifierType type)
        {
            return Equals(type, typeStr);
        }

        public static ListItem GetListItem(EIdentifierType type, bool selected)
        {
            var item = new ListItem(GetText(type), GetValue(type));
            if (selected)
            {
                item.Selected = true;
            }
            return item;
        }

        public static void AddListItems(ListControl listControl)
        {
            if (listControl != null)
            {
                listControl.Items.Add(GetListItem(EIdentifierType.Department, false));
                listControl.Items.Add(GetListItem(EIdentifierType.Channel, false));
                listControl.Items.Add(GetListItem(EIdentifierType.Attribute, false));
                listControl.Items.Add(GetListItem(EIdentifierType.Sequence, false));
            }
        }
    }
}
