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
using The_Ezio_Trilogy_Launcher.Classes;

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
        /// True if uMod is enabled
        /// False if uMod is disabled
        /// </summary>
        public static bool AC2uModStatus = false;

        /// <summary>
        /// True if uMod is enabled
        /// False if uMod is disabled
        /// </summary>
        public static bool ACBuModStatus = false;

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

        /// <summary>
        /// Sets the process affinity according to the number of cores/threads in the PC
        /// </summary>
        public static AffinityManager ProcessAffinityManager { get; } = new AffinityManager();

        public App()
        {
            InitializeComponent();
        }

        // Misc
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
        /// Checks if uMod is enabled or disabled.
        /// </summary>
        private async Task uModStatus()
        {
            try
            {
                // AC2
                Log.Information("Checking if uMod is enabled for Assassin's Creed 2");
                if (System.IO.File.Exists(AC2Path + @"\uMod\Status.txt"))
                {
                    string[] statusFile = File.ReadAllLines(AC2Path + @"\uMod\Status.txt");
                    foreach (string status in statusFile)
                    {
                        if (status.StartsWith("Enabled"))
                        {
                            string[] splitLine = status.Split('=');
                            if (int.Parse(splitLine[1]) == 1)
                            {
                                Log.Information("uMod is enabled for Assassin's Creed 2");
                                AC2uModStatus = true;
                            }
                            else
                            {
                                Log.Information("uMod is disabled for Assassin's Creed 2");
                                AC2uModStatus = false;
                            }
                        }
                    }
                }
                // ACB
                Log.Information("Checking if uMod is enabled for Assassin's Creed Brotherhood");
                if (System.IO.File.Exists(ACBPath + @"\uMod\Status.txt"))
                {
                    string[] statusFile = System.IO.File.ReadAllLines(ACBPath + @"\uMod\Status.txt");
                    foreach (string status in statusFile)
                    {
                        if (status.StartsWith("Enabled"))
                        {
                            string[] splitLine = status.Split('=');
                            if (int.Parse(splitLine[1]) == 1)
                            {
                                Log.Information("uMod is enabled for Assassin's Creed Brotherhood");
                                ACBuModStatus = true;
                            }
                            else
                            {
                                Log.Information("uMod is disabled for Assassin's Creed Brotherhood");
                                ACBuModStatus = false;
                            }
                        }
                    }
                }
                GC.Collect();
                await Task.Delay(1);
            }
            catch (Exception ex)
            {
                Log.Information(ex, "");
                System.Windows.MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Starts the game
        /// </summary>
        /// <param name="executableName">Name of the executable</param>
        /// <param name="workingDirectory">Directory where the game is</param>
        /// <param name="uMod">Does the game use uMod</param>
        /// <returns></returns>
        public async Task StartGame(string executableName, string? workingDirectory, bool uMod, bool skipLauncher=false)
        {
            try
            {
                Process[] gameProcesses = Process.GetProcessesByName(executableName);
                if (gameProcesses.Length <= 0)
                {
                    Process gameProcess = new Process();
                    gameProcess.StartInfo.WorkingDirectory = workingDirectory;
                    gameProcess.StartInfo.FileName = $"{executableName}.exe";
                    gameProcess.StartInfo.UseShellExecute = true;
                    Process uModProcess = new Process();
                    if (uMod)
                    {
                        uModProcess.StartInfo.WorkingDirectory = $"{workingDirectory}\\uMod";
                        uModProcess.StartInfo.FileName = "uMod.exe";
                        uModProcess.StartInfo.UseShellExecute = true;
                        uModProcess.Start();
                        gameProcess.Start();
                        Log.Information("Game is starting");
                        await Task.Delay(5000);
                    }
                    else
                    {
                        gameProcess.Start();
                        Log.Information("Game is starting");
                    }
                    Log.Information("Setting game affinity based on CPU Core/Thread Count");
                    gameProcesses = Process.GetProcessesByName(executableName);
                    while (gameProcesses.Length <= 0)
                    {
                        await Task.Delay(1000);
                        gameProcesses = Process.GetProcessesByName(executableName);
                    }
                    foreach (Process process in gameProcesses)
                    {
                        process.PriorityClass = ProcessPriorityClass.AboveNormal;
                        await ProcessAffinityManager.SetProcessAffinity(process.ProcessName);
                    }
                    Log.Information("Game started");
                    while (gameProcesses.Length > 0)
                    {
                        await Task.Delay(1000);
                        gameProcesses = Process.GetProcessesByName(executableName);
                    }
                    if (uMod)
                    {
                        try
                        {
                            if (Process.GetProcessById(uModProcess.Id) != null)
                            {
                                uModProcess.CloseMainWindow();
                                Log.Information("Game Closed");
                            }
                        }
                        catch (Exception)
                        {
                            Log.Information("uMod is already closed.");
                        }
                    }
                    GC.Collect();
                    await Task.Delay(1);
                    if (skipLauncher)
                    {
                        Environment.Exit(0);
                    }
                }
                else
                {
                    Log.Information($"Game {executableName} is already running");
                    System.Windows.MessageBox.Show($"Game {executableName} is already running.");
                }

                GC.Collect();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "");
                return;
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
                if (e.Args.Contains("-console"))
                {
                    AllocConsole();
                    logging = true;
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
                await uModStatus();
                foreach (var argument in e.Args)
                {
                    switch (argument)
                    {
                        case "-AC2":
                            MainWindow.Visibility = Visibility.Hidden;
                            await StartGame("AssassinsCreedIIGame", AC2Path, AC2uModStatus, true);
                            break;
                        case "-ACB":
                            MainWindow.Visibility = Visibility.Hidden;
                            await StartGame("ACBSP", ACBPath, ACBuModStatus, true);
                            break;
                        case "-ACR":
                            MainWindow.Visibility = Visibility.Hidden;
                            await StartGame("ACRSP", ACRPath, false, true);
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            Log.CloseAndFlush();
        }
    }
}
