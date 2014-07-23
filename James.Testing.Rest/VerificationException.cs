using System;

namespace James.Testing.Rest
{
    public class VerificationException : Exception
    {
        public VerificationException(string message)
            : base(message)
        {
        }
    }
}
