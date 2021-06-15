using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Platform_Racing_3_Server.Game.Match
{
    internal class MatchTimer
    {
        private Stopwatch Stopwatch { get; }
        private TimeSpan Delay { get; set; }

        public MatchTimer()
        {
            this.Stopwatch = new Stopwatch();
        }

        public void Start(TimeSpan delay = default)
        {
            if (!this.Stopwatch.IsRunning)
            {
                this.Stopwatch.Start();

                this.Delay = delay;
            }
        }

        public TimeSpan Elapsed => this.Stopwatch.Elapsed - this.Delay;

        public static MatchTimer StartNew(TimeSpan delay = default)
        {
            MatchTimer timer = new();
            timer.Start(delay);

            return timer;
        }
    }
}
