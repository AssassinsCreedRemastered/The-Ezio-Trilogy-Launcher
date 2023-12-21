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
                    await MissingGame("ACRSP");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error:");
                MessageBox.Show(ex.Message);
                return;
            }
        }
    }
}
