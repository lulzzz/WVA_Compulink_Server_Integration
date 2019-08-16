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

namespace WVA_Connect_CSI
{
    public partial class MainWindow : Window
    {
        private bool DsnConnectionIsGood { get; set; }

        private readonly BackgroundWorker CheckDsnConnectionStatusWorker = new BackgroundWorker();
        private readonly BackgroundWorker UpdateDsnConnectionStatusWorker = new BackgroundWorker();

        public MainWindow()
        {
            DeleteNetCoreIcon();
            InitializeComponent();
            SetTitle();
            SetUpFiles();
            SetUpServiceHost();
            SetContentControl();
            StartWorkers();
            TaskManager.StartAllJobs();
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
                Error.ReportOrLog(x);
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
                Error.ReportOrLog(x);
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

    }
}
