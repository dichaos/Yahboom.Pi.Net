using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;

namespace Robot.PWM
{
    //From https://github.com/GR8DAN/C-Sharp-Microtimer/blob/master/Ticker/Tick.cs
    public class Ticker
    {
        readonly Stopwatch watch = new Stopwatch();
        private readonly long TicksPerMicrosecond = Stopwatch.Frequency / 1000000L;
        readonly long ticksToCount = 100L * Stopwatch.Frequency / 1000000L;

        private readonly long _pulse;
        private readonly Action _process;
        
        public Ticker(long pulse)
        {
            _pulse = pulse;
            ticksToCount = pulse * TicksPerMicrosecond;
        }
        
        public void Wait()
        {
            watch.Start();
            
            var nextTrigger = watch.ElapsedTicks + ticksToCount;
            long diff;
            
            do
            {
                Thread.Sleep(0);
                diff = nextTrigger - watch.ElapsedTicks;
            } while (diff > 0);

            watch.Stop();
            watch.Reset();
        }
    }
}