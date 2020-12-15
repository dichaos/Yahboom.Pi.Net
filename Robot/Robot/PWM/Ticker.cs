using System.Diagnostics;

namespace Robot.PWM
{
    public class Ticker
    {
        private readonly long TicksPerMicrosecond = Stopwatch.Frequency / 1000000L;
        private readonly long ticksToCount;
        private readonly Stopwatch watch = new Stopwatch();

        public Ticker(long pulse)
        {
            ticksToCount = pulse * TicksPerMicrosecond;
        }

        public void Wait()
        {
            watch.Start();

            var nextTrigger = watch.ElapsedTicks + ticksToCount;
            long diff;

            do
            {
                //Thread.Sleep(0);
                diff = nextTrigger - watch.ElapsedTicks;
            } while (diff > 0);

            watch.Stop();
            watch.Reset();
        }
    }
}