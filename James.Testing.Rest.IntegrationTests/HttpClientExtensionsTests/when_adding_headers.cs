using System;
using System.Linq;
using System.Net.Http;
using FluentAssertions;
using NUnit.Framework;

namespace James.Testing.Rest.IntegrationTests.HttpClientExtensionsTests
{
    [TestFixture]
    public class when_adding_headers
    {
        [Test]
        public void given_null_headers_object_should_not_throw()
        {
            object headers = null;
            var client = new HttpClient();

            Action action = () => client.AddHeaders(headers);

            action.ShouldNotThrow();
        }

        [Test]
        public void given_new_string_headers_should_add_headers()
        {
            dynamic headers = new {FirstName = "Todd", LastName = "Meinershagen"};
            var client = new HttpClient();

            client.AddHeaders(headers as object);

            client.DefaultRequestHeaders.GetValues("FirstName").First().Should().Be(headers.FirstName);
            client.DefaultRequestHeaders.GetValues("LastName").First().Should().Be(headers.LastName);
        }

        [Test]
        public void given_new_typed_headers_should_add_headers_by_its_string_value()
        {
            dynamic headers = new {Id = Guid.NewGuid(), Strings = new string[2]};
            var client = new HttpClient();

            client.AddHeaders(headers as object);

            client.DefaultRequestHeaders.GetValues("Id").First().Should().Be(headers.Id.ToString());
            client.DefaultRequestHeaders.GetValues("Strings").First().Should().Be(headers.Strings.ToString());
        }

        [Test]
        public void
            given_new_typed_headers_with_underscores_in_name_should_add_header_names_with_dashes()
        {
            dynamic headers = new {x_medassets_auth = "auth", x_requested_with = "XmlHttpRequest"};
            var client = new HttpClient();

            client.AddHeaders(headers as object);

            client.DefaultRequestHeaders.GetValues("x-medassets-auth").First().Should().Be(headers.x_medassets_auth);
            client.DefaultRequestHeaders.GetValues("x-requested-with").First().Should().Be(headers.x_requested_with);
        }
    }
}