using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;
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
using The_Ezio_Trilogy_Launcher.Classes;

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
                ShadowQualitySelector.Items.Add("Very High");
                ShadowQualitySelector.Items.Add("Ultra");
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

        // Reading settings
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
                //System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)), @"Saved Games\Assassin's Creed Brotherhood\ACBrotherhood.ini"))
                if (System.IO.File.Exists(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)), @"Saved Games\Assassin's Creed Brotherhood\ACBrotherhood.ini")))
                {
                    Log.Information(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)), @"Saved Games\Assassin's Creed Brotherhood\ACBrotherhood.ini"));
                    string currentResolution = "";
                    string[] gameConfig = System.IO.File.ReadAllLines(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)), @"Saved Games\Assassin's Creed Brotherhood\ACBrotherhood.ini"));
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
        /// Checks if ReShade, EaglePatch and uMod are enabled
        /// </summary>
        private async Task CheckTools()
        {
            try
            {
                Log.Information("Checking if ReShade is enabled");
                if (System.IO.File.Exists(App.ACBPath + @"\scripts\d3d9.asi"))
                {
                    Log.Information("ReShade is enabled");
                    ReShade.IsChecked = true;
                }
                else
                {
                    Log.Information("ReShade is disabled");
                }
                Log.Information("Checking if uMod is enabled.");
                if (System.IO.File.Exists(App.ACBPath + @"\uMod\Status.txt"))
                {
                    Log.Information("uMod status found.");
                    string[] statusFile = File.ReadAllLines(App.ACBPath + @"\uMod\Status.txt");
                    foreach (string status in statusFile)
                    {
                        if (status.StartsWith("Enabled"))
                        {
                            string[] splitLine = status.Split('=');
                            if (int.Parse(splitLine[1]) == 1)
                            {
                                Log.Information("uMod is enabled");
                                uMod.IsChecked = true;
                                App.ACBuModStatus = true;
                            }
                            else
                            {
                                Log.Information("uMod is disabled");
                                uMod.IsChecked = false;
                                App.ACBuModStatus = false;
                            }
                        }
                    }
                }
                else
                {
                    Log.Information("uMod status file not found. Enabling it.");
                    using (StreamWriter sw = new StreamWriter(App.ACBPath + @"\uMod\Status.txt"))
                    {
                        sw.Write("Enabled=1");
                    }
                    uMod.IsChecked = true;
                    App.ACBuModStatus = true;
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
                foreach (string directory in Directory.GetDirectories(App.ACBPath + @"\Videos"))
                {
                    if (System.IO.File.Exists(directory + @"\warning_disclaimer.bik"))
                    {
                        enabled = false;
                    }
                }
                // UBI_LOGO.bik
                if (System.IO.File.Exists(App.ACBPath + @"\Videos\UBI_LOGO.bik"))
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
                string[] gameConfig = File.ReadAllLines(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)), @"Saved Games\Assassin's Creed Brotherhood\ACBrotherhood.ini"));
                using (StreamWriter sw = new StreamWriter(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)), @"Saved Games\Assassin's Creed Brotherhood\ACBrotherhood.ini")))
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
        /// Enables/Disabled EaglePatch and ReShade, depending on the selected option
        /// </summary>
        private async Task SaveModLoaderSettings()
        {
            try
            {
                if (ReShade.IsChecked == true)
                {
                    if (System.IO.File.Exists(App.ACBPath + @"\scripts\d3d9.asi.disabled"))
                    {
                        System.IO.File.Move(App.ACBPath + @"\scripts\d3d9.asi.disabled", App.ACBPath + @"\scripts\d3d9.asi");
                        Log.Information("ReShade is enabled");
                    }
                }
                else
                {
                    if (System.IO.File.Exists(App.ACBPath + @"\scripts\d3d9.asi"))
                    {
                        System.IO.File.Move(App.ACBPath + @"\scripts\d3d9.asi", App.ACBPath + @"\scripts\d3d9.asi.disabled");
                        Log.Information("ReShade is disabled");
                    }
                }
                if (uMod.IsChecked == true)
                {
                    using (StreamWriter sw = new StreamWriter(App.ACBPath + @"\uMod\Status.txt"))
                    {
                        sw.Write("Enabled=1");
                    }
                    App.ACBuModStatus = true;
                    Log.Information("uMod is enabled");
                }
                else
                {
                    using (StreamWriter sw = new StreamWriter(App.ACBPath + @"\uMod\Status.txt"))
                    {
                        sw.Write("Enabled=0");
                    }
                    App.ACBuModStatus = false;
                    Log.Information("uMod is disabled");
                }
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
                    foreach (string directory in Directory.GetDirectories(App.ACBPath + @"\Videos"))
                    {
                        if (System.IO.File.Exists(directory + @"\warning_disclaimer.bik"))
                        {
                            System.IO.File.Move(directory + @"\warning_disclaimer.bik", directory + @"\warning_disclaimer.bik.disabled");
                        }
                    }
                    // UBI_LOGO.bik
                    if (System.IO.File.Exists(App.ACBPath + @"\Videos\UBI_LOGO.bik"))
                    {
                        System.IO.File.Move(App.ACBPath + @"\Videos\UBI_LOGO.bik", App.ACBPath + @"\Videos\UBI_LOGO.bik.disabled");
                    }
                    Log.Information("Startup videos are disabled");
                }
                else
                {
                    Log.Information("Enabling startup videos");
                    // warning_disclaimer.bik
                    foreach (string directory in Directory.GetDirectories(App.ACBPath + @"\Videos"))
                    {
                        if (System.IO.File.Exists(directory + @"\warning_disclaimer.bik.disabled"))
                        {
                            System.IO.File.Move(directory + @"\warning_disclaimer.bik.disabled", directory + @"\warning_disclaimer.bik");
                        }
                    }
                    // UBI_LOGO.bik
                    if (System.IO.File.Exists(App.ACBPath + @"\Videos\UBI_LOGO.bik.disabled"))
                    {
                        System.IO.File.Move(App.ACBPath + @"\Videos\UBI_LOGO.bik.disabled", App.ACBPath + @"\Videos\UBI_LOGO.bik");
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



        /// <summary>
        /// Downloads DLC Unlocker
        /// </summary>
        private async Task DownloadDLCUnlocker()
        {
            try
            {
                Log.Information($"Downloading DLC Unlocker");
                using (HttpClient client = new HttpClient())
                {
                    using (var response = await client.GetAsync("https://github.com/AssassinsCreedRemastered/The-Ezio-Trilogy-Mods/releases/download/ACBMods/OPTIONS", HttpCompletionOption.ResponseHeadersRead))
                    {
                        response.EnsureSuccessStatusCode();
                        using (var stream = await response.Content.ReadAsStreamAsync())
                        using (var fileStream = new FileStream(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)), @"Saved Games\Assassin's Creed Brotherhood\SAVES\OPTIONS"), FileMode.Create, FileAccess.Write, FileShare.None))
                        {
                            var totalBytes = response.Content.Headers.ContentLength ?? -1;
                            var buffer = new byte[8192];
                            var bytesRead = 0;
                            var totalRead = 0;
                            do
                            {
                                bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                                if (bytesRead > 0)
                                {
                                    await fileStream.WriteAsync(buffer, 0, bytesRead);
                                    totalRead += bytesRead;
                                }
                            } while (bytesRead > 0);
                        }
                    }
                }
                Log.Information("Download finished");
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

        // Events
        /// <summary>
        /// Saves all of the settings when "Save" button is clicked
        /// </summary>
        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Log.Information("Saving Settings");
                if (PostFX.IsChecked == true && ReShade.IsChecked == true)
                {
                    MessageBoxResult result = System.Windows.MessageBox.Show("PostFX and ReShade are enabled. When you have ReShade enabled, it is recommended that PostFX is disabled. Do you want to continue with this?", "Confirmation", MessageBoxButton.YesNo);
                    if (result == MessageBoxResult.Yes)
                    {
                        await SaveGameSettings();
                        await SaveModLoaderSettings();
                        await SaveStartupVideoSetting();
                        Log.Information("Saving done");
                        System.Windows.MessageBox.Show("Saving done.");
                    }
                    else
                    {
                        Log.Information("Saving Settings cancelled");
                    }
                }
                else
                {
                    await SaveGameSettings();
                    await SaveModLoaderSettings();
                    await SaveStartupVideoSetting();
                    Log.Information("Saving done");
                    System.Windows.MessageBox.Show("Saving done.");
                }
                GC.Collect();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "");
                return;
            }
        }

        /// <summary>
        /// Unlock Bonus Content when the "Unlock Bonus Content" button is clicked
        /// </summary>
        private async void UnlockBonusContent_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBoxResult result = System.Windows.MessageBox.Show("This is still experimental, so it might breakt he save, but I'll make an backup of all of the files so you don't lose your progress. Do you want to continue?", "Confirmation", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    Log.Information("Unlocking bonus content");
                    if (System.IO.File.Exists(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)), @"Saved Games\Assassin's Creed Brotherhood\SAVES\OPTIONS")))
                    {
                        System.IO.File.Move(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)), @"Saved Games\Assassin's Creed Brotherhood\SAVES\OPTIONS"), System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)), @"Saved Games\Assassin's Creed Brotherhood\SAVES\OPTIONS Backup"),true);
                    }
                    await DownloadDLCUnlocker();
                }
                else
                {
                    return;
                }
                Log.Information("Bonus content unlocked");
                GC.Collect();
                System.Windows.MessageBox.Show("Bonus content unlocked.");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "");
                System.Windows.MessageBox.Show(ex.Message);
                return;
            }
        }
    }
}
