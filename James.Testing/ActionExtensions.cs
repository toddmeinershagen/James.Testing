using System;
using System.Diagnostics;
using System.Threading;

namespace James.Testing
{
    public static class ActionExtensions
    {
        public static void ExecuteWithRetries(this Action action, int retryTimes = 1, int waitTimeInSeconds = 0)
        {
            do
            {
                try
                {
                    Thread.Sleep(waitTimeInSeconds * 1001);
                    action();
                    break;
                }
                catch (Exception)
                {
                    if (--retryTimes == 0)
                        throw;
                }
            } while (retryTimes != 0);

        }

        public static void ExecuteWithTimeout(this Action action, int timeoutInSeconds = 0, int waitTimeInSeconds = 0)
        {
            var watch = new Stopwatch();

            do
            {
                try
                {
                    Thread.Sleep(waitTimeInSeconds * 1001);
                    watch.Start();
                    action();
                    watch.Stop();
                    break;
                }
                catch (Exception)
                {
                    if (watch.Elapsed.TotalSeconds >= timeoutInSeconds)
                    {
                        throw;
                    }
                }
            } while (watch.Elapsed.TotalSeconds < timeoutInSeconds);
        }
    }
}
