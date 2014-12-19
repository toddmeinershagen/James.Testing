using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace James.Testing.Rest
{
    internal static class UriExtensions
    {
        internal static Uri With(this Uri uri, DynamicDictionary query)
        {
            if (query == null) return uri;
            if (uri == null) return null;

            var uriBuilder = new UriBuilder(uri);

            foreach (var name in query.GetMemberNames())
            {
                var value = query.Get(name);
                var enumerableObjects = value as IEnumerable<object>;
                var enumerableStructs = value.GetType() != typeof (string) ? value as IEnumerable : null;

                if (enumerableObjects == null)
                {
                    if (enumerableStructs == null)
                    {
                        AddQuery(uriBuilder, name, value);
                    }
                    else
                    {
                        AddQuery(uriBuilder, name, enumerableStructs.Cast<object>());
                    }
                }
                else
                {
                    AddQuery(uriBuilder, name, enumerableObjects);

                }
                
             }

            return uriBuilder.Uri;
        }

        private static void AddQuery(UriBuilder uriBuilder, string name, IEnumerable<object> values)
        {
            values.ToList().ForEach(v => AddQuery(uriBuilder, name, v));
        }

        private static void AddQuery(UriBuilder uriBuilder, string name, object value)
        {
            var queryToAppend = string.Format("{0}={1}", name, value);

            if (uriBuilder.Query.Length > 1)
                uriBuilder.Query = uriBuilder.Query.Substring(1) + "&" + queryToAppend;
            else
                uriBuilder.Query = queryToAppend;
        }
    }
}