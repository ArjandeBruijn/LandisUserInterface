using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace LandisUserInterface
{
    public class TimerBackgroundWorker
    {
        Timer timer;
        BackgroundWorker BackGroundWorker;

        private void timer_tick(object sender, EventArgs e)
        {
            if (this.BackGroundWorker.IsBusy == false)
            {
                BackGroundWorker.RunWorkerAsync();
            }
        }

        public TimerBackgroundWorker(DoWorkEventHandler DoWork, RunWorkerCompletedEventHandler CompleteWork)
        {
            timer = new Timer();
            timer.Interval = 500;
            timer.Start();
            timer.Tick += timer_tick;

            BackGroundWorker = new BackgroundWorker();
            BackGroundWorker.DoWork += DoWork;
            BackGroundWorker.RunWorkerCompleted += CompleteWork;
        }
    }
}
