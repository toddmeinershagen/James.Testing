using System;
using NUnit.Framework;

namespace James.Testing.Rest.IntegrationTests
{
    [TestFixture]
    public abstract class HostTestFixture
    {
        private IHost _host;

        [TestFixtureSetUp]
        public void Init()
        {
            _host = new Host(new Uri(BaseUrl));
            _host.Start();
        }

        [TestFixtureTearDown]
        public void Dispose()
        {
            _host.Stop();
        }

        protected string BaseUrl
        {
            get { return "http://localhost:1234"; }
        }

        protected string GetUriString(string resource)
        {
            return string.Format("{0}/{1}", BaseUrl, resource);
        }
    }
}
