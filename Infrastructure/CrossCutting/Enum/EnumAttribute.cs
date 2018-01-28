using System;
using System.Collections.Generic;
using System.Text;

namespace CrossCutting.Enum
{
    public class EnumGuid : Attribute
    {
        public Guid Guid;

        public EnumGuid(string guid)
        {
            Guid = new Guid(guid);
        }
    }
}


