using System;
using System.Net.Http;
using FluentAssertions;
using NUnit.Framework;

namespace James.Testing.Rest.IntegrationTests.HttpClientExtensionsTests
{
    [TestFixture]
    public class when_adding_query
    {
        [Test]
        public void given_different_existing_query_params_when_adding_query_should_add_query_params()
        {
            dynamic query = new { FirstName = "Todd", Age=42 };
            var client = new HttpClient {BaseAddress = new Uri("http://www.microsoft.com?Id=1")};

            client.AddQuery(query as object);

            client.BaseAddress.Query.Should().Contain(string.Format("&FirstName={0}", query.FirstName));
            client.BaseAddress.Query.Should().Contain(string.Format("&Age={0}", query.Age));
        }

        [Test]
        public void given_same_existing_query_params_when_adding_query_should_replace_query_params()
        {
            dynamic query = new { FirstName = "Todd", BirthDate = 23.November(1972) };
            var client = new HttpClient { BaseAddress = new Uri("http://www.microsoft.com?FirstName=Ellie&Age=14") };

            client.AddQuery(query as object);

            client.BaseAddress.Query.Should().Contain(string.Format("&FirstName={0}", query.FirstName));
            client.BaseAddress.Query.Should().Contain(Uri.EscapeUriString(string.Format("&BirthDate={0}", query.BirthDate)));
        }

        [Test]
        public void given_no_existing_query_params_should_add_query_params()
        {
            AssertQueryContainsValue(new {Value = "Todd"}, "Todd", "strings should be supported");
            AssertQueryContainsValue(new { Value = 12 }, 12, "integers should be supported");
            AssertQueryContainsValue(new { Value = new []{"123", "456", "789"} }, "123,456,789", "string arrays should be supported");
            AssertQueryContainsValue(new { Value = new[]{ 123, 456, 789 } }, "123,456,789", "integer arrays should be supported");

            var date1 = 23.November(1972);
            var date2 = 24.November(1972);
            AssertQueryContainsValue(new { Value = date1 }, date1, "dates should be supported");
            AssertQueryContainsValue(new { Value = new[] { date1, date2 } }, string.Format("{0},{1}", date1, date2), "datetime arrays should be supported");
        }

        private void AssertQueryContainsValue(dynamic query, object value, string because = "")
        {
            var client = new HttpClient { BaseAddress = new Uri("http://www.microsoft.com") };

            client.AddQuery(query as object);

            client.BaseAddress.Query.Should().Contain(Uri.EscapeUriString(string.Format("?Value={0}", value)), because);
        }

        [Test]
        public void given_no_baseuri_exists_should_not_throw()
        {
            dynamic query = new { FirstName = "Todd", LastName = "Meinershagen" };
            var client = new HttpClient();

            Action action = () => client.AddQuery(query as object);

            action.ShouldNotThrow();
        }
    }
}