using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
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
using The_Ezio_Trilogy_Launcher.Windows;

namespace The_Ezio_Trilogy_Launcher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static int NumberOfCores { get; set; }
        public static int NumberOfThreads { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            FindNumberOfCores();
        }

        // Finding Core Count of current PC
        // Needed for a tweak to hopefully fix the stutters via Affinity
        private async void FindNumberOfCores()
        {
            try
            {
                Log.Information("Trying to find number of Cores and Threads of this PC");
                // Create a query to get information about the processor.
                ObjectQuery query = new ObjectQuery("SELECT * FROM Win32_Processor");

                // Create a ManagementObjectSearcher to execute the query.
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);

                // Get the collection of management objects.
                ManagementObjectCollection queryCollection = searcher.Get();

                foreach (ManagementObject m in queryCollection)
                {
#pragma warning disable CS8604 // Possible null reference argument.
                    NumberOfCores = int.Parse(m["NumberOfCores"].ToString());
#pragma warning disable CS8604 // Possible null reference argument.
                    NumberOfThreads = int.Parse(m["NumberOfLogicalProcessors"].ToString());
#pragma warning restore CS8604 // Possible null reference argument.

                    Log.Information($"Number of Cores: {NumberOfCores}");
                    Log.Information($"Number of Threads: {NumberOfThreads}");
                }
                await Task.Delay(10);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error:");
            }
        }

        private async void Exit_Click(object sender, RoutedEventArgs e)
        {
            Log.Information("Exiting");
            await Task.Delay(10);
            Environment.Exit(0);
        }

        private void OpenACII_Click(object sender, RoutedEventArgs e)
        {
            Log.Information("Opening Assassin's Creed 2 Launcher");
            try
            {
                AssassinsCreed2 ac2 = new AssassinsCreed2();
                this.Visibility = Visibility.Hidden;
                ac2.ShowDialog();
                this.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error:");
                return;
            }
        }

        private void OpenACB_Click(object sender, RoutedEventArgs e)
        {
            Log.Information("Opening Assassin's Creed Brotherhood Launcher");
        }

        private void OpenACR_Click(object sender, RoutedEventArgs e)
        {
            Log.Information("Opening Assassin's Creed Revelations Launcher");
        }
    }
}
