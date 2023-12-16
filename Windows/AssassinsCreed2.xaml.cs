using System;
using System.Collections.Generic;
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
using System.Management;

// Imported
using Serilog;
using System.IO;
using System.Security.Cryptography;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Markup;
using System.Windows.Navigation;
using The_Ezio_Trilogy_Launcher.Windows.AC2_Pages;

namespace The_Ezio_Trilogy_Launcher.Windows
{
	/// <summary>
	/// Interaction logic for AssassinsCreed2.xaml
	/// </summary>

	public partial class AssassinsCreed2 : Window
	{
        /// <summary>
        /// Holds all of the pages cached
        /// </summary>
        private Dictionary<string, Page> pageCache = new Dictionary<string, Page>();

        /// <summary>
        /// Used to check if uMod is enabled or not
        /// </summary>
        public static bool uModStatus { get; set; }

		public AssassinsCreed2()
		{
			InitializeComponent();
            ReaduModStatus();
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
                        AC2_Pages.Credits page = new AC2_Pages.Credits();
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
                        AC2_Pages.Settings page = new AC2_Pages.Settings();
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
                        AC2_Pages.Mods page = new AC2_Pages.Mods();
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
                        AC2_Pages.Default_Page page = new AC2_Pages.Default_Page();
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
				Process[] processes = Process.GetProcessesByName("AssassinsCreedIIGame");
				while (processes.Length <= 0)
				{
                    processes = Process.GetProcessesByName("AssassinsCreedIIGame");
					await Task.Delay(1000);
				}
				Log.Information($"Game process found.");
				int affinity;
				switch (true)
				{
					case bool when App.NumberOfCores >= 8 && App.NumberOfThreads >= 16:
						Log.Information("8 Cores/16 Threads or greater affinity");;
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
        /// Checks if uMod is enabled or disabled.
        /// </summary>
        private void ReaduModStatus()
        {
            try
            {
                Log.Information("Checking if uMod is enabled");
                if (System.IO.File.Exists(App.AC2Path + @"\uMod\Status.txt"))
                {
                    string[] statusFile = File.ReadAllLines(App.AC2Path + @"\uMod\Status.txt");
                    foreach (string status in statusFile)
                    {
                        if (status.StartsWith("Enabled"))
                        {
                            string[] splitLine = status.Split('=');
                            if (int.Parse(splitLine[1]) == 1)
                            {
                                Log.Information("uMod is enabled");
                                uModStatus = true;
                            }
                            else
                            {
                                Log.Information("uMod is disabled");
                                uModStatus = false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Information(ex, "");
                System.Windows.MessageBox.Show(ex.Message);
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
        private void Exit_Click(object sender, RoutedEventArgs e)
		{
			Log.Information("Closing Assassin's Creed 2 Launcher");
			this.Close();
		}

        /// <summary>
        /// Starts the game
        /// </summary>
        private async void Play_Click(object sender, RoutedEventArgs e)
		{
			try
			{
                Process[] GameRunning = Process.GetProcessesByName("AssassinsCreedIIGame");
                if (GameRunning.Length <= 0)
                {
                    // Create a new Game Process
                    Process gameProcess = new Process();
                    gameProcess.StartInfo.WorkingDirectory = App.AC2Path;
                    gameProcess.StartInfo.FileName = "AssassinsCreedIIGame.exe";
                    gameProcess.StartInfo.UseShellExecute = true;
                    if (uModStatus)
                    {
                        Process uModProcess = new Process();
                        uModProcess.StartInfo.WorkingDirectory = App.AC2Path + @"\uMod";
                        uModProcess.StartInfo.FileName = "uMod.exe";
                        uModProcess.StartInfo.UseShellExecute = true;
                        uModProcess.Start();
                        gameProcess.Start();
                        Log.Information("Game is starting");
                        Log.Information("Setting game affinity based on CPU Core/Thread Count");
                        Process[] Game = Process.GetProcessesByName("AssassinsCreedIIGame");
                        while (Game.Length <= 0)
                        {
                            await Task.Delay(1000);
                            Game = Process.GetProcessesByName("AssassinsCreedIIGame");
                        }
                        await Task.Delay(1000);
                        foreach (Process process in Game)
                        {
                            process.PriorityClass = ProcessPriorityClass.High;
                            await SetProcessAffinity();
                        }
                        Log.Information("Game started");
                        Log.Information($"Waiting for game to be closed.");
                        while (Game.Length > 0)
                        {
                            await Task.Delay(1000);
                            Game = Process.GetProcessesByName("AssassinsCreedIIGame");
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
                        Process[] Game = Process.GetProcessesByName("AssassinsCreedIIGame");
                        while (Game.Length <= 0)
                        {
                            await Task.Delay(1000);
                            Game = Process.GetProcessesByName("AssassinsCreedIIGame");
                        }
                        foreach (Process process in Game)
                        {
                            process.PriorityClass = ProcessPriorityClass.High;
                            await SetProcessAffinity();
                        }
                        Log.Information("Game started");
                        await Task.Delay(1);
                    }
                }
                else
                {
                    Log.Information("Game is already running");
                    System.Windows.MessageBox.Show("Game is already running.");
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
			if (System.IO.File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Ubisoft\Assassin's Creed 2\Assassin2.ini"))
			{
				Log.Information("Game configuration file found");
                NavigateToPage("Settings");
            }
			else
			{
                Log.Information("Game configuration not found");
				System.Windows.MessageBox.Show("Game configuration not found. Please open the game once and change some setting in game to generate that file fully to be able to change settings here.");
				return;
            }
        }

        /// <summary>
        /// Navigates to the uMod WPF Page in the Frame
        /// </summary>
        private void uMod_Click(object sender, RoutedEventArgs e)
        {
			NavigateToPage("Mods");
        }
    }
}