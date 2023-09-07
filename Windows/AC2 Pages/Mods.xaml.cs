using Serilog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for Mods.xaml
    /// </summary>
    public partial class Mods : Page
    {
        private ObservableCollection<string> elements = new ObservableCollection<string>
        {
            "Mod 1",
            "Mod 2",
            "Mod 3"
        };

        private int modNumber = 4;

        public Mods()
        {
            InitializeComponent();
            EnabledModsList.ItemsSource = elements;
        }
        private static T FindAncestor<T>(DependencyObject current) where T : DependencyObject
        {
            do
            {
                if (current is T ancestor)
                {
                    return ancestor;
                }
                current = VisualTreeHelper.GetParent(current);
            }
            while (current != null);
            return null;
        }

        private void SaveSettings_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void AddMod_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                elements.Add($"Mod {modNumber}");
                modNumber++;
                await Task.Delay(1);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                MessageBox.Show(ex.Message);
                return;
            }
        }

        private async void RemoveSelectedMod_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (EnabledModsList.SelectedItem != null)
                {
                   elements.Remove(EnabledModsList.SelectedItem.ToString());
                }
                await Task.Delay(1);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "");
                MessageBox.Show(ex.Message);
                return;
            }
        }

        private void MoveSelectedModUp_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (EnabledModsList.SelectedItem != null)
                {
                    int selectedIndex = EnabledModsList.SelectedIndex;
                    if (EnabledModsList.SelectedIndex > 0)
                    {
                        string selectedItem = elements[selectedIndex];
                        elements.RemoveAt(selectedIndex);
                        elements.Insert(selectedIndex - 1, selectedItem);
                        EnabledModsList.SelectedIndex = selectedIndex - 1;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "");
                MessageBox.Show(ex.Message);
                return;
            }
        }

        private void MoveSelectedModDown_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (EnabledModsList.SelectedItem != null)
                {
                    int selectedIndex = EnabledModsList.SelectedIndex;
                    if (EnabledModsList.SelectedIndex >= 0 && EnabledModsList.SelectedIndex < (elements.Count - 1))
                    {
                        string selectedItem = elements[selectedIndex];
                        elements.RemoveAt(selectedIndex);
                        elements.Insert(selectedIndex + 1, selectedItem.ToString());
                        EnabledModsList.SelectedIndex = selectedIndex + 1;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "");
                MessageBox.Show(ex.Message);
                return;
            }
        }
    }
}
