using System;
using System.Collections.Generic;
using System.Linq;
using Nancy;
using NUnit.Framework;
using HttpStatusCode = System.Net.HttpStatusCode;

namespace James.Testing.Rest.IntegrationTests
{
    [TestFixture]
    public class DeleteTests : HostTestFixture
    {
        [Test]
        public void given_resource_when_deleting_should_return_status()
        {
            Request
                .Delete(GetUriString(DeleteModule.PeopleResource) + "/" + DeleteModule.People[0].Id)
                .Verify(r => r.StatusCode == HttpStatusCode.NoContent);
        }

        [Test]
        public void given_resource_when_deleting_should_return_empty_body()
        {
            Request
                .Delete(GetUriString(DeleteModule.PeopleResource) + "/" + DeleteModule.People[0].Id)
                .Verify(r => string.IsNullOrEmpty(r.Body));
        }

    }

    public class DeleteModule : NancyModule
    {
        public const string PeopleResource = "People";

        public static readonly IList<Person> People = new List<Person>
        {
            new Person {Id = Guid.NewGuid(), FirstName = "Todd", LastName = "Meinershagen"},
            new Person {Id = Guid.NewGuid(), FirstName = "Brian", LastName = "Ellis"}
        };

        public DeleteModule()
        {
            Get[PeopleResource] = _ => Negotiate.WithModel(People);

            Delete[PeopleResource + "/{Id}"] = _ =>
            {
                People.Remove(People.FirstOrDefault(x => x.Id == _.Id));
                return Negotiate
                    .WithStatusCode(Nancy.HttpStatusCode.NoContent);
            };
        }
    }
}
