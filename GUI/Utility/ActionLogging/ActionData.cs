using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WVA_Connect_CSI.Utility.ActionLogging
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
