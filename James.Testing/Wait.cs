using System;
using System.Diagnostics;
using System.Threading;

namespace James.Testing
{
    public class Wait
    {
        public static IWaitFor For(int numberOf)
        {
            if (numberOf < 0)
                throw new ArgumentOutOfRangeException("numberOf", "Number must be non-negative.");

            return new WaitFor(numberOf);
        }

        public static void Until(Func<bool> predicate)
        {
            Until(predicate, TimeSpan.FromSeconds(15));
        }

        public static void Until(Func<bool> predicate, int timeoutInSeconds)
        {
            Until(predicate, TimeSpan.FromSeconds(timeoutInSeconds));
        }

        public static void Until(Func<bool> predicate, TimeSpan timeout)
        {
            var watch = new Stopwatch();
            watch.Start();

            while ((predicate == null || predicate() == false) && watch.Elapsed < timeout)
            {}
        }

        private class WaitFor : IWaitFor
        {
            private readonly int _numberOf;

            public WaitFor(int numberOf)
            {
                _numberOf = numberOf;
            }

            public void Milliseconds()
            {
                Thread.Sleep(_numberOf);
            }

            public void Seconds()
            {
                Thread.Sleep(TimeSpan.FromSeconds(_numberOf));
            }
        }
    }

    public interface IWaitFor
    {
        void Milliseconds();
        void Seconds();
    }
}