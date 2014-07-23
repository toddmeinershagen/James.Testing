using System;
using System.Collections.Generic;
using System.IO;
using Nancy;
using NUnit.Framework;
using HttpStatusCode = System.Net.HttpStatusCode;

namespace James.Testing.Rest.IntegrationTests
{
    [TestFixture]
    public class GetTests : HostTestFixture
    {
        [Test]
        public void given_resource_when_getting_should_return_list()
        {
            Request
                .Get<IList<Person>>(GetUriString(GetModule.PeopleResource))
                .Verify(r => r.Body != null)
                .Verify(r => r.Body.Count == 2)
                .Verify(r => r.Body[0].FirstName == "Todd")
                .Verify(r => r.Body[0].LastName == "Meinershagen")
                .Verify(r => r.Body[1].FirstName == "Brian")
                .Verify(r => r.Body[1].LastName == "Ellis");
        }

        [Test]
        public void given_resource_when_getting_should_be_ok_status()
        {
            Request
                .Get<IEnumerable<Person>>(GetUriString(GetModule.PeopleResource))
                .Verify(r => r.StatusCode == HttpStatusCode.OK);
        }

        [Test]
        public void given_resource_when_getting_as_bytes_should_return_bytes()
        {
            Request
                .GetAsBytes(GetUriString(GetModule.DocumentsResource) + "/" + Guid.NewGuid())
                .Verify(r => r.Body.Length > 0);
        }

        [Test]
        public void given_resource_when_getting_as_bytes_should_be_ok_status()
        {
            Request
                .GetAsBytes(GetUriString(GetModule.DocumentsResource) + "/" + Guid.NewGuid())
                .Verify(r => r.StatusCode == HttpStatusCode.OK);
        }
    }

    public class GetModule : NancyModule
    {
        public const string PeopleResource = "People";
        public const string DocumentsResource = "Documents";

        public GetModule()
        {
            Get[PeopleResource] = _ => Negotiate.WithModel(new[]
            {
                new Person {FirstName = "Todd", LastName = "Meinershagen"},
                new Person {FirstName = "Brian", LastName = "Ellis"}
            });

            Get["/" + DocumentsResource + "/{Id}"] = _ =>
            {
                var filePath = Path.Combine(Environment.CurrentDirectory, "SampleEstimate.pdf");
                return Response.FromByteArray(File.ReadAllBytes(filePath), "application/pdf");
            };
        }
    }
}
