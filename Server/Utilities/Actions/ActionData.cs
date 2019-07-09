using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WVA_Compulink_Server_Integration.Utilities.Actions
{
    public class ActionData
    {
        public string FileName { get; set; }
        public string Content { get; set; }

        public ActionData(string fileName, string content)
        {
            FileName = fileName;
            Content = content;
        }
    }
}
