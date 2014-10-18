using System;

namespace James.Testing
{
    public static class Verification
    {
        public static T Verify<T>(this T value, string verification, Predicate<T> predicate)
        {
            if (!predicate(value))
                throw new VerificationException(string.Format("Unable to verify {0}", verification));

            return value;
        }

        public static T Verify<T>(this T value, Predicate<T> predicate)
        {
            return value.Verify("custom verification.", predicate);
        }

        public static T VerifyThat<T>(this T value, Action<T> assertion)
        {
            try
            {
                assertion(value);
                return value;
            }
            catch (Exception ex)
            {
                throw new VerificationException(string.Format("Unable to verify.\r\n{0}", ex.Message));
            }
        }
    }
}
