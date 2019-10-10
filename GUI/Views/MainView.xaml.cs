using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WVA_Connect_CSI.Errors;
using WVA_Connect_CSI.Memory;
using WVA_Connect_CSI.Services;
using WVA_Connect_CSI.Updates;
using WVA_Connect_CSI.Utility.Files;

namespace WVA_Connect_CSI.Views
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : UserControl
    {
        private bool ServerIsRunning { get; set; }

        private readonly BackgroundWorker CheckServerWorker = new BackgroundWorker();
        private readonly BackgroundWorker UpdateStatusWorker = new BackgroundWorker();

        public MainView()
        {
            InitializeComponent();
            ResizeView();
            StartWorkers();
        }

        private void ResizeView()
        {
            foreach (Window window in Application.Current.Windows)
                if (window.GetType() == typeof(MainWindow))
                {
                    (window as MainWindow).MinHeight = 400;
                    (window as MainWindow).MinWidth = 455;
                    (window as MainWindow).Height = 400;
                    (window as MainWindow).Width = 455;
                    (window as MainWindow).ResizeMode = ResizeMode.CanMinimize;
                }
        }

        private void StartWorkers()
        {
            // Check for server status, update bubble if running or not
            CheckServerWorker.DoWork += CheckServerWorker_DoWork;
            UpdateStatusWorker.DoWork += UpdateStatusWorker_DoWork;

            CheckServerWorker.RunWorkerAsync();
            UpdateStatusWorker.RunWorkerAsync();
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

        private async void CheckForUpdates()
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
                        await Updater.ForceUpdate();
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

        private void UninstallService()
        {
            try
            {
                if (ServiceHost.IsRunning())
                    ServiceHost.Stop();

                ServiceHost.Uninstall();
                Thread.Sleep(1000);

                if (ServiceHost.IsInstalled())
                    MessageBox.Show("Service was not removed successfully.", "", MessageBoxButton.OK);
                else
                    MessageBox.Show("Service successfully removed.", "", MessageBoxButton.OK);
            }
            catch (Exception ex)
            {
                Error.ReportOrLog(ex);
            }
        }

        private void AdminLogin()
        {
            foreach (Window window in Application.Current.Windows)
                if (window.GetType() == typeof(MainWindow))
                    (window as MainWindow).MainContentControl.DataContext = new LoginView();
        }

        private void StartServiceHost()
        {
            try
            {
                if (!ServiceHost.IsInstalled())
                    ServiceHost.Install();

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

        //
        // Background workers
        //

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

        //
        // Looping Methods
        //

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

        //
        // UI Events
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

        private void CheckForUpdatesButton_Click(object sender, RoutedEventArgs e)
        {
            CheckForUpdates();
        }

        private void UninstallServiceButton_Click(object sender, RoutedEventArgs e)
        {
            UninstallService();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            AdminLogin();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

    }
}
