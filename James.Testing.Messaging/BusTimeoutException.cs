namespace James.Testing.Messaging
{
    public class BusTimeoutException : BusException
    {
        public const string BusTimeoutMessageFormat = "Your request for a response has expired.  The timeout was set to {0} second(s).";

        public BusTimeoutException(double seconds)
            : base(string.Format(BusTimeoutMessageFormat, seconds))
        { }
    }
}