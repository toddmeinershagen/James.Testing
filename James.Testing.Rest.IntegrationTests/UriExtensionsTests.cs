using System;
using FluentAssertions;
using NUnit.Framework;

namespace James.Testing.Rest.IntegrationTests
{

    [TestFixture]
    public class when_adding_query
    {
        [Test]
        public void given_different_existing_query_params_when_adding_query_should_add_query_params()
        {
            var uri = new Uri("http://www.microsoft.com?Id=1");
            var uriWithQuery = uri.With(DynamicDictionary.FromObject(new {FirstName = "Todd", Age = 42}));

            uriWithQuery.Query.Should().Contain(string.Format("&FirstName={0}", "Todd"));
            uriWithQuery.Query.Should().Contain(string.Format("&Age={0}", 42));
        }

        [Test]
        public void given_same_existing_query_params_when_adding_query_should_replace_query_params()
        {
            var uri = new Uri("http://www.microsoft.com?FirstName=Ellie&Age=14");
            var uriWithQuery =
                uri.With(DynamicDictionary.FromObject(new {FirstName = "Todd", BirthDate = 23.November(1972)}));

            uriWithQuery.Query.Should().Contain(string.Format("&FirstName={0}", "Todd"));
            uriWithQuery.Query.Should().Contain(Uri.EscapeUriString(string.Format("&BirthDate={0}", 23.November(1972))));
        }

        [Test]
        public void given_null_query_object_should_not_throw()
        {
            var uri = new Uri("http://someuri.org");

            Action action = () => uri.With(null);

            action.ShouldNotThrow();
        }

        [Test]
        public void given_no_existing_query_params_should_add_query_params()
        {
            AssertQueryContainsValue(new {Value = "Todd"}, "strings should be supported", "Todd");
            AssertQueryContainsValue(new {Value = 12}, "integers should be supported", 12);
            AssertQueryContainsValue(new {Value = new[] {"123", "456", "789"}}, "string arrays should be supported", "123","456","789");
            AssertQueryContainsValue(new {Value = new[] {123, 456, 789}}, "integer arrays should be supported", 123,456,789);

            var date1 = 23.November(1972);
            var date2 = 24.November(1972);
            AssertQueryContainsValue(new {Value = date1}, "dates should be supported", date1);
            AssertQueryContainsValue(new {Value = new[] {date1, date2}}, "datetime arrays should be supported", date1, date2);
        }

        private void AssertQueryContainsValue(object query, string because = "", params object[] values)
        {
            var uri = new Uri("http://www.microsoft.com");

            var uriWithQuery = uri.With(DynamicDictionary.FromObject(query));

            foreach (var value in values)
            {
                uriWithQuery.Query.Should().Contain(Uri.EscapeUriString(string.Format("Value={0}", value)), because);
            }
        }
    }
}
