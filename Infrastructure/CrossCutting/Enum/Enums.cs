using CrossCutting.Enum;
using System;
using System.ComponentModel;

namespace Infrastructure.CrossCutting.Enum
{
    public class Enums
    {
        public enum ApiEnum
        {
            [Description("me")]
            Me = 1
        }

        public enum StatusRedemption
        {
            [EnumGuid("CDD9764D-2170-4D09-B08F-08695E77653C")]
            Reversed = 1,
            [EnumGuid("403ACAF6-2C28-440C-A512-6564A8FB1152")]
            Cancelled = 2,
            [EnumGuid("BCC0C7C6-F1D6-4955-98B4-72C9B3685E7D")]
            Authorized = 3,
            [EnumGuid("A35FC608-AF4B-4519-A33E-DF31634098FC")]
            Confirmed = 4,
            [EnumGuid("F03265D5-1B27-4487-9F38-4BB875FA244C")]
            NotAuthorized = 5,
            [EnumGuid("FAE9CB7E-0FDC-4B19-9675-372C2C294316")]
            Pending = 6
        }

    }

}
