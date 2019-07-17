using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Media.Imaging;
using WVA_Compulink_Server_Integration.AsyncJobs;
using System.IO;
using WVA_Compulink_Server_Integration.Utility.Files;
using WVA_Compulink_Server_Integration.Errors;
using WVA_Compulink_Server_Integration.Updates;
using System.Threading.Tasks;
using System.Reflection;
using WVA_Compulink_Server_Integration.ODBC;
using WVA_Compulink_Server_Integration.Memory;

namespace WVA_Compulink_Server_Integration
{
    public partial class MainWindow : Window
    {
        private bool ServerIsRunning { get; set; }
        private bool DsnConnectionIsGood { get; set; }

        private readonly BackgroundWorker CheckServerWorker = new BackgroundWorker();
        private readonly BackgroundWorker UpdateStatusWorker = new BackgroundWorker();
        private readonly BackgroundWorker CheckDsnConnectionStatusWorker = new BackgroundWorker();
        private readonly BackgroundWorker UpdateDsnConnectionStatusWorker = new BackgroundWorker();

        public MainWindow()
        {
            DeleteNetCoreIcon();
            CloseDuplicateApp();
            InitializeComponent();
            SetTitle();
            SetUpFiles();
            SetupConfig();
            StartWorkers();
            TaskManager.StartAllJobs();
            Task.Run(() => Updater.RunUpdates());
        }

        private void SetTitle()
        {
            Title = $"v{FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion}";
        }

        private void DeleteNetCoreIcon()
        {
            string netCoreShortcut = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Microsoft® .NET Core Framework.lnk";

            if (File.Exists(netCoreShortcut))
                File.Delete(netCoreShortcut);
        }

        private void CloseDuplicateApp()
        {
            if (Process.GetProcessesByName(Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly().Location)).Count() > 1)
                Environment.Exit(0);
        }

        public bool SetupConfig()
        {
            // Moves config file to main app directory in app data
            if (File.Exists(Paths.ConfigDesktop))
                File.Move(Paths.ConfigDesktop, Paths.WvaConfigFile);

            if (File.Exists(Paths.ConfigDocuments))
                File.Move(Paths.ConfigDocuments, Paths.WvaConfigFile);

            if (File.Exists(Paths.ConfigInstallDir))
                File.Move(Paths.ConfigInstallDir, Paths.WvaConfigFile);

            // Sets up config file in memory
            if (File.Exists(Paths.WvaConfigFile))
            {
                try
                {
                    new Storage();
                    return true;
                }
                catch (Exception ex)
                {
                    Error.ReportOrLog(ex);
                    return false;
                }
            }
            else
            {
                return false;

            }
        }

        public void SetUpFiles()
        {
            if (!Directory.Exists($@"{Paths.DataDir}"))
                Directory.CreateDirectory($@"{Paths.DataDir}");
        }

        private void StartWorkers()
        {
            // Check DSN connection status, update app icon good or bad connection
            CheckDsnConnectionStatusWorker.DoWork += CheckDsnConnectionStatusWorker_DoWork;
            UpdateDsnConnectionStatusWorker.DoWork += UpdateDsnConnectionStatusWorker_DoWork;

            CheckDsnConnectionStatusWorker.RunWorkerAsync();
            UpdateDsnConnectionStatusWorker.RunWorkerAsync();

            // Check for server status, update bubble if running or not
            CheckServerWorker.DoWork += CheckServerWorker_DoWork;
            UpdateStatusWorker.DoWork += UpdateStatusWorker_DoWork;

            CheckServerWorker.RunWorkerAsync();
            UpdateStatusWorker.RunWorkerAsync();
        }

        private void ShowServerStatus()
        {
            try
            {
                ServerStatusImage.Source = ServerIsRunning ? new BitmapImage(new Uri(@"/Resources/GreenBubble.png", UriKind.Relative)) : new BitmapImage(new Uri(@"/Resources/RedBubble.jpg", UriKind.Relative));
            }
            catch (Exception x) { Error.ReportOrLog(x); }
        }

