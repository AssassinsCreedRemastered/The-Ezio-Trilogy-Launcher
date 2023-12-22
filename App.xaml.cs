using Serilog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;

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

        private bool logging = false;

        // Used to detect refresh Rate
        [DllImport("user32.dll")]
        private static extern int EnumDisplaySettings(string? deviceName, int modeNum, ref DEVMODE devMode);

        [StructLayout(LayoutKind.Sequential)]
        private struct DEVMODE
        {
            private const int CCHDEVICENAME = 32;
            private const int CCHFORMNAME = 32;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = CCHDEVICENAME)]
            public string dmDeviceName;
            public short dmSpecVersion;
            public short dmDriverVersion;
            public short dmSize;
            public short dmDriverExtra;
            public int dmFields;
            public int dmPositionX;
            public int dmPositionY;
            public int dmDisplayOrientation;
            public int dmDisplayFixedOutput;
            public short dmColor;
            public short dmDuplex;
            public short dmYResolution;
            public short dmTTOption;
            public short dmCollate;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = CCHFORMNAME)]
            public string dmFormName;

            public short dmLogPixels;
            public int dmBitsPerPel;
            public int dmPelsWidth;
            public int dmPelsHeight;
            public int dmDisplayFlags;
            public int dmDisplayFrequency;
        }

        private bool start = true;

        /// <summary>
        /// Stores how many Cores the CPU has
        /// </summary>
        public static int NumberOfCores { get; set; }

        /// <summary>
        /// Stores how many Threads the CPU has
        /// </summary>
        public static int NumberOfThreads { get; set; }

        /// <summary>
        /// Path to Assassin's Creed 2 installation folder
        /// </summary>
        public static string? AC2Path { get; set; }

        /// <summary>
        /// Path to Assassin's Creed:Brotherhood installation folder
        /// </summary>
        public static string? ACBPath { get; set; }

        /// <summary>
        /// Path to Assassin's Creed:Revelations installation folder
        /// </summary>
        public static string? ACRPath { get; set; }

        /// <summary>
        /// List of supported Resolutions by users Monitor
        /// </summary>
        public static List<Resolution> compatibleResolutions = new List<Resolution>();

        /// <summary>
        /// List of supported Refresh Rates by users Monitor
        /// </summary>
        public static List<int> compatibleRefreshRates = new List<int>();

        /// <summary>
        /// Monitor's Refresh Rate
        /// </summary>
        public static double RefreshRate { get; set; }

        public App()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Grabs all of the game installations paths
        /// </summary>
        private async Task FindGameInstallations()
        {
            try
            {
                Log.Information("Grabbing all of the paths to game installations");
                // Checks if launcher's folder exists
                if (System.IO.Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments) + @"\Assassin's Creed - The Ezio Trilogy Remastered"))
                {
                    // Checks if there's something in launchers folder
                    if (System.IO.Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments) + @"\Assassin's Creed - The Ezio Trilogy Remastered").Count() > 0)
                    {
                        // For every file in launchers folder
                        foreach (string path in System.IO.Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments) + @"\Assassin's Creed - The Ezio Trilogy Remastered"))
                        {
                            FileInfo fileInfo = new FileInfo(path);
                            // If it's AC2Path.txt then it adds the path inside of it to AC2Path variable
                            if (fileInfo.Name == "AC2Path.txt")
                            {
                                Log.Information("Assassin's Creed 2 is installed.");
                                using (StreamReader sr = new StreamReader(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments) + @"\Assassin's Creed - The Ezio Trilogy Remastered\" + fileInfo.Name))
                                {
                                    AC2Path = sr.ReadLine();
                                    Log.Information($"Assassin's Creed 2 Installation Path: {AC2Path}");
                                }
                            }
                            // If it's ACBPath.txt then it adds the path inside of it to ACBPath variable
                            else if (fileInfo.Name == "ACBPath.txt")
                            {
                                Log.Information("Assassin's Creed Brotherhood is installed.");
                                using (StreamReader sr = new StreamReader(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments) + @"\Assassin's Creed - The Ezio Trilogy Remastered\" + fileInfo.Name))
                                {
                                    ACBPath = sr.ReadLine();
                                    Log.Information($"Assassin's Creed Brotherhood Installation Path: {ACBPath}");
                                }
                            }
                            // If it's ACRPath.txt then it adds the path inside of it to ACRPath variable
                            else if (fileInfo.Name == "ACRPath.txt")
                            {
                                Log.Information("Assassin's Creed Revelations is installed.");
                                using (StreamReader sr = new StreamReader(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments) + @"\Assassin's Creed - The Ezio Trilogy Remastered\" + fileInfo.Name))
                                {
                                    ACRPath = sr.ReadLine();
                                    Log.Information($"Assassin's Creed Revelations Installation Path: {ACRPath}");
                                }
                            }
                        }
                    }
                }
                await Task.Delay(1);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "");
            }
        }

        /// <summary>
        /// Finds Core Count of the CPU. Needed for a tweak to fix the stutters via Affinity
        /// </summary>
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
                    NumberOfCores = int.Parse(m["NumberOfCores"].ToString());
                    NumberOfThreads = int.Parse(m["NumberOfLogicalProcessors"].ToString());
                    Log.Information($"Number of Cores: {NumberOfCores}");
                    Log.Information($"Number of Threads: {NumberOfThreads}");
                }
                await Task.Delay(1);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error:");
            }
        }

        /// <summary>
        /// Finds all of the resolutions supported by the users monitor.
        /// </summary>
        private async Task FindSupportedResolutions()
        {
            try
            {
                // This has every supported resolution for the current screen where the launcher is loading
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
                Log.Information($"Monitors Resolution: {resolutionWidth}x{resolutionHeight}");

                // Adding all of the resolutions supported by the game and the monitor to a List
                using (StreamReader sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("The_Ezio_Trilogy_Launcher.Assets.ListofSupportedResolutions.txt")))
                {
                    string? line = sr.ReadLine();
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
                await Task.Delay(1);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "");
            }
        }

        /// <summary>
        /// Detects the Refresh Rate of the monitor
        /// </summary>
        private async Task FindRefreshRate()
        {
            try
            {
                RefreshRate = 0;
                DEVMODE dm = new DEVMODE();
                dm.dmSize = (short)Marshal.SizeOf(typeof(DEVMODE));
                if (EnumDisplaySettings(null, -1, ref dm) != 0)
                {
                    RefreshRate = dm.dmDisplayFrequency;
                }

                // Adds all of the supported refresh rates to a List
                using (StreamReader sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("The_Ezio_Trilogy_Launcher.Assets.ListOfSupportedRefreshRates.txt")))
                {
                    string? line = sr.ReadLine();
                    while (line != null)
                    {
                        if (RefreshRate > double.Parse(line))
                        {
                            compatibleRefreshRates.Add(int.Parse(line));
                        }
                        else if (RefreshRate == double.Parse(line))
                        {
                            compatibleRefreshRates.Add(int.Parse(line));
                            break;
                        }
                        line = sr.ReadLine();
                    }
                }
                await Task.Delay(1);
                Log.Information($"Monitor's Refresh Rate: {RefreshRate}");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "");
            }
        }

        /// <summary>
        /// Checks for updates on Launch
        /// </summary>
        private async Task CheckForUpdates()
        {
            try
            {
                Log.Information("Checking for updates");
                string currentVersion = "";
                string newestVersion = "";
                using (StreamReader sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("The_Ezio_Trilogy_Launcher.Assets.Version.txt")))
                {
                    string? line = sr.ReadLine();
                    while (line != null)
                    {
                        if (line != "")
                        {
                            Log.Information("Current Version: " + line);
                            currentVersion = line;
                        }
                        line = sr.ReadLine();
                    }
                }
                HttpWebRequest SourceText = (HttpWebRequest)WebRequest.Create("https://raw.githubusercontent.com/AssassinsCreedRemastered/The-Ezio-Trilogy-Launcher/Version/Version.txt");
                SourceText.UserAgent = "Mozilla/5.0";
                var response = SourceText.GetResponse();
                var content = response.GetResponseStream();
                using (var reader = new StreamReader(content))
                {
                    string fileContent = reader.ReadToEnd();
                    string[] lines = fileContent.Split(new char[] { '\n' });
                    foreach (string line in lines)
                    {
                        if (line != "")
                        {
                            Log.Information("Newest Version: " + line);
                            newestVersion = line;
                        }
                    }
                }
                if (currentVersion == newestVersion)
                {
                    Log.Information("Newest version of the launcher is already installed");
                    GC.Collect();
                    await Task.Delay(1);
                    return;
                }
                else
                {
                    Log.Information("New version found.");
                    System.Windows.MessageBox.Show("New version of the launcher found. Click on the Update button to update the launcher.");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "");
                System.Windows.MessageBox.Show($"Error: {ex.Message}{Environment.NewLine}Possibly no internet connection");
            }
        }

        /// <summary>
        /// At startup it launches the Logger for debugging and checks for Launch arguments.
        /// </summary>
        /// <param name="e">Holds all of the launch Arguemnts as e.Args</param>
        private async void Application_Startup(object sender, StartupEventArgs e)
        {
            // This is to disable double startup
            if (start)
            {
                start = false;
                Serilog.Log.Logger = Log.Logger; // Initializing Logger
                // Checks for all of the Launch Arguments
                foreach (var argument in e.Args)
                {
                    switch (argument)
                    {
                        case "-console":
                            AllocConsole();
                            logging = true;
                            break;
                        default:
                            break;
                    }
                }
                // Creating Logger Configuration
                if (logging)
                {
                    Log.Logger = new LoggerConfiguration()
                    .WriteTo.Console(
                        outputTemplate: "{Timestamp:dd-MM-yyyy HH:mm:ss}|{Level}|{Message}{NewLine}{Exception}")
                    .WriteTo.File("Logs.txt", rollingInterval: RollingInterval.Day)
                    .CreateLogger();
                }
                else
                {
                    Log.Logger = new LoggerConfiguration()
                    .WriteTo.Console(
                        outputTemplate: "{Timestamp:dd-MM-yyyy HH:mm:ss}|{Level}|{Message}{NewLine}{Exception}")
                    //.WriteTo.File("Logs.txt", rollingInterval: RollingInterval.Day)
                    .CreateLogger();
                }
                await FindGameInstallations();
                await FindNumberOfCores();
                await FindSupportedResolutions();
                await FindRefreshRate();
                await CheckForUpdates();
            }
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            Log.CloseAndFlush();
        }
    }
}
