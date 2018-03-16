using System;
using System.Threading.Tasks;
using System.Timers;

namespace Gustoso.Common.Helpers
{
    public static class TimerConfigurator
    {
        public static Timer GetConfiguredTimer(int secondInterval, Func<Task> elapsedFunction)
        {
            Timer timer = new Timer()
            {
                Interval = 1000 * secondInterval,
                Enabled = true,
                AutoReset = true
            };

            timer.Elapsed += async (sender, e) => await elapsedFunction.Invoke();
            return timer;
        }
    }
}
