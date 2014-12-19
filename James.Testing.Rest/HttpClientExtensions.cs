using System.Net.Http;
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
    }
}