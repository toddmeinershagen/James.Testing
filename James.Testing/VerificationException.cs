using System;

namespace James.Testing
{
    public class VerificationException : Exception
    {
        public VerificationException(string message)
            : base(message)
        {
        }
    }
}
