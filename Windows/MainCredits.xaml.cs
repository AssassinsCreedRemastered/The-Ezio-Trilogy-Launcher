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
    /// Interaction logic for MainCredits.xaml
    /// </summary>
    public partial class MainCredits : Window
    {
        public MainCredits()
        {
            InitializeComponent();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Font_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://www.dafont.com/assassin.font",
                UseShellExecute = true,
            });
        }

        private void Icon_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://www.deviantart.com/abdelrahman18/art/Assassin-s-Creed-The-Ezio-Collection-Icon-882196951",
                UseShellExecute = true,
            });
        }

        private void Background_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://www.reddit.com/user/KokeNunez",
                UseShellExecute = true,
            });
        }

        private void uMod_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://code.google.com/archive/p/texmod/",
                UseShellExecute = true,
            });
        }

        private void ReShade_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://reshade.me/",
                UseShellExecute = true,
            });
        }

        private void mcflypg_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://www.patreon.com/mcflypg",
                UseShellExecute = true,
            });
        }
    }
}
