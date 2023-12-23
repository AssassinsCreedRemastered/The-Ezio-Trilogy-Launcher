using System;
using System.Diagnostics;
using System.Timers;
using System.Windows;
using DiscordRPC;

namespace The_Ezio_Trilogy_Launcher.Classes
{
    public class DiscordRPCManager
    {
        private DiscordRpcClient client;
        private Timer timer;
        private Stopwatch stopwatch;
        private string timeElapsed;
        public string IconKey;

        public DiscordRPCManager()
        {
            client = new DiscordRpcClient("1188096756756529192");
            stopwatch = new Stopwatch();
            IconKey = "icon";
        }

        public void InitializePresence(string state="Idle")
        {
            client.Initialize();
            client.SetPresence(new RichPresence()
            {
                State = state,
                Assets = new Assets()
                {
                    LargeImageKey = IconKey
                }
            });
        }

        public void InitializeInGamePresence()
        {
            ResetTimerAndStopwatch();
            timer = new Timer();
            timer.Interval = 10;
            timer.Elapsed += TimerElapsed;
            timer.Start();
            stopwatch.Start();
        }

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            UpdateRichPresence();
        }

        private void UpdateRichPresence()
        {
            try
            {
                TimeElapsedUpdate();
                client.SetPresence(new RichPresence()
                {
                    State = "Playing for " + timeElapsed,
                    Details = client.CurrentPresence.Details,
                    Assets = new Assets()
                    {
                        LargeImageKey = IconKey
                    }
                });

                if (timer.Interval == 10)
                {
                    timer.Interval = 60000;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void UpdateStateAndIcon(string icon, string details = "Idle", string state="")
        {
            client.ClearPresence();
            IconKey = icon;
            client.SetPresence(new RichPresence()
            {
                Details = details,
                State = state,
                Assets = new Assets()
                {
                    LargeImageKey = icon
                }
            });
        }

        private void TimeElapsedUpdate()
        {
            if (stopwatch.Elapsed.Days > 0)
            {
                timeElapsed = stopwatch.Elapsed.Days + " days";
            }
            else if (stopwatch.Elapsed.Hours > 0)
            {
                timeElapsed = stopwatch.Elapsed.Hours + " hours";
            }
            else if (stopwatch.Elapsed.Minutes >= 0)
            {
                timeElapsed = stopwatch.Elapsed.Minutes + " minutes";
            }
        }

        public void ResetTimerAndStopwatch()
        {
            timer?.Stop();
            timer?.Dispose();
            timer = null;

            stopwatch.Reset();
        }
    }
}
