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

namespace The_Ezio_Trilogy_Launcher.Windows.AC2_Pages
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
        private async Task FillComboBoxes()
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
                await Task.Delay(10);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "");
                MessageBox.Show(ex.Message);
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
                MSAASelector.SelectedIndex = MSAASelector.Items.Count - 1;
                Log.Information("Loading all of the MSAA options into MSAA ComboBox done.");
                Log.Information("Loading all of the Enviroment Quality options into Enviroment Quality Selector.");
                EnviromentQualitySelector.Items.Add("Low");
                EnviromentQualitySelector.Items.Add("Medium");
                EnviromentQualitySelector.Items.Add("High");
                EnviromentQualitySelector.Items.Add("Ultra");
                EnviromentQualitySelector.SelectedIndex = EnviromentQualitySelector.Items.Count - 1;
                Log.Information("Loading all of the Enviroment Quality options into Enviroment Quality Selector done.");
                Log.Information("Loading all of the Texture Quality options into Texture Quality Selector.");
                TextureQualitySelector.Items.Add("Low");
                TextureQualitySelector.Items.Add("Medium");
                TextureQualitySelector.Items.Add("High");
                TextureQualitySelector.SelectedIndex = TextureQualitySelector.Items.Count - 1;
                Log.Information("Loading all of the Texture Quality options into Texture Quality Selector done.");
                Log.Information("Loading all of the Shadow Quality options into Shadow Quality Selector.");
                ShadowQualitySelector.Items.Add("Low");
                ShadowQualitySelector.Items.Add("Medium");
                ShadowQualitySelector.Items.Add("High");
                ShadowQualitySelector.SelectedIndex = ShadowQualitySelector.Items.Count - 1;
                Log.Information("Loading all of the Shadow Quality options into Shadow Quality Selector done.");
                Log.Information("Loading all of the Reflection Quality options into Reflection Quality Selector.");
                ReflectionQualitySelector.Items.Add("Off");
                ReflectionQualitySelector.Items.Add("Low");
                ReflectionQualitySelector.Items.Add("High");
                ReflectionQualitySelector.SelectedIndex = ReflectionQualitySelector.Items.Count - 1;
                Log.Information("Loading all of the Reflection Quality options into Reflection Quality Selector done.");
                Log.Information("Loading all of the Character Quality options into Character Quality Selector.");
                CharacterQualitySelector.Items.Add("Low");
                CharacterQualitySelector.Items.Add("Medium");
                CharacterQualitySelector.Items.Add("High");
                CharacterQualitySelector.SelectedIndex = CharacterQualitySelector.Items.Count - 1;
                Log.Information("Loading all of the Character Quality options into Character Quality Selector done.");
                await Task.Delay(10);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "");
            }
        }
    }
}
