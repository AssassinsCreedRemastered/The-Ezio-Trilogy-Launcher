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

		public AssassinsCreed2()
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
            GC.Collect();
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
                Process[] GameRunning = Process.GetProcessesByName("AssassinsCreedIIGame");
                if (GameRunning.Length <= 0)
                {
                    // Create a new Game Process
                    Process gameProcess = new Process();
                    gameProcess.StartInfo.WorkingDirectory = App.AC2Path;
                    gameProcess.StartInfo.FileName = "AssassinsCreedIIGame.exe";
                    gameProcess.StartInfo.UseShellExecute = true;
                    if (App.AC2uModStatus)
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
                            process.PriorityClass = ProcessPriorityClass.AboveNormal;
                            if (System.Windows.Application.Current is App app)
                            {
                                await App.ProcessAffinityManager.SetProcessAffinity(process.ProcessName);
                            }
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
                else
                {
                    Log.Information("Game is already running");
                    System.Windows.MessageBox.Show("Game is already running.");
                }
                */
                if (System.Windows.Application.Current is App app)
                {
                    await app.StartGame("AssassinsCreedIIGame", App.AC2Path, App.AC2uModStatus);
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