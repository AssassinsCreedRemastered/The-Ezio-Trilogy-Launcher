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
using System.Windows.Navigation;
using System.Windows.Shapes;

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

        private async void Exit_Click(object sender, RoutedEventArgs e)
        {
            Log.Information("Exiting");
            await Task.Delay(10);
            Environment.Exit(0);
        }

        private void OpenACII_Click(object sender, RoutedEventArgs e)
        {
            Log.Information("Opening Assassin's Creed 2 Launcher");
        }

        private void OpenACB_Click(object sender, RoutedEventArgs e)
        {
            Log.Information("Opening Assassin's Creed Brotherhood Launcher");
        }

        private void OpenACR_Click(object sender, RoutedEventArgs e)
        {
            Log.Information("Opening Assassin's Creed Revelations Launcher");
        }
    }
}
