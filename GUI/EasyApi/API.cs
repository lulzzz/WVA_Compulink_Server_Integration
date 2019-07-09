using WVA_Compulink_Server_Integration.Errors;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WVA_Compulink_Server_Integration.WebTools
{
    class API
    {
        static public string Post(string endpoint, object jsonObject)
        {
            try
            {
                string targetResponse = null;
                string json = JsonConvert.SerializeObject(jsonObject);

                UTF8Encoding encoding = new UTF8Encoding();
                byte[] byteArray = encoding.GetBytes(json);

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(endpoint);
                request.ContentLength = byteArray.Length;
                request.ContentType = @"application/json";
                request.Method = "POST";

                using (Stream dataStream = request.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                }

                WebResponse webResponse = request.GetResponse();
                using (Stream responseStream = webResponse.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                    targetResponse = reader.ReadToEnd();
                    reader.Close();
                }

                return targetResponse;
            }
            catch (Exception x)
            {
                Error.ReportOrLog(x);
                return null;
            }
        }
    }
}
