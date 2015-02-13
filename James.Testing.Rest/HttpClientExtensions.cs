using System.Collections.Specialized;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Reflection;

namespace James.Testing.Rest
{
    internal static class HttpClientExtensions
    {
        internal static void AddHeaders(this HttpClient client, object headers)
        {
            if (headers == null) return;

            foreach (var property in headers.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                client.DefaultRequestHeaders.Add(property.Name.Replace("_", "-"),
                    property.GetValue(headers, null).ToString());
            }
        }

        internal static NameValueCollection GetHeaderValues(this object headers)
        {
            var headerValues = new NameValueCollection();

            if (headers == null) return headerValues;

            foreach (var property in headers.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                headerValues.Add(property.Name.Replace("_", "-"), property.GetValue(headers, null).ToString());
            }

            return headerValues;
        }
    }
}