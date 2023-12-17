using Serilog;
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
        private void Play_Click(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// Navigates to the Credits WPF page in the Frame
        /// </summary>
        private void Credits_Click(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// Navigates to the Settings WPF page in the Frame if there is AC2 configuration file
        /// </summary>
        private void Settings_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
