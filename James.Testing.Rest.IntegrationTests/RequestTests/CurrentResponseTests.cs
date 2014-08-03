using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using James.Testing.Rest.IntegrationTests.Models;
using NUnit.Framework;

namespace James.Testing.Rest.IntegrationTests.RequestTests
{
    [TestFixture]
    public class CurrentResponseTests : HostTestFixture
    {
        [Test]
        public void given_no_previous_request_when_getting_current_response_should_return_null()
        {
            Action action = () => Request
                .CurrentResponse<Guid>()
                .Should().BeNull();

            Parallel.Invoke(action);
        }

        [Test]
        public void given_previous_request_when_getting_current_response_should_return_response()
        {
            Action action = () =>
            {
                Request
                    .WithUri(GetUriString(GetModule.PeopleResource))
                    .Get<IList<Person>>();

                var response = Request.CurrentResponse<IList<Person>>();
                response.Should().NotBeNull();
            };

            Parallel.Invoke(action);
        }

        [Test]
        public void given_multiple_threads_when_getting_current_response_should_return_thread_specific_response()
        {
            Action action1 = () =>
            {
                var person = new Person {FirstName = "Tammy", LastName = "Ellis"};
                Request
                    .WithUri(GetUriString(PostModule.Resource))
                    .Post<Person, Guid>(person);

                Request
                    .CurrentResponse<Guid>()
                    .Body.Should().NotBeEmpty();
            };

            Action action2 = () =>
            {
                Request
                    .WithUri(GetUriString(GetModule.PeopleResource))
                    .Get<IList<Person>>();
                
                Request
                    .CurrentResponse<IList<Person>>()
                    .Body.Should().NotBeNull();
            };

            Parallel.Invoke(action1, action2);
        }

        [Test]
        public void given_previous_request_with_different_type_when_getting_current_response_should_be_null()
        {
            Action action = () =>
            {
                Request
                    .WithUri(GetUriString(GetModule.PeopleResource))
                    .Get<IList<Person>>();

                Request
                    .CurrentResponse<Guid>()
                    .Should().BeNull();
            };

            Parallel.Invoke(action);
        }
    }
}
