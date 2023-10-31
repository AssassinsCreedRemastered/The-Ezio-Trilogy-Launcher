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

namespace The_Ezio_Trilogy_Launcher.Windows.ACB_Pages
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Page
    {
        public Settings()
        {
            InitializeComponent();
            FillComboBoxes();
        }

        // Fills all of the ComboBoxes
        private async void FillComboBoxes()
        {
            try
            {
                Log.Information("Loading all of the supported resolutions into Resolutions ComboBox.");
                foreach (Resolution resolution in App.compatibleResolutions)
                {
                    ResolutionSelector.Items.Add(resolution.Res);
                }
                ResolutionSelector.SelectedIndex = ResolutionSelector.Items.Count - 1;
                Log.Information("Loading all of the supported resolutions into Resolutions ComboBox done.");
                Log.Information("Loading all of the supported refresh rates into Refresh Rate ComboBox.");
                foreach (int refreshRate in App.compatibleRefreshRates)
                {
                    RefreshRateSelector.Items.Add(refreshRate);
                }
                RefreshRateSelector.SelectedIndex = RefreshRateSelector.Items.Count - 1;
                Log.Information("Loading all of the supported refresh rates into Refresh Rate ComboBox done.");
                await FillComboBoxesWithFixedItems();
                GC.Collect();
                await Task.Delay(10);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "");
                System.Windows.MessageBox.Show(ex.Message);
            }
        }

        private async Task FillComboBoxesWithFixedItems()
        {
            try
            {
                Log.Information("Loading all of the MSAA options into MSAA ComboBox.");
                MSAASelector.Items.Add("Off");
                MSAASelector.Items.Add("2x");
                MSAASelector.Items.Add("4x");
                MSAASelector.Items.Add("8x");
                Log.Information("Loading all of the MSAA options into MSAA ComboBox done.");
                Log.Information("Loading all of the Enviroment Quality options into Enviroment Quality Selector.");
                EnviromentQualitySelector.Items.Add("Very Low");
                EnviromentQualitySelector.Items.Add("Low");
                EnviromentQualitySelector.Items.Add("Medium");
                EnviromentQualitySelector.Items.Add("High");
                EnviromentQualitySelector.Items.Add("Very High");
                Log.Information("Loading all of the Enviroment Quality options into Enviroment Quality Selector done.");
                Log.Information("Loading all of the Texture Quality options into Texture Quality Selector.");
                TextureQualitySelector.Items.Add("Low");
                TextureQualitySelector.Items.Add("Medium");
                TextureQualitySelector.Items.Add("High");
                Log.Information("Loading all of the Texture Quality options into Texture Quality Selector done.");
                Log.Information("Loading all of the Shadow Quality options into Shadow Quality Selector.");
                ShadowQualitySelector.Items.Add("Low");
                ShadowQualitySelector.Items.Add("Medium");
                ShadowQualitySelector.Items.Add("High");
                ShadowQualitySelector.Items.Add("Very High");
                Log.Information("Loading all of the Shadow Quality options into Shadow Quality Selector done.");
                Log.Information("Loading all of the Reflection Quality options into Reflection Quality Selector.");
                ReflectionQualitySelector.Items.Add("Off");
                ReflectionQualitySelector.Items.Add("Low");
                ReflectionQualitySelector.Items.Add("Medium");
                ReflectionQualitySelector.Items.Add("High");
                Log.Information("Loading all of the Reflection Quality options into Reflection Quality Selector done.");
                Log.Information("Loading all of the Character Quality options into Character Quality Selector.");
                CharacterQualitySelector.Items.Add("Very Low");
                CharacterQualitySelector.Items.Add("Low");
                CharacterQualitySelector.Items.Add("Medium");
                CharacterQualitySelector.Items.Add("High");
                CharacterQualitySelector.Items.Add("Very High");
                Log.Information("Loading all of the Character Quality options into Character Quality Selector done.");
                GC.Collect();
                await Task.Delay(10);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "");
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {

        }

        private void UnlockBonusContent_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
