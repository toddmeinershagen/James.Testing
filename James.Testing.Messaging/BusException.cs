using System;

namespace James.Testing.Messaging
{
    public class BusException : ApplicationException
    {
        public BusException(string message)
            : base(message)
        { }
    }
}
