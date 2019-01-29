using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Platform_Racing_3_Server.Game.Match
{
    internal class MatchTimer
    {
        private long StartTimestamp;


        public MatchTimer()
        {

        }

        public void Start(TimeSpan delay = default)
        {
            this.StartTimestamp = Stopwatch.GetTimestamp() + delay.Ticks;
        }

        public TimeSpan Elapsed => TimeSpan.FromTicks(this.GetElapsedTicks());

        private long GetElapsedTicks() => Stopwatch.GetTimestamp() - this.StartTimestamp;

        public static MatchTimer StartNew(TimeSpan delay = default)
        {
            MatchTimer timer = new MatchTimer();
            timer.Start(delay);

            return timer;
        }
    }
}
