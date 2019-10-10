using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows;
using WVA_Connect_CSI.AsyncJobs;
using System.IO;
using WVA_Connect_CSI.Utility.Files;
using WVA_Connect_CSI.Errors;
using System.Reflection;
using WVA_Connect_CSI.Memory;
using WVA_Connect_CSI.Services;
using WVA_Connect_CSI.ViewModels;
using System.ComponentModel;
using WVA_Connect_CSI.ODBC;
using System.Windows.Media.Imaging;
using static WVA_Connect_CSI.Views.MainView;
using WVA_Connect_CSI.Data;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using WVA_Connect_CSI.Views;

namespace WVA_Connect_CSI
{
    public partial class MainWindow : Window
    {
        public static bool ShouldCheckForInactivity = true;

        private CancellationTokenSource tokenSource = new CancellationTokenSource();
        private readonly Task inactivityTask;
        private bool DsnConnectionIsGood { get; set; }
        private readonly BackgroundWorker CheckDsnConnectionStatusWorker = new BackgroundWorker();
        private readonly BackgroundWorker UpdateDsnConnectionStatusWorker = new BackgroundWorker();

        public MainWindow()
        {
            try
            {
                DeleteNetCoreIcon();
                InitializeComponent();
                SetTitle();
                SetUpFiles();
                SetUpDatabase();
                SetUpServiceHost();
                SetContentControl();
                StartWorkers();
                TaskManager.StartAllJobs();

                inactivityTask = new Task(StartInactivityTimer, tokenSource.Token);
                inactivityTask.Start();
            }
            catch (Exception ex)
            {
                Error.ReportOrLog(ex);
            }
        }

        //
        // Check for user inacvitity 
        //

        [DllImport("user32.dll")]
        public static extern Boolean GetLastInputInfo(ref tagLASTINPUTINFO plii);
        public struct tagLASTINPUTINFO
        {
            public uint cbSize; 
            public Int32 dwTime;
        }

        private async void StartInactivityTimer()
        {
            // Keeps this thread alive
            while (true)
                // Only do work on this thread when we want to
                if (ShouldCheckForInactivity)
                    try     { CheckUserInactivity(); }
                    finally { Thread.Sleep(5000); }
                else
                    Thread.Sleep(2500);
        }

        private void CheckUserInactivity()
        {
            try
            {
                tagLASTINPUTINFO LastInput = new tagLASTINPUTINFO();
                Int32 IdleTime;

                LastInput.cbSize = (uint)Marshal.SizeOf(LastInput);
                LastInput.dwTime = 0;

                if (GetLastInputInfo(ref LastInput))
                {
                    IdleTime = System.Environment.TickCount - LastInput.dwTime;
                    Trace.WriteLine($"*** IdleTime: {IdleTime}ms ***");

                    if (IdleTime > 300000)
                    {
                        Dispatcher.Invoke(new UpdateUICallBack(ForceLogOff));
                    }
                }
            }
            catch (Exception ex)
            {
                Error.ReportOrLog(ex);
            }
        }

        private void ForceLogOff()
        {
            foreach (Window window in Application.Current.Windows)
                if (window.GetType() == typeof(MainWindow))
                    (window as MainWindow).MainContentControl.DataContext = new MainView();
        }

        //
        // Window Setup Functions
        //

        private void SetTitle()
        {
            Title = $"v{FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion}";
        }

        private void SetContentControl()
        {
            MainContentControl.DataContext = new MainViewModel();
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

        private void StartWorkers()
        {
            // Check DSN connection status, update app icon good or bad connection
            CheckDsnConnectionStatusWorker.DoWork += CheckDsnConnectionStatusWorker_DoWork;
            UpdateDsnConnectionStatusWorker.DoWork += UpdateDsnConnectionStatusWorker_DoWork;

            CheckDsnConnectionStatusWorker.RunWorkerAsync();
            UpdateDsnConnectionStatusWorker.RunWorkerAsync();
        }

        private void SetUpServiceHost()
        {
            if (!ServiceHost.IsInstalled())
            {
                ServiceHost.Install();
                Thread.Sleep(250);

                if (!ServiceHost.IsInstalled())
                    MessageBox.Show("WVA_Connect_CSI service host not installed!", "Error", MessageBoxButton.OK);
            }
        }

        public void SetUpFiles()
        {
            if (!Directory.Exists($@"{Paths.DataDir}"))
                Directory.CreateDirectory($@"{Paths.DataDir}");

            if (!Directory.Exists($@"{Paths.ConfigDir}"))
                Directory.CreateDirectory($@"{Paths.ConfigDir}");
        }

        public void SetUpDatabase()
        {
            new Database().SetUpRoles();
        }

        // 
        // Background worker tasks
        //

        private void CheckDsnStatus()
        {
            try
            {
                DsnConnectionIsGood = DsnConnectionTester.IsGoodConnection();
            }
            catch (Exception x)
            {
                Error.WriteError(x.ToString());
            }
        }

        private void UpdateAppIcon()
        {
            try
            {
                Icon = DsnConnectionIsGood ? BitmapFrame.Create(Application.GetResourceStream(new Uri(@"/Resources/GreenBubble.png", UriKind.Relative)).Stream) : BitmapFrame.Create(Application.GetResourceStream(new Uri(@"/Resources/RedBubble.jpg", UriKind.Relative)).Stream);
            }
            catch (Exception x)
            {
                Error.WriteError(x.ToString());
            }
        }

        private void CheckDsnConnectionStatusWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                CheckDsnStatus();
                Thread.Sleep(2000);
            }
        }

        private void UpdateDsnConnectionStatusWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                Dispatcher.Invoke(
                  new UpdateUICallBack(UpdateAppIcon)
                  );
                Thread.Sleep(2000);
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            tokenSource.Cancel();
        }
    }
}
