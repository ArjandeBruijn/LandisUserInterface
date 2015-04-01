using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace LandisUserInterface
{
    public static class TimerBackgroundWorker
    {
        public static Timer timer;
        public static BackgroundWorker BackGroundWorker;

        private static void timer_tick(object sender, EventArgs e)
        {
            if (BackGroundWorker.IsBusy == false)
            {
                BackGroundWorker.RunWorkerAsync();
            }
        }

        public static void Initialize()
        {
            timer = new Timer();
            timer.Interval = 500;
            timer.Start();
            timer.Tick += timer_tick;

            BackGroundWorker = new BackgroundWorker();
            
        }
    }
}
