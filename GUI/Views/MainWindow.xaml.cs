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
using WVA_Compulink_Server_Integration.Services;
using System.Windows.Input;

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
            InitializeComponent();
            SetTitle();
            SetUpFiles();
            SetupConfig();
            SetUpServiceHost();
            StartWorkers();
            TaskManager.StartAllJobs();
            //Task.Run(() => Updater.RunUpdates());
        }


        //
        // Window Setup Functions
        //

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

        private void SetUpServiceHost()
        {
            if (!ServiceHost.IsInstalled())
            {
                ServiceHost.Install();
                Thread.Sleep(250);

                if (!ServiceHost.IsInstalled())
                    MessageBox.Show("WVA_Compulink_Server_Integration service host not installed!", "Error", MessageBoxButton.OK);
            }
        }

        public bool SetupConfig()
        {
            // Moves config file to main app directory in app data
            if (File.Exists(Paths.ConfigDesktop) && !File.Exists(Paths.WvaConfigFile))
                File.Move(Paths.ConfigDesktop, Paths.WvaConfigFile);

            if (File.Exists(Paths.ConfigDocuments) && !File.Exists(Paths.WvaConfigFile))
                File.Move(Paths.ConfigDocuments, Paths.WvaConfigFile);

            if (File.Exists(Paths.ConfigInstallDir) && !File.Exists(Paths.WvaConfigFile))
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

            if (!Directory.Exists($@"{Paths.ConfigDir}"))
                Directory.CreateDirectory($@"{Paths.ConfigDir}");
        }

        //
        // Main operations
        //

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

        private void CheckServerStatus()
        {
            try
            {
                ServerIsRunning = ServiceHost.IsRunning();
            }
            catch (Exception x)
            {
                Error.ReportOrLog(x);
            }
        }

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

        private void StartServiceHost()
        {
            try
            {
                if (!ServiceHost.IsRunning())
                    ServiceHost.Start();
            }
            catch (Exception x)
            {
                MessageBox.Show("An error has occurred while starting service host!", "Error", MessageBoxButton.OK);
                Error.ReportOrLog(x);
            }
        }

        private void KillServiceHost()
        {
            try
            {
                if (ServiceHost.IsRunning())
                    ServiceHost.Stop();
            }
            catch (Exception x)
            {
                Error.ReportOrLog(x);
            }
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

        //
        // Main elements for service control
        //


        private void StartServerButton_Click(object sender, RoutedEventArgs e)
        {
            bool configLocated = SetupConfig();

            if (configLocated)
                StartServiceHost();
        }

        private void StopServerButton_Click(object sender, RoutedEventArgs e)
        {
            KillServiceHost();
        }

        //
        // Extends window size, exposing more elements
        //

        private void DropDownButton_Click(object sender, RoutedEventArgs e)
        {
            if (Height == 400)
            {
                Height = 460;
                DropDownImage.Source = new BitmapImage(new Uri(@"/Resources/icons8-slide-up-32.png", UriKind.Relative));
            }
            else
            {
                Height = 400;
                DropDownImage.Source = new BitmapImage(new Uri(@"/Resources/icons8-down-button-32.png", UriKind.Relative));
            }
        }

        //
        // Hidden elements made visible by DropDownButton_Click 
        //

        private async void CheckForUpdatesButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Disable button clicks
                CheckForUpdatesButton.IsEnabled = false;

                // Wait while we check for updates
                Cursor = Cursors.Wait;
                bool updateAvailable = await Task.Run(() => Updater.UpdatesAvailable());
                Cursor = Cursors.Arrow;

                if (updateAvailable)
                {
                    MessageBoxResult result = MessageBox.Show("An update is available. Would you like to install it?", "", MessageBoxButton.YesNo);

                    if (result == MessageBoxResult.Yes) // Run an update if user clicks 'Yes' button
                    {
                        Cursor = Cursors.Wait;
                        await Updater.ForceRunUpdates();
                    }
                }
                else
                {
                    MessageBox.Show("No updates found! Your application is up-to-date.", "", MessageBoxButton.OK);
                }
            }
            catch (Exception ex)
            {
                Error.ReportOrLog(ex);
            }
            finally
            {
                Cursor = Cursors.Arrow;
                CheckForUpdatesButton.IsEnabled = true;
            }
        }
    }
}
