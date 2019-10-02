using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WVA_Connect_CSI.Errors;
using WVA_Connect_CSI.Memory;
using WVA_Connect_CSI.Models;
using WVA_Connect_CSI.Responses;
using WVA_Connect_CSI.Roles;
using WVA_Connect_CSI.Utility.Files;
using WVA_Connect_CSI.WebTools;

namespace WVA_Connect_CSI.Utility.ActionLogging
{
    public class ActionLogger
    {
        // -----------------------------------------------------------------------------------------------------
        // --------------------------------- LOGGING ACTIONS ---------------------------------------------------
        // -----------------------------------------------------------------------------------------------------

        static Role userRole;

        public static void Log(string actionLocation, string userName, int roleId, string actionMessage = null)
        {
            try
            {
                userRole = new Role(roleId, userName).DetermineRole();

                CreateLogFile();
                string file = GetLogFileName();

                string contents = actionMessage == null ? GetFileContents(actionLocation) : GetFileContents(actionLocation, actionMessage);

                WriteToLogFile(file, contents);
            }
            catch (Exception ex)
            {
                Error.ReportOrLog(ex);
            }
        }

        public static void Log(string actionLocation, Role role, string actionMessage = null)
        {
            try
            {
                userRole = role;
                CreateLogFile();
                string file = GetLogFileName();

                string contents = actionMessage == null ? GetFileContents(actionLocation) : GetFileContents(actionLocation, actionMessage);

                WriteToLogFile(file, contents);
            }
            catch (Exception ex)
            {
                Error.ReportOrLog(ex);
            }
        }

        private static void CreateLogFile()
        {
            string file = GetLogFileName();

            if (!Directory.Exists(Paths.TempDir))
                Directory.CreateDirectory(Paths.TempDir);

            if (!File.Exists(file))
                File.Create(file).Close(); ;
        }

        private static string GetLogFileName()
        {
            return $"{Paths.TempDir}CSI_Action_Log_{DateTime.Today.ToString("MM-dd-yy")}.txt";
        }

        private static string GetFileContents(string actionLocation)
        {
            try
            {
                string apiKey = GetApiKey();
                string time = DateTime.Now.ToString("hh:mm:ss");
                return $"ApiKey={apiKey} => MachName={Environment.MachineName} => EnvUserName={Environment.UserName} => UserRole={userRole.RoleId} => UserName={userRole.UserName} => {time} => {actionLocation}";
            }
            catch
            {
                return "";
            }
        }

        private static string GetApiKey()
        {
            try
            {
                return JsonConvert.DeserializeObject<WvaConfig>(File.ReadAllText(Paths.WvaConfigFile)).ApiKey ?? null;
            }
            catch (Exception ex)
            {
                Error.ReportOrLog(ex);
                return null;
            }
        }

        private static string GetFileContents(string actionLocation, string actionMessage)
        {
            return GetFileContents(actionLocation) + $" => {actionMessage}";
        }

        private static void WriteToLogFile(string file, string contents)
        {
            try
            {
                if (!File.Exists(file))
                    File.Create(file);

                var stream = File.AppendText(file);
                stream.WriteLine(contents);
                stream.Close();
            }
            catch (Exception ex)
            {
                Error.ReportOrLog(ex);
            }
        }

        // -----------------------------------------------------------------------------------------------------
        // --------------------------------- GETTING ACTION DATA -----------------------------------------------
        // -----------------------------------------------------------------------------------------------------

        public static List<ActionData> GetDataNotToday()
        {
            try
            {
                List<ActionData> listActionData = new List<ActionData>();

                var files = Directory.EnumerateFiles(Paths.TempDir, "CSI_Action_Log*").Where(x => !x.Contains(DateTime.Today.ToString("MM-dd-yy")));

                foreach (string file in files)
                {
                    if (File.Exists(file))
                    {
                        string content = File.ReadAllText(file);
                        listActionData.Add(new ActionData(file, content));
                    }
                }

                return listActionData;
            }
            catch
            {
                return null;
            }
        }

        public static List<ActionData> GetAllData()
        {
            try
            {
                List<ActionData> listActionData = new List<ActionData>();

                var files = Directory.EnumerateFiles(Paths.TempDir, "CSI_Action_Log*");

                foreach (string file in files)
                {
                    if (File.Exists(file))
                    {
                        string content = File.ReadAllText(file);
                        listActionData.Add(new ActionData(file, content));
                    }
                }

                return listActionData;
            }
            catch
            {
                return null;
            }
        }



        // -----------------------------------------------------------------------------------------------------
        // --------------------------------- SENDING ACTION DATA -----------------------------------------------
        // -----------------------------------------------------------------------------------------------------

        public static bool ReportData(string data)
        {
            try
            {
                var dataMessage = new JsonError()
                {
                    ActNum = GetApiKey(),
                    Error = data.ToString(),
                    Application = Assembly.GetCallingAssembly().GetName().Name,
                    AppVersion = System.Reflection.Assembly.GetEntryAssembly().GetName().Version.ToString()
                };

                string strResponse = API.Post(Paths.WisVisErrors, dataMessage);
                var response = JsonConvert.DeserializeObject<Response>(strResponse);

                return response.Status == "SUCCESS" ? true : false;
            }
            catch
            {
                return false;
            }
        }

        public static void ReportAllDataNow()
        {
            var listActionData = GetAllData();
            string data = "";

            foreach (ActionData d in listActionData)
            {
                data += d.Content;
            }

            // Don't report if there is not data (user clicks button multiple times)
            if (data.Trim() != "")
                ReportData(data);
        }
    }
}
