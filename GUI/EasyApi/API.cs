using WVA_Connect_CSI.Errors;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WVA_Connect_CSI.WebTools
{
    class API
    {
        static public string Post(string endpoint, object jsonObject)
        {
            try
            {
                // Convert the object to a json string
                string json = JsonConvert.SerializeObject(jsonObject);

                // Convert json string to a byte array
                var encoding = new UTF8Encoding();
                byte[] byteArray = encoding.GetBytes(json);

                // Build the request object
                var request = (HttpWebRequest)WebRequest.Create(endpoint);
                    request.ContentLength = byteArray.Length;
                    request.ContentType = @"application/json";
                    request.Method = "POST";

                // Open up a stream and write our data to it
                using (Stream dataStream = request.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                }

                // Get the response object
                var webResponse = request.GetResponse();

                // Read data from the response object 
                using (Stream responseStream = webResponse.GetResponseStream())
                {
                    using (var reader = new StreamReader(responseStream, Encoding.UTF8))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
            catch (Exception x)
            {
                Error.ReportOrLog(x);
                return null;
            }
        }
    }
}
