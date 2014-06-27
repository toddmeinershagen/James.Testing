using System;

namespace James.Abstractions.System
{
    /// <summary>
    /// Ambient context dependency for manipulating time within an application.
    /// </summary>
    /// <remarks>
    /// This is only useful for testing purposes and provides a good local default for production code.  Note that it is not threadsafe.  So, if you decide to leverage 
    /// it in a production multi-threaded application, you will need to make sure that you never try to modify the value of .Current.
    /// </remarks>
    public abstract class TimeProvider
    {
        private static TimeProvider _current;

        static TimeProvider()
        {
            _current = new DefaultTimeProvider();
        }

        public static TimeProvider Current
        {
            get { return _current; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                
                _current = value;
            }
        }

        public abstract DateTime NowAsDateTime { get; }
        public abstract DateTimeOffset NowAsDateTimeOffset { get; }

        public static void ResetToDefault()
        {
            _current = new DefaultTimeProvider();
        }
    }

    /// <summary>
    /// Local default for the TimeProvider ambient context dependency.
    /// </summary>
    internal class DefaultTimeProvider : TimeProvider
    {
        public override DateTime NowAsDateTime
        {
            get { return DateTime.Now; }
        }

        public override DateTimeOffset NowAsDateTimeOffset
        {
            get { return DateTimeOffset.Now; }
        }
    }
}
