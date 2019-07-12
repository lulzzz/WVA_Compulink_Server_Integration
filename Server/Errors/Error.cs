using WVA_Compulink_Server_Integration.WebTools;
using WVA_Compulink_Server_Integration.Utilities.Files;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace WVA_Compulink_Server_Integration.Errors
{
    public class Error
    {
        public static void ReportOrLog(Exception e)
        {
            try
            {
                JsonError error = new JsonError()
                {
                    ActNum = $"Machine Name=({Environment.MachineName}) User Name =({Environment.UserName}) API Key =({Memory.Storage.Config?.ApiKey ?? "NOT SET"})",
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
                    Directory.CreateDirectory(Paths.ErrorLogDir);

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
