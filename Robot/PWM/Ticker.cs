using System.ComponentModel;
using System.Diagnostics;
using System.Threading;

namespace Robot.PWM
{
    //From https://github.com/GR8DAN/C-Sharp-Microtimer/blob/master/Ticker/Tick.cs
    public class Ticker : BackgroundWorker
    {
        Stopwatch sw = new Stopwatch();
        public bool Ticking { get; private set; }
        public readonly long TicksPerMicrosecond = Stopwatch.Frequency / 1000000L;
        long ticksToCount = 100L * Stopwatch.Frequency / 1000000L;
        
        public long TicksToCount
        {
            get { return ticksToCount; }
        }
        
        long microseconds = 100L;
        
        public long Microseconds
        {
            get { return Microseconds; }
            set
            {
                microseconds = value;
                ticksToCount = value * TicksPerMicrosecond;
            }
        }

        private void RunStopwatch(object sender, DoWorkEventArgs e)
        {
            long nextTrigger;
            long diff;
            int tick=1;   
            
            sw.Start();
            Ticking = sw.IsRunning;
            
            while(Ticking)
            {
                nextTrigger = sw.ElapsedTicks + ticksToCount;
                do
                {
                    Thread.Sleep(0);
                    diff = nextTrigger - sw.ElapsedTicks;
                } while (diff > 0);
                
                ((BackgroundWorker)sender).ReportProgress(tick);
                tick = (tick==0) ? 1 : 0;
            }
            
            sw.Stop();
            sw.Reset();
        }

        public Ticker(ProgressChangedEventHandler CallOnTick)
        {
            WorkerReportsProgress = true;
            ProgressChanged += CallOnTick;
            DoWork += RunStopwatch;
        }
        
        public void Start()
        {
            if (IsBusy != true)
            {
                RunWorkerAsync();
            }
        }
        
        public void Stop()
        {
            Ticking = false;
        }
    }
}