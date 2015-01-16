using System;
using System.Net;
using FluentAssertions;
using NUnit.Framework;

namespace James.Testing.Rest.IntegrationTests.RequestTests
{
    [TestFixture]
    public class WithUriTests
    {
        [Test]
        public void given_valid_url_when_getting_request_with_uri_should_return_request_with_proper_uri_string()
        {
            const string url = "http://www.someuri.org";
            var request = Request.WithUri(url);
            request.UriString.Should().Be(url);
        }

        [Test]
        public void given_invalid_url_when_getting_request_with_uri_should_throw()
        {
            Action action = () => Request.WithUri("not_a_url");
            action.ShouldThrow<UriFormatException>();
        }

        [Test]
        public void given_valid_url_format_with_args_when_getting_request_with_uri_should_return_request_with_proper_uri_string()
        {
            var request = Request.WithUri("http://www.someuri.org/Facilities/{0}/Rooms", 42883);
            request.UriString.Should().Be("http://www.someuri.org/Facilities/42883/Rooms");
        }

        [Test]
        public void given_invalid_url_format_with_args_when_getting_request_with_uri_should_throw()
        {
            Action action = () => Request.WithUri("not_a_url/{0}/Funny", "really");
            action.ShouldThrow<UriFormatException>();
        }
    }
}
