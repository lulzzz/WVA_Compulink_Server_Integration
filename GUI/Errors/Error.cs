using WVA_Compulink_Server_Integration.EasyApi;
using WVA_Compulink_Server_Integration.Utility.Files;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WVA_Compulink_Server_Integration.Errors
{
    class Error
    {
        public static void ReportOrLog(Exception e)
        {
            try
            {
                JsonError error = new JsonError()
                {
                    ActNum = $"(ApiKey={File.ReadAllText(Paths.ApiKeyFile)})",
                    Error = e.ToString(),
                    Application = Assembly.GetCallingAssembly().GetName().Name,
                    AppVersion = System.Reflection.Assembly.GetEntryAssembly().GetName().Version.ToString()
                };

                if (!ErrorReported(error))
                    WriteError(error.Error);
            }
            catch (Exception error)
            {
                WriteError(error.ToString());
            }
        }

        private static bool ErrorReported(JsonError error)
        {
            try
            {
                string endpoint = $"{Paths.WisVisErrors}";
                string strResponse = API.Post(endpoint, error);
                bool messageSent = JsonConvert.DeserializeObject<bool>(strResponse);

                return messageSent;
            }
            catch
            {
                return false;
            }
        }

        private static void WriteError(string exceptionMessage)
        {
            try
            {
                string time = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");

                if (!Directory.Exists(Paths.ErrorLogDir))
                {
                    Directory.CreateDirectory(Paths.ErrorLogDir);
                }

                if (!File.Exists(Paths.ErrorLogDir + $@"\Error_{time}.txt"))
                {
                    var file = File.Create(Paths.ErrorLogDir + $@"\Error_{time}.txt");
                    file.Close();
                }

                using (StreamWriter writer = new StreamWriter((Paths.ErrorLogDir + $@"\Error_{time}.txt"), true))
                {
                    writer.WriteLine("-----------------------------------------------------------------------------------");
                    writer.WriteLine("");
                    writer.WriteLine($"(ERROR.TIME_ENCOUNTERED: {time})");
                    writer.WriteLine($"(ERROR.MESSAGE: {exceptionMessage})");
                    writer.WriteLine("");
                    writer.WriteLine("-----------------------------------------------------------------------------------");
                    writer.Close();
                }
            }
            catch { }
        }
    }
}
