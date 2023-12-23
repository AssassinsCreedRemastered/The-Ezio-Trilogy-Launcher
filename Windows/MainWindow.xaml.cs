using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
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
using Microsoft.Win32;
using System.Reflection;
using System.Net;
using System.Diagnostics;

namespace The_Ezio_Trilogy_Launcher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // Functions
        /// <summary>
        /// Opens FileDialog and replaces the old path to the game
        /// <param name="game">Game whose path needs to be updated</param>
        /// </summary>
        private async Task MissingGame(string game)
        {
            try
            {
                OpenFileDialog FileDialog = new OpenFileDialog();
                switch (game)
                {
                    case "ACBSP": // ACB
                        FileDialog.Filter = "Executable Files|ACBSP.exe";
                        FileDialog.Title = "Select an Assassins Creed Executable";
                        if (FileDialog.ShowDialog() == true)
                        {
                            App.ACBPath = System.IO.Path.GetDirectoryName(FileDialog.FileName);
                            using (StreamWriter sw = new StreamWriter(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments) + @"\Assassin's Creed - The Ezio Trilogy Remastered\ACBPath.txt"))
                            {
                                sw.WriteLine(App.ACBPath);
                            };
                        }
                        break;
                    case "ACRSP": // ACR
                        FileDialog.Filter = "Executable Files|ACRSP.exe";
                        FileDialog.Title = "Select an Assassins Creed Executable";
                        if (FileDialog.ShowDialog() == true)
                        {
                            App.ACRPath = System.IO.Path.GetDirectoryName(FileDialog.FileName);
                            using (StreamWriter sw = new StreamWriter(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments) + @"\Assassin's Creed - The Ezio Trilogy Remastered\ACRPath.txt"))
                            {
                                sw.WriteLine(App.ACRPath);
                            };
                        }
                        break;
                    default: // AC2
                        FileDialog.Filter = "Executable Files|AssassinsCreedIIGame.exe";
                        FileDialog.Title = "Select an Assassins Creed Executable";
                        if (FileDialog.ShowDialog() == true)
                        {
                            App.AC2Path = System.IO.Path.GetDirectoryName(FileDialog.FileName);
                            using (StreamWriter sw = new StreamWriter(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments) + @"\Assassin's Creed - The Ezio Trilogy Remastered\AC2Path.txt"))
                            {
                                sw.WriteLine(App.AC2Path);
                            };
                        }
                        break;
                }
                await Task.Delay(1);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "");
                return;
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
                    if (this.Visibility == Visibility.Visible)
                    {
                        System.Windows.MessageBox.Show("New version of the launcher found. Click on the Update button to update the launcher.");
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "");
                if (this.Visibility == Visibility.Visible)
                {
                    System.Windows.MessageBox.Show($"Error: {ex.Message}{Environment.NewLine}Possibly no internet connection");
                }
            }
        }

        // Events/Buttons
        /// <summary>
        /// This is executed when the Credits button is clicked.
        /// Opens "Credits" Window
        /// </summary>
        private void Credits_Click(object sender, RoutedEventArgs e)
        {
            Log.Information("Opening Main Credits tab");
            try
            {
                MainCredits mainCredits = new MainCredits();
                mainCredits.ShowDialog();
            }
            catch (Exception ex)
            {
                Log.Information(ex, "");
                MessageBox.Show(ex.Message);
                return;
            }
        }

        /// <summary>
        /// Checks for updates and asks user to update if there are any new updates
        /// </summary>
        private async void Update_Click(object sender, RoutedEventArgs e)
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
                    Log.Information("Newest version is already installed");
                    MessageBox.Show("Newest version is already installed.");
                    GC.Collect();
                    await Task.Delay(1);
                    return;
                }
                else
                {
                    Log.Information("New version found.");
                    MessageBoxResult result = MessageBox.Show("New version of the launcher found. Do you want to update?", "Confirmation", MessageBoxButton.YesNo);
                    if (result == MessageBoxResult.Yes)
                    {
                        Process updater = new Process();
                        updater.StartInfo.FileName = "The Ezio Trilogy Launcher Updater.exe";
                        updater.StartInfo.WorkingDirectory = Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments) + @"\Assassin's Creed - The Ezio Trilogy Remastered\";
                        updater.StartInfo.UseShellExecute = true;
                        Log.Information("Starting the Launcher Updater");
                        updater.Start();
                        Log.Information("Closing the Launcher to perform the update");
                        Environment.Exit(0);
                    }
                    else
                    {
                        GC.Collect();
                        await Task.Delay(1);
                        return;
                    }
                }
                GC.Collect();
                await Task.Delay(1);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "");
                MessageBox.Show(ex.Message);
                return;
            }
        }

        /// <summary>
        /// Executed when Exit button is clicked
        /// Closes the launcher
        /// </summary>
        private async void Exit_Click(object sender, RoutedEventArgs e)
        {
            Log.Information("Exiting");
            await Task.Delay(1);
            Environment.Exit(0);
        }

        /// <summary>
        /// Opens Assassin's Creed 2 launcher and hides the main launcher
        /// </summary>
        private async void OpenACII_Click(object sender, RoutedEventArgs e)
        {
            Log.Information("Opening Assassin's Creed 2 Launcher");
            try
            {
                // Checks if AC2Path exists
                if (App.AC2Path != null)
                {
                    // Checks if there's something in AC2Path
                    if (System.IO.File.Exists(App.AC2Path + @"\AssassinsCreedIIGame.exe"))
                    {
                        AssassinsCreed2 ac2 = new AssassinsCreed2();
                        this.Visibility = Visibility.Hidden;
                        ac2.ShowDialog();
                        this.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        System.Windows.MessageBox.Show("Assassin's Creed 2 not found.\nA window will now open asking you to reselect ACBSP.exe");
                        await MissingGame("AssassinsCreedIIGame");
                    }
                } else
                {
                    MessageBox.Show("Assassin's Creed 2 Remaster is not installed.");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error:");
                MessageBox.Show(ex.Message);
                return;
            }
        }

        /// <summary>
        /// Opens Assassin's Creed: Brotherhood launcher and hides the main launcher
        /// </summary>
        private async void OpenACB_Click(object sender, RoutedEventArgs e)
        {
            Log.Information("Opening Assassin's Creed Brotherhood Launcher");
            try
            {
                // Checks if ACBPath exists
                if (App.ACBPath != null)
                {
                    // Checks if there's something in ACBPath
                    if (System.IO.File.Exists(App.ACBPath + @"\ACBSP.exe"))
                    {
                        AssassinsCreedBrotherhood acb = new AssassinsCreedBrotherhood();
                        this.Visibility = Visibility.Hidden;
                        acb.ShowDialog();
                        this.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        System.Windows.MessageBox.Show("Assassin's Creed Brotherhood not found.\nA window will now open asking you to reselect ACBSP.exe");
                        await MissingGame("ACBSP");
                    }
                }
                else
                {
                    MessageBox.Show("Assassin's Creed Brotherhood Remaster is not installed.");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error:");
                MessageBox.Show(ex.Message);
                return;
            }
        }

        /// <summary>
        /// Opens Assassin's Creed: Revelations launcher and hides the main launcher
        /// </summary>
        private async void OpenACR_Click(object sender, RoutedEventArgs e)
        {
            Log.Information("Opening Assassin's Creed Revelations Launcher");
            try
            {
                if (App.ACRPath != null)
                {
                    if (System.IO.File.Exists(App.ACRPath + @"\ACRSP.exe"))
                    {
                        AssassinsCreedRevelations acr = new AssassinsCreedRevelations();
                        this.Visibility = Visibility.Hidden;
                        acr.ShowDialog();
                        this.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        System.Windows.MessageBox.Show("Assassin's Creed Revelations not found.\nA window will now open asking you to reselect ACBSP.exe");
                        await MissingGame("ACRSP");
                    }
                }
                else
                {
                    MessageBox.Show("Assassin's Creed Revelations Remaster is not installed.");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error:");
                MessageBox.Show(ex.Message);
                return;
            }
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                await Task.Delay(1000);
                await CheckForUpdates();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "");
                System.Windows.MessageBox.Show(ex.Message);
            }
        }
    }
}
