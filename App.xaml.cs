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
using System.Windows.Media;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

        // Number of Cores and Threads this PC has. Needed to set Affinity for the process
        public static int NumberOfCores { get; set; }
        public static int NumberOfThreads { get; set; }

        // Paths to game installations
        public static string? AC2Path { get; set; }
        public static string? ACBPath { get; set; }
        public static string? ACRPath { get; set; }

        // Supported Resolutions by this Monitor
        public static List<Resolution> compatibleResolutions = new List<Resolution>();

        public static List<int> compatibleRefreshRates = new List<int>();

        // Monitor's Refresh Rate
        public static double RefreshRate { get; set; }

        public App()
        {
            InitializeComponent();
            Log.Logger = new LoggerConfiguration()
            .WriteTo.Console(
                outputTemplate: "{Timestamp:dd-MM-yyyy HH:mm:ss}|{Level}|{Message}{NewLine}{Exception}")
            //.WriteTo.File("Logs.txt", rollingInterval: RollingInterval.Day)
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
                                Log.Information("Assassin's Creed 2 is installed.");
                                using (StreamReader sr = new StreamReader(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments) + @"\Assassin's Creed - The Ezio Trilogy Remastered\" + fileInfo.Name))
                                {
                                    AC2Path = sr.ReadLine();
                                    Log.Information($"Assassin's Creed 2 Installation Path: {AC2Path}");
                                }
                            }
                            else if (fileInfo.Name == "ACBPath.txt")
                            {
                                Log.Information("Assassin's Creed Brotherhood is installed.");
                                using (StreamReader sr = new StreamReader(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments) + @"\Assassin's Creed - The Ezio Trilogy Remastered\" + fileInfo.Name))
                                {
                                    ACBPath = sr.ReadLine();
                                    Log.Information($"Assassin's Creed Brotherhood Installation Path: {ACBPath}");
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
                await Task.Delay(1);
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
                Log.Information($"Monitors Resolution: {resolutionWidth}x{resolutionHeight}");
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

        // Detecting Refresh Rate of the monitor
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
                await FindRefreshRate();
            }
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            Log.CloseAndFlush();
        }
    }
}
