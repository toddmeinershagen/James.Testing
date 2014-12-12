using System;
using System.Diagnostics;

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
                    Wait.For(waitTimeInSeconds).Seconds();
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
                    Wait.For(waitTimeInSeconds).Seconds();
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

        public static void GulpException(this Action action)
        {
            try
            {
                action();
            }
            catch
            {}
        }

        public static void ExecuteWithCleanup(this Action action, Action cleanup)
        {
            try
            {
                action();
            }
            finally
            {
                cleanup();
            }
        }
    }
}
