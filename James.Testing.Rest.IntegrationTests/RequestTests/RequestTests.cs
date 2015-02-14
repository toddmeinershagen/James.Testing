using System;
using System.ComponentModel;
using FluentAssertions;
using NUnit.Framework;

namespace James.Testing.Rest.IntegrationTests.RequestTests
{
    [TestFixture]
    public class RequestTests
    {
        [Test]
        public void given_request_with_uri_and_no_query_tostring_should_return_uri()
        {
            Request
                .WithUri("http://localhost:1234/Estimates")
                .ToString().Should().Be("http://localhost:1234/Estimates");
        }

        [Test]
        public void given_request_with_uri_and_query_tostring_should_return_uri_with_query()
        {
            Request
                .WithUri("http://localhost:1234/Estimates")
                .WithQuery(new {FacilityId = 48223, AccountNumber = "AC12345 1234 1234"})
                .ToString()
                .Should()
                .Be(String.Format("http://localhost:1234/Estimates?FacilityId={0}&AccountNumber={1}", 48223, "AC12345%201234%201234"));
        }

        [Test]
        public void given_request_with_uri_and_headers_tostring_should_return_uri_with_headers()
        {
            Request
                .WithUri("http://localhost:1234/Estimates")
                .WithHeaders(new {Accept = "application/xml", FacilityId = 48223, AccountNumber = "AC12345"})
                .ToString()
                .Should()
                .Be(String.Format("http://localhost:1234/Estimates\r\nAccept: {0}\r\nFacilityId: {1}\r\nAccountNumber: {2}", "application/xml", 48223, "AC12345"));
        }

        [Test]
        public void given_request_with_uri_and_query_and_headers_tostring_should_return_uri_with_query_with_headers()
        {
            Request
                .WithUri("http://localhost:1234/Estimates")
                .WithQuery(new {FacilityId = 48223, AccountNumber = "AC12345"})
                .WithHeaders(new { Accept = "application/xml", FacilityId = 48223, AccountNumber = "AC12345" })
                .ToString()
                .Should()
                .Be(String.Format("http://localhost:1234/Estimates?FacilityId={1}&AccountNumber={2}\r\nAccept: {0}\r\nFacilityId: {1}\r\nAccountNumber: {2}", "application/xml", 48223, "AC12345"));
        }
    }
}
