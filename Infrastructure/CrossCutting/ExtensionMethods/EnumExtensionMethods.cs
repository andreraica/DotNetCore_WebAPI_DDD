using CrossCutting.Enum;
using System;
using System.ComponentModel;
using System.Reflection;

namespace Infrastructure.CrossCutting.ExtensionMethods
{
    public static class EnumExtensionMethods
    {
        #region "Public"

        public static string GetDescription(this System.Enum value)
        {
            var fi = value.GetType().GetField(value.ToString());
            var attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute),
                false);

            return (attributes.Length > 0) ? attributes[0].Description : value.ToString();
        }

        public static Guid GetEnumGuid(this System.Enum e)
        {
            Type type = e.GetType();

            MemberInfo[] memInfo = type.GetMember(e.ToString());

            if (memInfo != null && memInfo.Length > 0)
            {
                object[] attrs = (object[]) memInfo[0].GetCustomAttributes(typeof(EnumGuid), false);
                if (attrs != null && attrs.Length > 0)
                    return ((EnumGuid)attrs[0]).Guid;
            }

            throw new ArgumentException("Enum " + e.ToString() + " has no EnumGuid defined!");
        }

        #endregion
    }
}
