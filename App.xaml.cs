using Serilog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;

namespace The_Ezio_Trilogy_Launcher
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        // This is needed for Console to show up when using argument -console
        [DllImport("Kernel32")]
        public static extern void AllocConsole();
        [DllImport("Kernel32")]
        public static extern void FreeConsole();
        public App()
        {
            InitializeComponent();
            // Activating Logging Tool
            Log.Logger = new LoggerConfiguration()
            .WriteTo.Console(
                outputTemplate: "{Timestamp:dd-MM-yyyy HH:mm:ss}|{Level}|{Message}{NewLine}{Exception}")
            //.WriteTo.File("Installer Logs.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();
        }

        // At startup it checks for launch arguments
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            foreach (var argument in e.Args)
            {
                switch (argument)
                {
                    case "-console":
                        AllocConsole();
                        break;
                    default:
                        FreeConsole();
                        break;
                }
            }
            Serilog.Log.Logger = Log.Logger;
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            Log.CloseAndFlush();
        }
    }
}
