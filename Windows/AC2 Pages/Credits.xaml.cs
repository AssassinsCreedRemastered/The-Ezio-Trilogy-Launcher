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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace The_Ezio_Trilogy_Launcher.Windows.AC2_Pages
{
    /// <summary>
    /// Interaction logic for Credits.xaml
    /// </summary>
    public partial class Credits : Page
    {
        public Credits()
        {
            InitializeComponent();
        }

        private void OverhaulMod_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://www.moddb.com/mods/assassins-creed-ii-overhaul-mod",
                UseShellExecute = true,
            });
        }

        private void EaglePatch_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://github.com/Sergeanur/EaglePatch",
                UseShellExecute = true,
            });
        }

        private void PS3Buttons_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://www.nexusmods.com/assassinscreedii/mods/11",
                UseShellExecute = true,
            });
        }

        private void ReShadePreset_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://www.nexusmods.com/assassinscreedii/mods/6",
                UseShellExecute = true,
            });
        }

        private void Background_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://wall.alphacoders.com/big.php?i=516027",
                UseShellExecute = true,
            });
        }

        private void ContentUnlock_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://steamcommunity.com/sharedfiles/filedetails/?id=2841221628",
                UseShellExecute = true,
            });
        }
    }
}
