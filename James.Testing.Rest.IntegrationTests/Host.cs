using System;
using Nancy.Hosting.Self;

namespace James.Testing.Rest.IntegrationTests
{
    public class Host : IHost
    {
        private readonly NancyHost _host;

        public Host(Uri baseUri)
        {
            _host = new NancyHost(baseUri);
        }

        public void Start()
        {
            _host.Start();
        }

        public void Stop()
        {
            _host.Stop();
        }
    }
}
