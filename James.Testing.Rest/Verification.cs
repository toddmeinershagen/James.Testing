using System;
using System.Net;

namespace James.Testing.Rest
{
    public static class VerifyableOfHttpStatusCodeExtensions
    {
        public static T Is<T>(this IVerifyable<T, HttpStatusCode> verifyable, HttpStatusCode statusCode)
        {
            return verifyable.VerifyThat(v => v == statusCode, "status code is {0}.  Found {1}.", statusCode, verifyable.Value);
        }
    }

    public static class ResponseVerifications
    {
        public static IResponse<TResponse, TError> Verify<TResponse, TError>(this IResponse<TResponse, TError> response, Func<IResponse<TResponse, TError>, bool> predicate, string verification, params object[] args)
        {
            if (predicate(response))
                return response;

            var message = string.Format(verification, args);
            throw new VerificationException(string.Format("Unable to verify.  {0}", message));
        }
    }
}
