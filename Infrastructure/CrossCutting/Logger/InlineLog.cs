using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.CrossCutting.Logger
{
    public class InlineLog
    {
        public string application_name { get; set; } = "APP";
        public string type_log { get; set; } = "InLine";
        public string short_message { get; set; }         
        public string stringActionArguments { get; set; }
        public string Method { get; set; }
        public string UserName { get; set; }
        public string Action { get; set; }
        public string Controller { get; set; }

    }
}
