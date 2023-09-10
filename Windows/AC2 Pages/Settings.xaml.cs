using Serilog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Markup.Localizer;
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
            ReadConfigurationFiles();
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
                EnviromentQualitySelector.Items.Add("Low");
                EnviromentQualitySelector.Items.Add("Medium");
                EnviromentQualitySelector.Items.Add("High");
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
                ReflectionQualitySelector.Items.Add("High");
                Log.Information("Loading all of the Reflection Quality options into Reflection Quality Selector done.");
                Log.Information("Loading all of the Character Quality options into Character Quality Selector.");
                CharacterQualitySelector.Items.Add("Low");
                CharacterQualitySelector.Items.Add("Medium");
                CharacterQualitySelector.Items.Add("High");
                Log.Information("Loading all of the Character Quality options into Character Quality Selector done.");
                Log.Information("Loading all of the keyboard layout options into the selector.");
                KeyboardLayoutSelector.Items.Add("KeyboardMouse2");
                KeyboardLayoutSelector.Items.Add("KeyboardMouse5");
                KeyboardLayoutSelector.Items.Add("Keyboard");
                KeyboardLayoutSelector.Items.Add("KeyboardAlt");
                Log.Information("Loading all of the keyboard layout options into the selector done.");
                GC.Collect();
                await Task.Delay(10);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "");
            }
        }

        // Read configuration files
        private async void ReadConfigurationFiles()
        {
            Log.Information("Reading configuration files");
            await ReadGameConfig();
            await CheckTools();
            await ReadEaglePatchConfig();
        }

        // Read game configuration file
        private async Task ReadGameConfig()
        {
            try
            {
                Log.Information("Reading game configuration file");
                if (System.IO.File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Ubisoft\Assassin's Creed 2\Assassin2.ini"))
                {
                    Log.Information(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Ubisoft\Assassin's Creed 2\Assassin2.ini");
                    string currentResolution = "";
                    string[] gameConfig = File.ReadAllLines(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Ubisoft\Assassin's Creed 2\Assassin2.ini");
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
                                } else
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

        // Check if ReShade, EaglePatch and uMod are enabled
        private async Task CheckTools()
        {
            try
            {
                Log.Information("Checking if ReShade is enabled");
                if (System.IO.File.Exists(App.AC2Path + @"\dxgi.dll"))
                {
                    Log.Information("ReShade is enabled");
                    ReShade.IsChecked = true;
                }
                else
                {
                    Log.Information("ReShade is disabled");
                }
                Log.Information("Checking if EaglePatch is enabled");
                if (System.IO.File.Exists(App.AC2Path + @"\dinput8.dll"))
                {
                    Log.Information("EaglePatch is enabled");
                    EaglePatch.IsChecked = true;
                }
                else
                {
                    Log.Information("EaglePatch is disabled");
                }
                Log.Information("Checking if uMod is enabled.");
                if (System.IO.File.Exists(App.AC2Path + @"\uMod\Status.txt"))
                {
                    Log.Information("uMod status found.");
                    string[] statusFile = File.ReadAllLines(App.AC2Path + @"\uMod\Status.txt");
                    foreach(string status in statusFile)
                    {
                        if (status.StartsWith("Enabled"))
                        {
                            string[] splitLine = status.Split('=');
                            if (int.Parse(splitLine[1]) == 1)
                            {
                                Log.Information("uMod is enabled");
                                uMod.IsChecked = true;
                                AssassinsCreed2.uModStatus = true;
                            }
                            else
                            {
                                Log.Information("uMod is disabled");
                                uMod.IsChecked = false;
                                AssassinsCreed2.uModStatus = false;
                            }
                        }
                    }
                }
                else
                {
                    Log.Information("uMod status file not found. Enabling it.");
                    using (StreamWriter sw = new StreamWriter(App.AC2Path + @"\uMod\Status.txt"))
                    {
                        sw.Write("Enabled=1");
                    }
                    uMod.IsChecked = true;
                    AssassinsCreed2.uModStatus = true;
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

        // Read EaglePatch configuration file
        private async Task ReadEaglePatchConfig()
        {
            try
            {
                Log.Information("Reading Eagle Patch configuration file");
                if (System.IO.File.Exists(App.AC2Path + @"\scripts\EaglePatchAC2.ini"))
                {
                    string[] EaglePatchConfig = File.ReadAllLines(App.AC2Path + @"\scripts\EaglePatchAC2.ini");
                    foreach (string line in EaglePatchConfig)
                    {
                        List<string> splitLine = new List<string>();
                        switch (line)
                        {
                            case string x when line.StartsWith("ImproveDrawDistance"):
                                splitLine = line.Split('=').ToList();
                                if (int.Parse(splitLine[1]) == 1)
                                {
                                    Log.Information("Improved Draw Distance is enabled");
                                    ImproveDrawDistance.IsChecked = true;
                                }
                                else
                                {
                                    Log.Information("Improved Draw Distance is disabled");
                                }
                                splitLine.Clear();
                                break;
                            case string x when line.StartsWith("ImproveShadowMapResolution"):
                                splitLine = line.Split('=').ToList();
                                if (int.Parse(splitLine[1]) == 1)
                                {
                                    Log.Information("Improved ShadowMap Resolution is enabled");
                                    ImproveShadowMapResolution.IsChecked = true;
                                }
                                else
                                {
                                    Log.Information("Improved ShadowMap Resolution is disabled");
                                }
                                splitLine.Clear();
                                break;
                            case string x when line.StartsWith("KeyboardLayout"):
                                splitLine = line.Split('=').ToList();
                                KeyboardLayoutSelector.SelectedIndex = int.Parse(splitLine[1]);
                                Log.Information($"Selected Keyboard Layout: {KeyboardLayoutSelector.SelectedItem.ToString()}");
                                splitLine.Clear();
                                break;
                            case string x when line.StartsWith("PS3Controls"):
                                splitLine = line.Split('=').ToList();
                                if (int.Parse(splitLine[1]) == 1)
                                {
                                    Log.Information("PS3 Controls are enabled");
                                    PS3Controls.IsChecked = true;
                                }
                                else
                                {
                                    Log.Information("PS3 Controls are disabled");
                                }
                                splitLine.Clear();
                                break;
                            case string x when line.StartsWith("SkipIntroVideos"):
                                splitLine = line.Split('=').ToList();
                                if (int.Parse(splitLine[1]) == 1)
                                {
                                    Log.Information("Skip Intro Videos is enabled");
                                    SkipIntroVideos.IsChecked = true;
                                }
                                else
                                {
                                    Log.Information("Skip Intro Videos is disabled");
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

        // Saving Game Settings
        private async Task SaveGameSettings()
        {
            try
            {
                string[] gameConfig = File.ReadAllLines(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Ubisoft\Assassin's Creed 2\Assassin2.ini");
                using (StreamWriter sw = new StreamWriter(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Ubisoft\Assassin's Creed 2\Assassin2.ini"))
                {
                    foreach(string line in gameConfig)
                    {
                        switch (line)
                        {
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
                            case string x when line.StartsWith("RefreshRate"):
                                sw.Write("RefreshRate=" + RefreshRateSelector.SelectedItem.ToString() + "\r\n");
                                break;
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
                            case string x when line.StartsWith("EnvironmentQuality"):
                                sw.Write("EnvironmentQuality=" + EnviromentQualitySelector.SelectedIndex + "\r\n");
                                break;
                            case string x when line.StartsWith("TextureQuality"):
                                sw.Write("TextureQuality=" + TextureQualitySelector.SelectedIndex + "\r\n");
                                break;
                            case string x when line.StartsWith("ShadowQuality"):
                                sw.Write("ShadowQuality=" + ShadowQualitySelector.SelectedIndex + "\r\n");
                                break;
                            case string x when line.StartsWith("ReflectionQuality"):
                                sw.Write("ReflectionQuality=" + ReflectionQualitySelector.SelectedIndex + "\r\n");
                                break;
                            case string x when line.StartsWith("CharacterQuality"):
                                sw.Write("CharacterQuality=" + CharacterQualitySelector.SelectedIndex + "\r\n");
                                break;
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

        // Enables/Disables EaglePatch and ReShade
        private async Task SaveModLoaderSettings()
        {
            try
            {
                if (ReShade.IsChecked == true)
                {
                    if (System.IO.File.Exists(App.AC2Path + @"\dxgi.dll.disabled"))
                    {
                        System.IO.File.Move(App.AC2Path + @"\dxgi.dll.disabled", App.AC2Path + @"\dxgi.dll");
                        Log.Information("ReShade is enabled");
                    }
                }
                else
                {
                    if (System.IO.File.Exists(App.AC2Path + @"\dxgi.dll"))
                    {
                        System.IO.File.Move(App.AC2Path + @"\dxgi.dll", App.AC2Path + @"\dxgi.dll.disabled");
                        Log.Information("ReShade is disabled");
                    }
                }
                if (EaglePatch.IsChecked == true)
                {
                    if (System.IO.File.Exists(App.AC2Path + @"\dinput8.dll.disabled"))
                    {
                        System.IO.File.Move(App.AC2Path + @"\dinput8.dll.disabled", App.AC2Path + @"\dinput8.dll");
                        Log.Information("EaglePatch is enabled");
                    }
                }
                else
                {
                    if (System.IO.File.Exists(App.AC2Path + @"\dinput8.dll"))
                    {
                        System.IO.File.Move(App.AC2Path + @"\dinput8.dll", App.AC2Path + @"\dinput8.dll.disabled");
                        Log.Information("EaglePatch is disabled");
                    }
                }
                if (uMod.IsChecked == true)
                {
                    using (StreamWriter sw = new StreamWriter(App.AC2Path + @"\uMod\Status.txt"))
                    {
                        sw.Write("Enabled=1");
                    }
                    AssassinsCreed2.uModStatus = true;
                    Log.Information("uMod is enabled");
                }
                else
                {
                    using (StreamWriter sw = new StreamWriter(App.AC2Path + @"\uMod\Status.txt"))
                    {
                        sw.Write("Enabled=0");
                    }
                    AssassinsCreed2.uModStatus = false;
                    Log.Information("uMod is disabled");
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

        // Saving all of the settings
        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            Log.Information("Saving Settings");
            if (PostFX.IsChecked == true && ReShade.IsChecked == true) 
            {
                MessageBoxResult result = System.Windows.MessageBox.Show("PostFX and ReShade are enabled. When you have ReShade enabled, it is recommended that PostFX is disabled. Do you want to continue with this?", "Confirmation", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    await SaveGameSettings();
                    await SaveModLoaderSettings();
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
                Log.Information("Saving done");
                System.Windows.MessageBox.Show("Saving done.");
            }
        }

        // Unlock Bonus Content
        private void UnlockBonusContent_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Log.Information("Unlocking Bonus Content");
                MessageBoxResult result = System.Windows.MessageBox.Show("This is still experimental, so it might breakt he save, but I'll make an backup of all of the files so you don't lose your progress. Do you want to continue?", "Confirmation", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    OpenFileDialog FileDialog = new OpenFileDialog();
                    FileDialog.Filter = "Save Files|1.save";
                    FileDialog.Title = @"Save files are usually in C:\Program Files (x86)\Ubisoft\Ubisoft Game Launcher\savegames\(Ubisoft account ID)\4\";
                    if (System.IO.Directory.Exists(@"C:\Program Files (x86)\Ubisoft\Ubisoft Game Launcher\savegames\"))
                    {
                        FileDialog.InitialDirectory = @"C:\Program Files (x86)\Ubisoft\Ubisoft Game Launcher\savegames\";
                    }
                    if (FileDialog.ShowDialog() == DialogResult.OK)
                    {
                        string Savepath = System.IO.Path.GetDirectoryName(FileDialog.FileName);
                        Log.Information(Savepath);
                        Log.Information("Backing up saves into the folder Backup next to them");
                        string[] files = Directory.GetFiles(System.IO.Path.GetDirectoryName(FileDialog.FileName));
                        if (!System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(FileDialog.FileName) + @"\backup\"))
                        {
                            System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(FileDialog.FileName) + @"\backup\");
                        }
                        foreach (string saveFile in files)
                        {
                            File.Copy(saveFile, System.IO.Path.GetDirectoryName(FileDialog.FileName) + @"\backup\" + System.IO.Path.GetFileName(saveFile), true);
                            Log.Information($"Copied: {System.IO.Path.GetFileName(saveFile)}");
                        }
                        Log.Information("All savefiles are backed up.");
                        Log.Information("Loading all of the offsets into a List");
                        List<long> offsets = new List<long>();
                        using (StreamReader sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("The_Ezio_Trilogy_Launcher.Assets.AC2Offsets.txt")))
                        {
                            string line = sr.ReadLine();
                            while (line != null)
                            {
                                if (long.TryParse(line.Trim(), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out long offset))
                                {
                                    offsets.Add(offset);
                                }
                                line = sr.ReadLine();
                            }
                        }
                        foreach(long offset in offsets)
                        {
                            Log.Information($"Offset: {offset}");
                        }
                        Log.Information("Loading file");
                        using (FileStream fs = new FileStream(FileDialog.FileName, FileMode.Open, FileAccess.ReadWrite))
                        {
                            for (int i = 0; i < offsets.Count; i++)
                            {
                                byte[] value = new byte[2];
                                byte replacementValue = 0x01;
                                fs.Seek(offsets[i], SeekOrigin.Begin);
                                int currentByte = fs.ReadByte();
                                if (currentByte >= 0)
                                {
                                    if (currentByte != replacementValue)
                                    {
                                        fs.Seek(-1, SeekOrigin.Current);
                                        fs.WriteByte(replacementValue);
                                    }
                                }
                            }
                        };
                        System.Windows.MessageBox.Show("Bonus Content unlocked.");
                    }
                    else
                    {
                        Log.Information("Uninstallation cancelled");
                        System.Windows.MessageBox.Show("Unninstallation cancelled");
                        return;
                    }
                }
                else
                {
                    Log.Information("Unlocking Bonus Content cancelled");
                    System.Windows.MessageBox.Show("Unlocking Bonus Content cancelled.");
                    return;
                }
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
