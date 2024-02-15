using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
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
    /// Interaction logic for AssassinsCreedBrotherhood.xaml
    /// </summary>
    public partial class AssassinsCreedBrotherhood : Window
    {
        /// <summary>
        /// Holds all of the pages cached
        /// </summary>
        private Dictionary<string, Page> pageCache = new Dictionary<string, Page>();

        public AssassinsCreedBrotherhood()
        {
            InitializeComponent();
        }

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
                        ACB_Pages.Credits page = new ACB_Pages.Credits();
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
                        ACB_Pages.Settings page = new ACB_Pages.Settings();
                        pageCache[PageName] = page;
                        PageViewer.Content = page;
                    }
                    else
                    {
                        Log.Information("Page is already cached. Loading it");
                        PageViewer.Content = pageCache[PageName];
                    }
                    break;
                case "Mods":
                    if (!pageCache.ContainsKey(PageName))
                    {
                        Log.Information("Page is not cached. Loading it and caching it for future use.");
                        ACB_Pages.Mods page = new ACB_Pages.Mods();
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
                        ACB_Pages.Default_Page page = new ACB_Pages.Default_Page();
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
            Log.Information("Closing Assassin's Creed Brotherhood Launcher");
            GC.Collect();
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
                /*
                Process[] Game = Process.GetProcessesByName("ACBSP");
                if (Game.Length <= 0)
                {
                    Process gameProcess = new Process();
                    gameProcess.StartInfo.WorkingDirectory = App.ACBPath;
                    gameProcess.StartInfo.FileName = "ACBSP.exe";
                    gameProcess.StartInfo.UseShellExecute = true;
                    if (App.ACBuModStatus)
                    {
                        Process uModProcess = new Process();
                        uModProcess.StartInfo.WorkingDirectory = App.ACBPath + @"\uMod";
                        uModProcess.StartInfo.FileName = "uMod.exe";
                        uModProcess.StartInfo.UseShellExecute = true;
                        uModProcess.Start();
                        gameProcess.Start();
                        Log.Information("Game is starting");
                        await Task.Delay(5000);
                        Log.Information("Setting game affinity based on CPU Core/Thread Count");
                        Game = Process.GetProcessesByName("ACBSP");
                        while (Game.Length <= 0)
                        {
                            await Task.Delay(1000);
                            Game = Process.GetProcessesByName("ACBSP");
                        }
                        foreach (Process process in Game)
                        {
                            process.PriorityClass = ProcessPriorityClass.AboveNormal;
                            if (System.Windows.Application.Current is App app)
                            {
                                await App.ProcessAffinityManager.SetProcessAffinity(process.ProcessName);
                            }
                        }
                        Log.Information("Game started");
                        Log.Information("Waiting for game to be closed.");
                        while (Game.Length > 0)
                        {
                            await Task.Delay(1000);
                            Game = Process.GetProcessesByName("ACBSP");
                        }
                        Log.Information("Game Closed");
                        try
                        {
                            if (Process.GetProcessById(uModProcess.Id) != null)
                            {
                                uModProcess.CloseMainWindow();
                            }
                        }
                        catch (Exception)
                        {
                            Log.Information("uMod is already closed.");
                        }
                        await Task.Delay(1);
                    }
                    else
                    {
                        gameProcess.Start();
                        Log.Information("Game is starting");
                        Log.Information("Setting game affinity based on CPU Core/Thread Count");
                        Game = Process.GetProcessesByName("ACBSP");
                        while (Game.Length <= 0)
                        {
                            await Task.Delay(1000);
                            Game = Process.GetProcessesByName("ACBSP");
                        }
                        foreach (Process process in Game)
                        {
                            process.PriorityClass = ProcessPriorityClass.AboveNormal;
                            if (System.Windows.Application.Current is App app)
                            {
                                await App.ProcessAffinityManager.SetProcessAffinity(process.ProcessName);
                            }
                        }
                        Log.Information("Game started");
                        await Task.Delay(1);
                    }
                }
                */
                if (System.Windows.Application.Current is App app)
                {
                    App.discordRPCManager.UpdateStateAndIcon("acb1", "Assassin's Creed: Brotherhood", "Idle");
                    App.discordRPCManager.InitializeInGamePresence();
                    await app.StartGame("ACBSP", App.ACBPath, App.ACBuModStatus);
                    App.discordRPCManager.UpdateStateAndIcon("acb1", "Assassin's Creed: Brotherhood", "Idle");
                }
                GC.Collect();
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
            App.discordRPCManager.UpdateStateAndIcon("acb1", "Assassin's Creed: Brotherhood", "Credits");
            NavigateToPage("Credits");
        }

        /// <summary>
        /// Navigates to the Settings WPF page in the Frame if there is AC2 configuration file
        /// </summary>
        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //System.IO.File.Exists(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)), @"Saved Games\Assassin's Creed Brotherhood\ACBrotherhood.ini"))
                if (System.IO.File.Exists(System.IO.Path.Combine(App.SavedGamesFolderPath, @"Assassin's Creed Brotherhood\ACBrotherhood.ini")))
                {
                    App.discordRPCManager.UpdateStateAndIcon("acb1", "Assassin's Creed: Brotherhood", "Settings");
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

        /// <summary>
        /// Navigates to the uMod WPF Page in the Frame if uMod is enabled
        /// </summary>
        private void uMod_Click(object sender, RoutedEventArgs e)
        {
            if (App.ACBuModStatus)
            {
                App.discordRPCManager.UpdateStateAndIcon("acb1", "Assassin's Creed: Brotherhood", "uMod Mods");
                NavigateToPage("Mods");
            }
            else
            {
                MessageBox.Show("uMod is disabled. Please Enable it.");
            }
        }
    }
}
