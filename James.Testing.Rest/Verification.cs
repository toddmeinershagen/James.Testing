using System;

namespace James.Testing.Rest
{
    public static class Verification
    {
        public static T Verify<T>(this T obj, string verification, Predicate<T> predicate)
        {
            if (!predicate(obj))
                throw new VerificationException(string.Format("Unable to verify {0}", verification));

            return obj;
        }

        public static T Verify<T>(this T obj, Predicate<T> predicate)
        {
            return obj.Verify("custom verification", predicate);
        }
    }
}
