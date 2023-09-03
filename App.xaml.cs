using Serilog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Management;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace The_Ezio_Trilogy_Launcher
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        // This is needed for Console to show up when using argument -console
        [DllImport("Kernel32")]
        public static extern void AllocConsole();
        [DllImport("Kernel32")]
        public static extern void FreeConsole();

        private bool start = true;

        // Number of Cores and Threads this PC has. Needed to set Affinity for the process
        public static int NumberOfCores { get; set; }
        public static int NumberOfThreads { get; set; }

        // Paths to game installations
        public static string? AC2Path { get; set; }
        public static string? ACBPath { get; set; }
        public static string? ACRPath { get; set; }

        // Supported Resolutions by this Monitor
        List<Resolution> compatibleResolutions = new List<Resolution>();

        public App()
        {
            InitializeComponent();
            // Activating Logging Tool
            Log.Logger = new LoggerConfiguration()
            .WriteTo.Console(
                outputTemplate: "{Timestamp:dd-MM-yyyy HH:mm:ss}|{Level}|{Message}{NewLine}{Exception}")
            //.WriteTo.File("Installer Logs.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();
        }

        // Grabbing all of the game installations
        private async Task FindGameInstallations()
        {
            try
            {
                Log.Information("Grabbing all of the paths to game installations");
                if (System.IO.Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments) + @"\Assassin's Creed - The Ezio Trilogy Remastered"))
                {
                    if (System.IO.Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments) + @"\Assassin's Creed - The Ezio Trilogy Remastered").Count() > 0)
                    {
                        foreach (string path in System.IO.Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments) + @"\Assassin's Creed - The Ezio Trilogy Remastered"))
                        {
                            FileInfo fileInfo = new FileInfo(path);
                            if (fileInfo.Name == "AC2Path.txt")
                            {
                                Log.Information("AC2 is installed.");
                                using (StreamReader sr = new StreamReader(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments) + @"\Assassin's Creed - The Ezio Trilogy Remastered\" + fileInfo.Name))
                                {
                                    AC2Path = sr.ReadLine();
                                    Log.Information($"AC2 Installation Path: {AC2Path}");
                                }
                            }
                        }
                    }
                }
                await Task.Delay(10);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "");
            }
        }

        // Finding Core Count of current PC
        // Needed for a tweak to hopefully fix the stutters via Affinity
        private async Task FindNumberOfCores()
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

        // Finding all of the supported resolutions
        private async Task FindSupportedResolutions()
        {
            try
            {
                Screen[] allScreens = Screen.AllScreens;
                int resolutionWidth = 0;
                int resolutionHeight = 0;
                foreach (Screen screen in allScreens)
                {
                    if (resolutionWidth < screen.Bounds.Width)
                    {
                        resolutionWidth = screen.Bounds.Width;
                        resolutionHeight = screen.Bounds.Height;
                    };
                }
                using (StreamReader sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("The_Ezio_Trilogy_Launcher.Assets.ListofSupportedResolutions.txt")))
                {
                    string line = sr.ReadLine();
                    while (line != null)
                    {
                        string[] splitLine = line.Split('x');
                        if (double.Parse(splitLine[0]) < resolutionWidth && double.Parse(splitLine[1]) < resolutionHeight)
                        {
                            Resolution newRes = new Resolution();
                            newRes.Res = line;
                            newRes.Width = double.Parse(splitLine[0]);
                            newRes.Height = double.Parse(splitLine[1]);
                            compatibleResolutions.Add(newRes);
                        }
                        else if (double.Parse(splitLine[0]) == resolutionWidth && double.Parse(splitLine[1]) == resolutionHeight)
                        {
                            Resolution newRes = new Resolution();
                            newRes.Res = line;
                            newRes.Width = double.Parse(splitLine[0]);
                            newRes.Height = double.Parse(splitLine[1]);
                            compatibleResolutions.Add(newRes);
                            break;
                        }
                        line = sr.ReadLine();
                    }
                }
                Log.Information("Compatible Resolutions:");
                foreach (Resolution res in compatibleResolutions)
                {
                    Log.Information(res.Res);
                }
                await Task.Delay(1);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "");
            }
        }

        // At startup it checks for launch arguments
        private async void Application_Startup(object sender, StartupEventArgs e)
        {
            if (start)
            {
                start = false;
                Serilog.Log.Logger = Log.Logger;
                foreach (var argument in e.Args)
                {
                    switch (argument)
                    {
                        case "-console":
                            AllocConsole();
                            break;
                        default:
                            break;
                    }
                }
                await FindGameInstallations();
                await FindNumberOfCores();
                await FindSupportedResolutions();
            }
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            Log.CloseAndFlush();
        }
    }
}
