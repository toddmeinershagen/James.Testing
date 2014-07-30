using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Nancy;
using NUnit.Framework;
using HttpStatusCode = System.Net.HttpStatusCode;
using Request = James.Testing.Rest.Request;

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
                .Verify("Body != null", r => r.Body != null)
                .Verify("Count == 2", r => r.Body.Count == 2)
                .Verify("Item 1, FirstName = Todd", r => r.Body[0].FirstName == "Todd")
                .Verify("Item 1, LastName = Meinershagen", r => r.Body[0].LastName == "Meinershagen")
                .Verify("Item 2, FirstName = Brian", r => r.Body[1].FirstName == "Brian")
                .Verify("Item 2, LastName = Ellis", r => r.Body[1].LastName == "Ellis")
                .Verify("StatusCode == OK", r => r.StatusCode == HttpStatusCode.OK);
        }
    }

    [TestFixture]
    public class given_resource_when_getting_as_bytes : HostTestFixture
    {
        [TestFixtureSetUp]
        public void SetUp()
        {
            Request
                .GetAsBytes(GetUriString(GetModule.DocumentsResource) + "/" + Guid.NewGuid());
        }

        [Test]
        public void should_return_bytes()
        {
            Request
                .CurrentResponse<byte[]>()
                .Verify(r => r.Body.Length > 0);
        }

        [Test]
        public void should_be_ok_status()
        {
            Request
                .CurrentResponse<byte[]>()
                .Verify(r => r.StatusCode == HttpStatusCode.OK);
        }

        [Test]
        public void should_return_headers()
        {
            Request
                .CurrentResponse<byte[]>()
                .Verify(r => r.Headers.Any());
        }
    }

    public class GetModule : NancyModule
    {
        public const string PeopleResource = "People";
        public const string DocumentsResource = "Documents";
        private readonly Person[] _people = new[]
        {
            new Person { Id = Guid.NewGuid(), FirstName = "Todd", LastName = "Meinershagen" },
            new Person { Id = Guid.NewGuid(), FirstName = "Brian", LastName = "Ellis" }
        };

        public GetModule()
        {
            Get[PeopleResource] = _ => Negotiate
                .WithModel(_people)
                .WithStatusCode(Nancy.HttpStatusCode.OK);

            Get[DocumentsResource + "/{Id}"] = _ =>
            {
                var filePath = Path.Combine(Environment.CurrentDirectory, "SampleEstimate.pdf");
                return Response.FromByteArray(File.ReadAllBytes(filePath), "application/pdf");
            };
        }
    }
}
