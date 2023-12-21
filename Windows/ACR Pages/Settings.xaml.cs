using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace The_Ezio_Trilogy_Launcher.Windows.ACR_Pages
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
            ReadConfigurationFiles();
        }

        // Functions
        /// <summary>
        /// Fills all of the ComboBoxes
        /// </summary>
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
                await Task.Delay(1);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "");
                System.Windows.MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Fills all of the ComboBoxes that have fixed items
        /// </summary>
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
                EnviromentQualitySelector.Items.Add("Ultra");
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
                await Task.Delay(1);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "");
            }
        }

        /// <summary>
        /// Reads all of the configuration files
        /// </summary>
        private async void ReadConfigurationFiles()
        {
            Log.Information("Reading configuration files");
            await ReadGameConfig();
            await CheckTools();
            await CheckStartupVideos();
        }

        /// <summary>
        /// Reads game configuration file
        /// </summary>
        private async Task ReadGameConfig()
        {
            try
            {
                Log.Information("Reading game configuration file");
                //System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"Assassin's Creed Revelations\ACRevelations.ini")
                if (System.IO.File.Exists(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"Assassin's Creed Revelations\ACRevelations.ini")))
                {
                    Log.Information(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"Assassin's Creed Revelations\ACRevelations.ini"));
                    string currentResolution = "";
                    string[] gameConfig = System.IO.File.ReadAllLines(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"Assassin's Creed Revelations\ACRevelations.ini"));
                    foreach (string line in gameConfig)
                    {
                        List<string> splitLine = new List<string>();
                        switch (line)
                        {
                            case string x when line.StartsWith("DisplayWidth"):
                                splitLine = line.Split('=').ToList();
                                currentResolution = splitLine[1];
                                splitLine.Clear();
                                break;
                            case string x when line.StartsWith("DisplayHeight"):
                                splitLine = line.Split('=').ToList();
                                currentResolution = currentResolution + "x" + splitLine[1];
                                splitLine.Clear();
                                Log.Information($"Selected Resolution: {currentResolution}");
                                ResolutionSelector.SelectedItem = currentResolution;
                                break;
                            case string x when line.StartsWith("RefreshRate"):
                                splitLine = line.Split('=').ToList();
                                RefreshRateSelector.SelectedItem = int.Parse(splitLine[1]);
                                splitLine.Clear();
                                Log.Information($"Selected Refresh Rate: {RefreshRateSelector.SelectedItem.ToString()}");
                                break;
                            case string x when line.StartsWith("MultiSampleType"):
                                splitLine = line.Split('=').ToList();
                                switch (int.Parse(splitLine[1]))
                                {
                                    case 8:
                                        MSAASelector.SelectedIndex = 3;
                                        break;
                                    case 4:
                                        MSAASelector.SelectedIndex = 2;
                                        break;
                                    case 2:
                                        MSAASelector.SelectedIndex = 1;
                                        break;
                                    default:
                                        MSAASelector.SelectedIndex = 0;
                                        break;
                                }
                                Log.Information($"Selected MSAA: {MSAASelector.SelectedItem.ToString()}");
                                splitLine.Clear();
                                break;
                            case string x when line.StartsWith("VSync"):
                                splitLine = line.Split('=').ToList();
                                if (int.Parse(splitLine[1]) == 1)
                                {
                                    Log.Information("VSync is enabled");
                                    VSync.IsChecked = true;
                                }
                                else
                                {
                                    Log.Information("VSync is disabled");
                                }
                                splitLine.Clear();
                                break;
                            case string x when line.StartsWith("EnvironmentQuality"):
                                splitLine = line.Split('=').ToList();
                                EnviromentQualitySelector.SelectedIndex = int.Parse(splitLine[1]);
                                Log.Information($"Enviroment Quality: {EnviromentQualitySelector.SelectedItem.ToString()}");
                                splitLine.Clear();
                                break;
                            case string x when line.StartsWith("TextureQuality"):
                                splitLine = line.Split('=').ToList();
                                TextureQualitySelector.SelectedIndex = int.Parse(splitLine[1]);
                                Log.Information($"Texture Quality: {TextureQualitySelector.SelectedItem.ToString()}");
                                splitLine.Clear();
                                break;
                            case string x when line.StartsWith("ShadowQuality"):
                                splitLine = line.Split('=').ToList();
                                ShadowQualitySelector.SelectedIndex = int.Parse(splitLine[1]);
                                Log.Information($"Shadow Quality: {ShadowQualitySelector.SelectedItem.ToString()}");
                                splitLine.Clear();
                                break;
                            case string x when line.StartsWith("ReflectionQuality"):
                                splitLine = line.Split('=').ToList();
                                ReflectionQualitySelector.SelectedIndex = int.Parse(splitLine[1]);
                                Log.Information($"Reflection Quality: {ReflectionQualitySelector.SelectedItem.ToString()}");
                                splitLine.Clear();
                                break;
                            case string x when line.StartsWith("CharacterQuality"):
                                splitLine = line.Split('=').ToList();
                                CharacterQualitySelector.SelectedIndex = int.Parse(splitLine[1]);
                                Log.Information($"Character Quality: {CharacterQualitySelector.SelectedItem.ToString()}");
                                splitLine.Clear();
                                break;
                            case string x when line.StartsWith("PostFX"):
                                splitLine = line.Split('=').ToList();
                                if (int.Parse(splitLine[1]) == 1)
                                {
                                    Log.Information("PostFX is enabled");
                                    PostFX.IsChecked = true;
                                }
                                else
                                {
                                    Log.Information("PostFX is disabled");
                                }
                                splitLine.Clear();
                                break;
                            default:
                                break;
                        }
                    }
                }
                GC.Collect();
                await Task.Delay(1);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "");
                System.Windows.MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Checks if systemdetection.dll fix is enabled
        /// </summary>
        private async Task CheckTools()
        {
            try
            {
                // SystemDetectionFix
                Log.Information("Checks if systemdetection.dll fix is enabled");
                if (System.IO.File.Exists(App.ACRPath + @"\systemdetection.dll.disabled"))
                {
                    Log.Information("SystemDetectionFix is enabled");
                    SystemDetectionFix.IsChecked = true;
                }
                else
                {
                    Log.Information("SystemDetectionFix is disabled");
                }
                
                GC.Collect();
                await Task.Delay(1);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "");
                System.Windows.MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Checks if Startup Videos are enabled
        /// </summary>
        private async Task CheckStartupVideos()
        {
            try
            {
                // warning_disclaimer.bik
                Log.Information("Checking if startup videos are enabled or disabled");
                bool enabled = true;
                foreach (string directory in Directory.GetDirectories(App.ACRPath + @"\Videos"))
                {
                    if (System.IO.File.Exists(directory + @"\warning_disclaimer.bik"))
                    {
                        enabled = false;
                    }
                }
                // UBI_LOGO.bik
                if (System.IO.File.Exists(App.ACRPath + @"\Videos\UBI_LOGO.bik"))
                {
                    enabled = false;
                }
                if (enabled)
                {
                    Log.Information("Startup videos are disabled.");
                }
                else
                {
                    Log.Information("Startup videos are enabled.");
                }
                SkipIntroVideos.IsChecked = enabled;
                GC.Collect();
                await Task.Delay(1);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "");
                System.Windows.MessageBox.Show(ex.Message);
                return;
            }
        }

        // Saving Settings
        /// <summary>
        /// Saves game settings
        /// </summary>
        private async Task SaveGameSettings()
        {
            try
            {
                string[] gameConfig = File.ReadAllLines(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"Assassin's Creed Revelations\ACRevelations.ini"));
                using (StreamWriter sw = new StreamWriter(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"Assassin's Creed Revelations\ACRevelations.ini")))
                {
                    foreach (string line in gameConfig)
                    {
                        switch (line)
                        {
                            // Display Width
                            case string x when line.StartsWith("DisplayWidth"):
                                foreach (Resolution resolution in App.compatibleResolutions)
                                {
                                    if (resolution.Res == ResolutionSelector.SelectedItem.ToString())
                                    {
                                        sw.Write("DisplayWidth=" + resolution.Width + "\r\n");
                                        break;
                                    }
                                }
                                break;
                            // Display Height
                            case string x when line.StartsWith("DisplayHeight"):
                                foreach (Resolution resolution in App.compatibleResolutions)
                                {
                                    if (resolution.Res == ResolutionSelector.SelectedItem.ToString())
                                    {
                                        sw.Write("DisplayHeight=" + resolution.Height + "\r\n");
                                        break;
                                    }
                                }
                                break;
                            // Refresh Rate
                            case string x when line.StartsWith("RefreshRate"):
                                sw.Write("RefreshRate=" + RefreshRateSelector.SelectedItem.ToString() + "\r\n");
                                break;
                            // MSAA
                            case string x when line.StartsWith("MultiSampleType"):
                                switch (MSAASelector.SelectedIndex)
                                {
                                    case 3:
                                        sw.Write("MultiSampleType=8\r\n");
                                        break;
                                    case 2:
                                        sw.Write("MultiSampleType=4\r\n");
                                        break;
                                    case 1:
                                        sw.Write("MultiSampleType=2\r\n");
                                        break;
                                    default:
                                        sw.Write("MultiSampleType=0\r\n");
                                        break;
                                }
                                break;
                            // VSync
                            case string x when line.StartsWith("VSync"):
                                if (VSync.IsChecked == true)
                                {
                                    sw.Write("VSync=1\r\n");
                                }
                                else
                                {
                                    sw.Write("VSync=0\r\n");
                                }
                                break;
                            // Enviroment Quality
                            case string x when line.StartsWith("EnvironmentQuality"):
                                sw.Write("EnvironmentQuality=" + EnviromentQualitySelector.SelectedIndex + "\r\n");
                                break;
                            // Texture Quality
                            case string x when line.StartsWith("TextureQuality"):
                                sw.Write("TextureQuality=" + TextureQualitySelector.SelectedIndex + "\r\n");
                                break;
                            // Shadow Quality
                            case string x when line.StartsWith("ShadowQuality"):
                                sw.Write("ShadowQuality=" + ShadowQualitySelector.SelectedIndex + "\r\n");
                                break;
                            // Reflection Quality
                            case string x when line.StartsWith("ReflectionQuality"):
                                sw.Write("ReflectionQuality=" + ReflectionQualitySelector.SelectedIndex + "\r\n");
                                break;
                            // Character Quality
                            case string x when line.StartsWith("CharacterQuality"):
                                sw.Write("CharacterQuality=" + CharacterQualitySelector.SelectedIndex + "\r\n");
                                break;
                            // PostFX
                            case string x when line.StartsWith("PostFX"):
                                if (PostFX.IsChecked == true)
                                {
                                    sw.Write("PostFX=1\r\n");
                                }
                                else
                                {
                                    sw.Write("PostFX=0\r\n");
                                }
                                break;
                            default:
                                sw.Write(line + "\r\n");
                                break;
                        }
                    }
                }
                GC.Collect();
                await Task.Delay(1);
            }
            catch (Exception ex)
            {
                Log.Information(ex, "");
                System.Windows.MessageBox.Show(ex.Message);
                return;
            }
        }

        /// <summary>
        /// Saves things like "systemdetection.dll" tweak, ReShade etc.
        /// </summary>
        private async Task SaveOtherSettings()
        {
            try
            {
                // systemdetection.dll check
                if (SystemDetectionFix.IsChecked == true)
                {
                    Log.Information(@"SystemDetection Fix is enabled, disabling systemdetection.dll");
                    if (System.IO.File.Exists(App.ACRPath + @"\systemdetection.dll"))
                    {
                        System.IO.File.Move(App.ACRPath + @"\systemdetection.dll", App.ACRPath + @"\systemdetection.dll.disabled");
                        Log.Information("systemdetection.dll is disabled");
                    }
                }
                else
                {
                    Log.Information(@"SystemDetection Fix is disabled, enabling systemdetection.dll");
                    if (System.IO.File.Exists(App.ACRPath + @"\systemdetection.dll.disabled"))
                    {
                        System.IO.File.Move(App.ACRPath + @"\systemdetection.dll.disabled", App.ACRPath + @"\systemdetection.dll");
                        Log.Information("systemdetection.dll is enabled");
                    }
                }
                await Task.Delay(1);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "");
                System.Windows.MessageBox.Show(ex.Message);
                return;
            }
        }

        /// <summary>
        /// Enables/Disabled startup videos
        /// </summary>
        private async Task SaveStartupVideoSetting()
        {
            try
            {
                if (SkipIntroVideos.IsChecked == true)
                {
                    Log.Information("Disabling startup videos");
                    // warning_disclaimer.bik
                    foreach (string directory in Directory.GetDirectories(App.ACRPath + @"\Videos"))
                    {
                        if (System.IO.File.Exists(directory + @"\warning_disclaimer.bik"))
                        {
                            System.IO.File.Move(directory + @"\warning_disclaimer.bik", directory + @"\warning_disclaimer.bik.disabled");
                        }
                    }
                    // UBI_LOGO.bik
                    if (System.IO.File.Exists(App.ACRPath + @"\Videos\UBI_LOGO.bik"))
                    {
                        System.IO.File.Move(App.ACRPath + @"\Videos\UBI_LOGO.bik", App.ACRPath + @"\Videos\UBI_LOGO.bik.disabled");
                    }
                    Log.Information("Startup videos are disabled");
                }
                else
                {
                    Log.Information("Enabling startup videos");
                    // warning_disclaimer.bik
                    foreach (string directory in Directory.GetDirectories(App.ACRPath + @"\Videos"))
                    {
                        if (System.IO.File.Exists(directory + @"\warning_disclaimer.bik.disabled"))
                        {
                            System.IO.File.Move(directory + @"\warning_disclaimer.bik.disabled", directory + @"\warning_disclaimer.bik");
                        }
                    }
                    // UBI_LOGO.bik
                    if (System.IO.File.Exists(App.ACRPath + @"\Videos\UBI_LOGO.bik.disabled"))
                    {
                        System.IO.File.Move(App.ACRPath + @"\Videos\UBI_LOGO.bik.disabled", App.ACRPath + @"\Videos\UBI_LOGO.bik");
                    }
                    Log.Information("Startup videos are enabled");
                }
                await Task.Delay(1);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "");
                System.Windows.MessageBox.Show(ex.Message);
                return;
            }
        }

        // Events
        /// <summary>
        /// Saves all of the settings when "Save" button is clicked
        /// </summary>
        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Log.Information("Saving Settings");
                await SaveGameSettings();
                await SaveOtherSettings();
                await SaveStartupVideoSetting();
                Log.Information("Saving done");
                System.Windows.MessageBox.Show("Saving done.");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "");
                return;
            }
        }
    }
}