        private void UpdateAppIcon()
        {
            try
            {
                Icon = DsnConnectionIsGood ? BitmapFrame.Create(Application.GetResourceStream(new Uri(@"/Resources/GreenBubble.png", UriKind.Relative)).Stream) : BitmapFrame.Create(Application.GetResourceStream(new Uri(@"/Resources/RedBubble.jpg", UriKind.Relative)).Stream);
            }
            catch (Exception x) { Error.ReportOrLog(x); }
        }

        private void CheckServerStatus()
        {
            try
            {
                ServerIsRunning = Process.GetProcessesByName("dotnet").Length == 0 ? false : true;
            }
            catch (Exception x) { Error.ReportOrLog(x); }
        }

        private void CheckDsnStatus()
        {
            try
            {
                DsnConnectionIsGood = DsnConnectionTester.IsGoodConnection();
            }
            catch (Exception x) { Error.ReportOrLog(x); }
        }

        private void StartServerInstance()
        {
            try
            {
                string dirName = GetServerDirectoryName();
                string cdString = $"cd {dirName}";
                string cmdString = $"\"{dirName}dotnet\" WVA_Compulink_Server_Integration.dll";

                Process cmd = new Process();
                cmd.StartInfo.FileName = "cmd.exe";
                cmd.StartInfo.RedirectStandardInput = true;
                cmd.StartInfo.RedirectStandardOutput = true;
                cmd.StartInfo.CreateNoWindow = true;
                cmd.StartInfo.UseShellExecute = false;
                cmd.Start();

                cmd.StandardInput.WriteLine(cdString);
                cmd.StandardInput.Flush();
                cmd.StandardInput.WriteLine(cmdString);
                cmd.StandardInput.Flush();
                cmd.StandardInput.Close();
            }
            catch (Exception x)
            {
                Error.ReportOrLog(x);
            }
        }

        private void KillServerInstances()
        {
            try
            {
                Process cmd = new Process();
                cmd.StartInfo.FileName = "cmd.exe";
                cmd.StartInfo.RedirectStandardInput = true;
                cmd.StartInfo.RedirectStandardOutput = true;
                cmd.StartInfo.CreateNoWindow = true;
                cmd.StartInfo.UseShellExecute = false;
                cmd.Start();

                cmd.StandardInput.WriteLine(@"Taskkill /IM dotnet.exe /F");
                cmd.StandardInput.Flush();
                cmd.StandardInput.Close();
            }
            catch (Exception x) { Error.ReportOrLog(x); }
        }

        private static string GetServerDirectoryName()
        {
            string path = Paths.AppDataLocal + $@"\{Assembly.GetCallingAssembly().GetName().Name}\";
            string[] dirs = Directory.GetDirectories(path, "app-*");

            return dirs[dirs.Length - 1] + @"\Server\"; // Returns the last item in the array (highest app version)
        }

        private void CheckServerWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                CheckServerStatus();
                Thread.Sleep(1000);
            }
        }

        public delegate void UpdateUICallBack();
        private void UpdateStatusWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                Dispatcher.Invoke(
                    new UpdateUICallBack(ShowServerStatus)
                    );
                Thread.Sleep(1000);
            }
        }

        private void CheckDsnConnectionStatusWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                CheckDsnStatus();
                Thread.Sleep(1500);
            }
        }

        private void UpdateDsnConnectionStatusWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                Dispatcher.Invoke(
                  new UpdateUICallBack(UpdateAppIcon)
                  );
                Thread.Sleep(1500);
            }
        }

        private void StartServerButton_Click(object sender, RoutedEventArgs e)
        {
            bool configLocated = SetupConfig();

            if (configLocated)
                StartServerInstance();
        }

        private void StopServerButton_Click(object sender, RoutedEventArgs e)
        {
            KillServerInstances();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            KillServerInstances();
        }

    }
}
