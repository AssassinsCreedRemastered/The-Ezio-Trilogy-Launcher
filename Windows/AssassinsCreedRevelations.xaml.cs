using Serilog;
using System;
using System.Collections.Generic;
    using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace The_Ezio_Trilogy_Launcher.Windows
{
    /// <summary>
    /// Interaction logic for AssassinsCreedRevelations.xaml
    /// </summary>
    public partial class AssassinsCreedRevelations : Window
    {
        public AssassinsCreedRevelations()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Holds all of the pages cached
        /// </summary>
        private Dictionary<string, Page> pageCache = new Dictionary<string, Page>();

        // Functions
        /// <summary>
        /// Holds all of the pages cached
        /// <param name="PageName">Name of the Page.</param>
        /// </summary>
        private void NavigateToPage(string PageName)
        {
            Log.Information($"Trying to navigate to {PageName}");
            switch (PageName)
            {
                case "Credits":

                    if (!pageCache.ContainsKey(PageName))
                    {

                        Log.Information("Page is not cached. Loading it and caching it for future use.");
                        ACR_Pages.Credits page = new ACR_Pages.Credits();
                        pageCache[PageName] = page;
                        PageViewer.Content = pageCache[PageName];
                    }
                    else
                    {
                        Log.Information("Page is already cached. Loading it");
                        PageViewer.Content = pageCache[PageName];
                    }
                    break;
                case "Settings":
                    if (!pageCache.ContainsKey(PageName))
                    {
                        Log.Information("Page is not cached. Loading it and caching it for future use.");
                        ACR_Pages.Settings page = new ACR_Pages.Settings();
                        pageCache[PageName] = page;
                        PageViewer.Content = page;
                    }
                    else
                    {
                        Log.Information("Page is already cached. Loading it");
                        PageViewer.Content = pageCache[PageName];
                    }
                    break;
                default:
                    if (!pageCache.ContainsKey(PageName))
                    {
                        Log.Information("Page is not cached. Loading it and caching it for future use.");
                        ACR_Pages.Default_Page page = new ACR_Pages.Default_Page();
                        pageCache[PageName] = page;
                        PageViewer.Content = page;
                    }
                    else
                    {
                        Log.Information("Page is already cached. Loading it");
                        PageViewer.Content = pageCache[PageName];
                    }
                    break;
            }
        }

        /// <summary>
        /// Sets Process Affinity based on the amount of cores. Can help with stutters and tearing.
        /// <param name="gameProcessID">ID of the game process needed so we can change it's CPU affinity</param>
        /// </summary>
        private async Task SetProcessAffinity()
        {
            try
            {
                Log.Information("Grabbing game process by ID to change affinity.");
                Process[] processes = Process.GetProcessesByName("ACRSP");
                while (processes.Length <= 0)
                {
                    processes = Process.GetProcessesByName("ACRSP");
                    await Task.Delay(1000);
                }
                Log.Information($"Game process found.");
                int affinity;
                switch (true)
                {
                    case bool when App.NumberOfCores >= 8 && App.NumberOfThreads >= 16:
                        Log.Information("8 Cores/16 Threads or greater affinity"); ;
                        foreach (Process gameProcess in processes)
                        {
                            Log.Information($"Game Process: {gameProcess.ProcessName}, ID: {gameProcess.Id}");
                            gameProcess.ProcessorAffinity = new IntPtr(0xFFFF);
                        }
                        break;
                    case bool when App.NumberOfCores == 6 && App.NumberOfThreads == 12:
                        Log.Information("6 Cores/12 Threads affinity");
                        foreach (Process gameProcess in processes)
                        {
                            Log.Information($"Game Process: {gameProcess.ProcessName}, ID: {gameProcess.Id}");
                            gameProcess.ProcessorAffinity = new IntPtr(0x7F);
                        }
                        break;
                    case bool when App.NumberOfCores == 6 && App.NumberOfThreads == 6:
                        Log.Information("6 Cores/6 Threads affinity");
                        foreach (Process gameProcess in processes)
                        {
                            Log.Information($"Game Process: {gameProcess.ProcessName}, ID: {gameProcess.Id}");
                            gameProcess.ProcessorAffinity = new IntPtr(0x3F);
                        }
                        break;
                    case bool when App.NumberOfCores == 8 && App.NumberOfThreads == 8:
                    case bool when App.NumberOfCores == 4 && App.NumberOfThreads == 8:
                        Log.Information("4 Cores/8 Threads or 8 Cores/8 Threads affinity");
                        foreach (Process gameProcess in processes)
                        {
                            Log.Information($"Game Process: {gameProcess.ProcessName}, ID: {gameProcess.Id}");
                            gameProcess.ProcessorAffinity = new IntPtr(0xFF);
                        }
                        break;
                    case bool when App.NumberOfCores == 4 && App.NumberOfThreads == 4:
                        Log.Information("4 Cores/4 Threads affinity");
                        foreach (Process gameProcess in processes)
                        {
                            Log.Information($"Game Process: {gameProcess.ProcessName}, ID: {gameProcess.Id}");
                            gameProcess.ProcessorAffinity = new IntPtr(0x0F);
                        }
                        break;
                    default:
                        Log.Information("Default preset");
                        affinity = (1 << App.NumberOfThreads) - 1;
                        Log.Information($"Affinity Bitmask: 0x{affinity.ToString("X")}");
                        foreach (Process gameProcess in processes)
                        {
                            Log.Information($"Game Process: {gameProcess.ProcessName}, ID: {gameProcess.Id}");
                            gameProcess.ProcessorAffinity = new IntPtr(affinity);
                        }
                        break;
                }
                await Task.Delay(1);
            }
            catch (Exception ex)
            {
                Log.Information(ex, "Error:");
                return;
            }
        }

        /// <summary>
        /// This is used for Window Dragging. Needed when disabling Window stuff in XAML
        /// </summary>
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        /// <summary>
        /// Exits the Launcher back to the main launcher
        /// </summary>
        private async void Exit_Click(object sender, RoutedEventArgs e)
        {
            Log.Information("Closing Assassin's Creed Revelations Launcher");
            await Task.Delay(1);
            this.Close();
        }

        /// <summary>
        /// Starts the game
        /// </summary>
        private async void Play_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process[] Game = Process.GetProcessesByName("ACRSP");
                if (Game.Length <= 0)
                {
                    Process gameProcess = new Process();
                    gameProcess.StartInfo.WorkingDirectory = App.ACRPath;
                    gameProcess.StartInfo.FileName = "ACRSP.exe";
                    gameProcess.StartInfo.UseShellExecute = true;
                    gameProcess.Start();
                    Log.Information("Game is starting");
                    Log.Information("Setting game affinity based on CPU Core/Thread Count");
                    Game = Process.GetProcessesByName("ACRSP");
                    while (Game.Length <= 0)
                    {
                        await Task.Delay(1000);
                        Game = Process.GetProcessesByName("ACRSP");
                    }
                    foreach (Process process in Game)
                    {
                        process.PriorityClass = ProcessPriorityClass.AboveNormal;
                        await SetProcessAffinity();
                    }
                    Log.Information("Game started");
                    await Task.Delay(1);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error:");
                return;
            }
        }

        /// <summary>
        /// Navigates to the Credits WPF page in the Frame
        /// </summary>
        private void Credits_Click(object sender, RoutedEventArgs e)
        {
            NavigateToPage("Credits");
        }

        /// <summary>
        /// Navigates to the Settings WPF page in the Frame if there is AC2 configuration file
        /// </summary>
        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (System.IO.File.Exists(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"Assassin's Creed Revelations\ACRevelations.ini")))
                {
                    NavigateToPage("Settings");
                }
                else
                {
                    MessageBox.Show("Configuration file missing\nPlease launch the game once.");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error:");
                return;
            }
        }
    }
}
