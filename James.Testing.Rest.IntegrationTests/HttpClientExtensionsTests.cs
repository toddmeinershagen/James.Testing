using System;
using System.Linq;
using System.Net.Http;
using FluentAssertions;
using NUnit.Framework;

namespace James.Testing.Rest.IntegrationTests
{
    [TestFixture]
    public class HttpClientExtensionsTests
    {
        [Test]
        public void given_null_headers_object_when_adding_headers_should_not_throw()
        {
            object headers = null;
            var client = new HttpClient();

            Action action = () => client.AddHeaders(headers);

            action.ShouldNotThrow();
        }

        [Test]
        public void given_new_string_headers_when_adding_headers_should_add_headers()
        {
            dynamic headers = new {FirstName = "Todd", LastName = "Meinershagen"};
            var client = new HttpClient();

            client.AddHeaders(headers as object);

            client.DefaultRequestHeaders.GetValues("FirstName").First().Should().Be(headers.FirstName);
            client.DefaultRequestHeaders.GetValues("LastName").First().Should().Be(headers.LastName);
        }

        [Test]
        public void given_new_typed_headers_when_adding_headers_should_add_headers_by_its_string_value()
        {
            dynamic headers = new { Id = Guid.NewGuid(), Strings = new string[2] };
            var client = new HttpClient();

            client.AddHeaders(headers as object);

            client.DefaultRequestHeaders.GetValues("Id").First().Should().Be(headers.Id.ToString());
            client.DefaultRequestHeaders.GetValues("Strings").First().Should().Be(headers.Strings.ToString());
        }

        [Test]
        public void given_new_typed_headers_with_underscores_in_name_when_adding_headers_should_add_header_names_with_dashes()
        {
            dynamic headers = new { x_medassets_auth = "auth", x_requested_with = "XmlHttpRequest" };
            var client = new HttpClient();

            client.AddHeaders(headers as object);

            client.DefaultRequestHeaders.GetValues("x-medassets-auth").First().Should().Be(headers.x_medassets_auth);
            client.DefaultRequestHeaders.GetValues("x-requested-with").First().Should().Be(headers.x_requested_with);
        }

        [Test]
        public void given_null_query_object_when_adding_headers_should_not_throw()
        {
            object query = null;
            var client = new HttpClient();

            Action action = () => client.AddQuery(query);

            action.ShouldNotThrow();
        }

        [Test]
        public void given__when_adding_query_params_to_existing_query_params_when_adding_query_should_add_query_params()
        {
            dynamic query = new { FirstName = "Todd", LastName = "Meinershagen" };
            var client = new HttpClient {BaseAddress = new Uri("http://www.microsoft.com?Id=1")};

            client.AddQuery(query as object);

            client.BaseAddress.Query.Should().Contain(string.Format("&FirstName={0}", query.FirstName));
            client.BaseAddress.Query.Should().Contain(string.Format("&LastName={0}", query.LastName));
        }

        [Test]
        public void given__when_adding_query_params_to_non_existing_query_params_when_adding_query_should_add_query_params()
        {
            dynamic query = new { FirstName = "Todd", LastName = "Meinershagen" };
            var client = new HttpClient {BaseAddress = new Uri("http://www.microsoft.com")};

            client.AddQuery(query as object);

            client.BaseAddress.Query.Should().Contain(string.Format("?FirstName={0}", query.FirstName));
            client.BaseAddress.Query.Should().Contain(string.Format("&LastName={0}", query.LastName));
        }

        [Test]
        public void given__when_adding_query_params_to_non_existing_baseuri_when_adding_query_should_not_throw()
        {
            dynamic query = new { FirstName = "Todd", LastName = "Meinershagen" };
            var client = new HttpClient();

            Action action = () => client.AddQuery(query as object);

            action.ShouldNotThrow();
        }
    }
}
