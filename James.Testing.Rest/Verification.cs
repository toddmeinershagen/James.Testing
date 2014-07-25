using System;

namespace James.Testing.Rest
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
            return value.Verify("custom verification", predicate);
        }

        public static T Store<T, TData>(this T value, out TData data, Func<T, TData> expression)
        {
            data = expression(value);
            return value;
        }
    }
}
