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

namespace The_Ezio_Trilogy_Launcher.Windows
{
    /// <summary>
    /// Interaction logic for AssassinsCreed2.xaml
    /// </summary>
    public partial class AssassinsCreed2 : Window
    {
        [DllImport("kernel32.dll")]
        public static extern IntPtr GetCurrentProcess();

        [DllImport("kernel32.dll")]
        public static extern IntPtr SetProcessAffinityMask(IntPtr hProcess, IntPtr dwProcessAffinityMask);


        public AssassinsCreed2()
        {
            InitializeComponent();
            Log.Information(MainWindow.NumberOfCores.ToString());
            Log.Information(MainWindow.NumberOfThreads.ToString());
        }

        // Sets Process Affinity based on the amount of cores
        // According to PCGamingWiki it can help with Stutters and Tearing
        private async Task SetProcessAffinity(int gameProcessID)
        {
            try
            {
                Log.Information("Grabbing game process by ID to change affinity.");
                Process processAfinity = Process.GetProcessById(gameProcessID);
                Log.Information($"Process found: {processAfinity}");
                int affinity;
                switch (true)
                {
                    case bool when MainWindow.NumberOfCores >= 8 && MainWindow.NumberOfThreads >= 16:
                        Log.Information("8 Cores/16 Threads or greater affinity");
                        processAfinity.ProcessorAffinity = new IntPtr(0xFFFF);
                        break;
                    case bool when MainWindow.NumberOfCores == 6 && MainWindow.NumberOfThreads == 12:
                        Log.Information("6 Cores/12 Threads affinity");
                        processAfinity.ProcessorAffinity = new IntPtr(0x7F);
                        break;
                    case bool when MainWindow.NumberOfCores == 6 && MainWindow.NumberOfThreads == 6:
                        Log.Information("6 Cores/6 Threads affinity");
                        processAfinity.ProcessorAffinity = new IntPtr(0x3F);
                        break;
                    case bool when MainWindow.NumberOfCores == 8 && MainWindow.NumberOfThreads == 8:
                    case bool when MainWindow.NumberOfCores == 4 && MainWindow.NumberOfThreads == 8:
                        Log.Information("4 Cores/8 Threads or 8 Cores/8 Threads affinity");
                        processAfinity.ProcessorAffinity = new IntPtr(0xFF);
                        break;
                    case bool when MainWindow.NumberOfCores == 4 && MainWindow.NumberOfThreads == 4:
                        Log.Information("4 Cores/4 Threads affinity");
                        processAfinity.ProcessorAffinity = new IntPtr(0x0F);
                        break;
                    default:
                        Log.Information("Default preset");
                        affinity = (1 << MainWindow.NumberOfThreads) - 1;
                        Log.Information($"Affinity Bitmask: 0x{affinity.ToString("X")}");
                        processAfinity.ProcessorAffinity = new IntPtr(affinity);
                        break;
                }
                await Task.Delay(10);
            }
            catch (Exception ex)
            {
                Log.Information(ex, "Error:");
                return;
            }
        }

        // Window Dragging
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        // Exits the Launcher back to the main window
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Log.Information("Closing Assassin's Creed 2 Launcher");
            this.Close();
        }

        // Starts the game
        private async void Play_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //D:\Games\Assassin's Creed II\AssassinsCreedIIGame.exe
                // Create a new process start info
                ProcessStartInfo GameInfo = new ProcessStartInfo
                {
                    FileName = @"D:\Games\Assassin's Creed II\AssassinsCreedIIGame.exe",
                    UseShellExecute = false, // Required for setting affinity
                    RedirectStandardOutput = false, // Set to true if you want to capture the output
                    CreateNoWindow = true // Set to true to hide the console window
                };
                Process gameProcess = new Process();
                gameProcess.StartInfo = GameInfo;
                gameProcess.Start();
                Log.Information("Game is starting");
                Log.Information("Setting game affinity based on CPU Core/Thread Count");
                await SetProcessAffinity(gameProcess.Id);
                Log.Information("Game started");
                gameProcess.WaitForExit();
                Log.Information("Game closed");
                await Task.Delay(10);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error:");
                return;
            }
        }
    }
}