using System;
using System.Net;

namespace James.Testing.Rest
{
    public static class Verification
    {
        public static T Store<T, TData>(this T value, out TData data, Func<T, TData> expression)
        {
            data = expression(value);
            return value;
        }
    }

    public static class VerifyableOfHttpStatusCodeExtensions
    {
        public static T Is<T>(this IVerifyable<T, HttpStatusCode> verifyable, HttpStatusCode statusCode)
        {
            return verifyable.VerifyThat(v => v == statusCode, "status code is {0}.  Found {1}.", statusCode, verifyable.Value);
        }
    }
}
