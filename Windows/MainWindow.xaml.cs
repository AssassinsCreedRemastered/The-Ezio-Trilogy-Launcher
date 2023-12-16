using Serilog;
using System;
using System.Collections.Generic;
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
        private void OpenACII_Click(object sender, RoutedEventArgs e)
        {
            Log.Information("Opening Assassin's Creed 2 Launcher");
            try
            {
                if (App.AC2Path != null)
                {
                    AssassinsCreed2 ac2 = new AssassinsCreed2();
                    this.Visibility = Visibility.Hidden;
                    ac2.ShowDialog();
                    this.Visibility = Visibility.Visible;
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
        private void OpenACB_Click(object sender, RoutedEventArgs e)
        {
            Log.Information("Opening Assassin's Creed Brotherhood Launcher");
            try
            {
                if (App.ACBPath != null)
                {
                    AssassinsCreedBrotherhood acb = new AssassinsCreedBrotherhood();
                    this.Visibility = Visibility.Hidden;
                    acb.ShowDialog();
                    this.Visibility = Visibility.Visible;
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
        private void OpenACR_Click(object sender, RoutedEventArgs e)
        {
            Log.Information("Opening Assassin's Creed Revelations Launcher");
            try
            {
                if (App.ACRPath != null)
                {

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
    }
}
