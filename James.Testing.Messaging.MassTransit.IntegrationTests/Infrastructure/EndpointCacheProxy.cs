using System;
using MassTransit;
using MassTransit.Diagnostics.Introspection;

namespace James.Testing.Messaging.MassTransit.IntegrationTests.Infrastructure
{
    public class EndpointCacheProxy :
        IEndpointCache
    {
        readonly IEndpointCache _endpointCache;

        public EndpointCacheProxy(IEndpointCache endpointCache)
        {
            _endpointCache = endpointCache;
        }

        public void Dispose()
        {
            // we don't dispose, since we're in testing
        }

        public IEndpoint GetEndpoint(Uri uri)
        {
            return _endpointCache.GetEndpoint(uri);
        }

        public void Inspect(DiagnosticsProbe probe)
        {
            _endpointCache.Inspect(probe);
        }
    }
}
