using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Serilog;

namespace The_Ezio_Trilogy_Launcher.Classes
{
    public class AffinityManager
    {
        public async Task SetProcessAffinity(string executableName)
        {
            try
            {
                Log.Information("Grabbing game process by ID to change affinity.");
                Process[] processes = Process.GetProcessesByName(executableName);
                while (processes.Length <= 0)
                {
                    processes = Process.GetProcessesByName(executableName);
                    await Task.Delay(1000);
                }
                Log.Information($"Game process found.");

                SetAffinityBasedOnCores(processes);

                GC.Collect();
                await Task.Delay(1);
            }
            catch (Exception ex)
            {
                Log.Information(ex, "");
                return;
            }
        }

        private void SetAffinityBasedOnCores(Process[] processes)
        {
            int affinity;
            switch (true)
            {
                case bool when App.NumberOfCores >= 8 && App.NumberOfThreads >= 16:
                    Log.Information("8 Cores/16 Threads or greater affinity"); ;
                    foreach (Process gameProcess in processes)
                    {
                        Log.Information($"Game Process: {gameProcess.ProcessName}, ID: {gameProcess.Id}");
                        gameProcess.ProcessorAffinity = new IntPtr(0xFFFF);
                    }
                    break;
                case bool when App.NumberOfCores == 6 && App.NumberOfThreads == 12:
                    Log.Information("6 Cores/12 Threads affinity");
                    foreach (Process gameProcess in processes)
                    {
                        Log.Information($"Game Process: {gameProcess.ProcessName}, ID: {gameProcess.Id}");
                        gameProcess.ProcessorAffinity = new IntPtr(0x7F);
                    }
                    break;
                case bool when App.NumberOfCores == 6 && App.NumberOfThreads == 6:
                    Log.Information("6 Cores/6 Threads affinity");
                    foreach (Process gameProcess in processes)
                    {
                        Log.Information($"Game Process: {gameProcess.ProcessName}, ID: {gameProcess.Id}");
                        gameProcess.ProcessorAffinity = new IntPtr(0x3F);
                    }
                    break;
                case bool when App.NumberOfCores == 8 && App.NumberOfThreads == 8:
                case bool when App.NumberOfCores == 4 && App.NumberOfThreads == 8:
                    Log.Information("4 Cores/8 Threads or 8 Cores/8 Threads affinity");
                    foreach (Process gameProcess in processes)
                    {
                        Log.Information($"Game Process: {gameProcess.ProcessName}, ID: {gameProcess.Id}");
                        gameProcess.ProcessorAffinity = new IntPtr(0xFF);
                    }
                    break;
                case bool when App.NumberOfCores == 4 && App.NumberOfThreads == 4:
                    Log.Information("4 Cores/4 Threads affinity");
                    foreach (Process gameProcess in processes)
                    {
                        Log.Information($"Game Process: {gameProcess.ProcessName}, ID: {gameProcess.Id}");
                        gameProcess.ProcessorAffinity = new IntPtr(0x0F);
                    }
                    break;
                default:
                    Log.Information("Default preset");
                    affinity = (1 << App.NumberOfThreads) - 1;
                    Log.Information($"Affinity Bitmask: 0x{affinity.ToString("X")}");
                    foreach (Process gameProcess in processes)
                    {
                        Log.Information($"Game Process: {gameProcess.ProcessName}, ID: {gameProcess.Id}");
                        gameProcess.ProcessorAffinity = new IntPtr(affinity);
                    }
                    break;
            }
        }

        private void SetAffinityForProcesses(Process[] processes, int affinityMask)
        {
            foreach (Process gameProcess in processes)
            {
                Log.Information($"Game Process: {gameProcess.ProcessName}, ID: {gameProcess.Id}");
                gameProcess.ProcessorAffinity = new IntPtr(affinityMask);
            }
        }
    }
}
