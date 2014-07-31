using System;
using System.ServiceModel;
using NUnit.Framework;

namespace James.Testing.Wcf.IntegrationTests
{
    [TestFixture]
    public abstract class HostFixture<TService>
    {
        private ServiceHost _host;

        [TestFixtureSetUp]
        public void Init()
        {
            _host = new ServiceHost(typeof(TService));
            _host.Open();
            _host.Faulted += HostOnFaulted;
        }

        [TestFixtureTearDown]
        public void Dispose()
        {
            _host.Close();
        }

        private void HostOnFaulted(object sender, EventArgs e)
        {
            Console.WriteLine("The {0} host has faulted.", typeof(TService).Name);
        }
    }
}