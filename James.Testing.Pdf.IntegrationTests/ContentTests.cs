using System;
using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace James.Testing.Pdf.IntegrationTests
{
    [TestFixture]
    public class ContentTests
    {
        [Test]
        public void given_path_when_getting_content_from_path_should_return_document()
        {
            var filePath = Path.Combine(Environment.CurrentDirectory, "Sample.pdf");
            Content
                .From(filePath)
                .Has(1)
                .Pages()
                .Should().BeTrue();
        }

        [Test]
        public void given_bytes_when_getting_content_from_bytes_should_return_document()
        {
            var filePath = Path.Combine(Environment.CurrentDirectory, "Sample.pdf");
            var buffer = File.ReadAllBytes(filePath);
            Content
                .From(buffer)
                .Has(1)
                .Pages()
                .Should().BeTrue();
        }

        [Test]
        public void given_stream_when_getting_content_from_stream_should_return_document()
        {
            var filePath = Path.Combine(Environment.CurrentDirectory, "Sample.pdf");
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                Content
                    .From(stream)
                    .Has(1)
                    .Pages()
                    .Should().BeTrue();
            }
        }

        [Test]
        public void given_false_path_when_getting_content_from_bytes_should_throw()
        {
            var filePath = Path.Combine(Environment.CurrentDirectory, "Sample3.pdf");
            Action action = () => Content
                .From(filePath);

            action.ShouldThrow<FileNotFoundException>();
        }

        [Test]
        public void given_null_bytes_when_getting_content_from_bytes_should_throw()
        {
            Action action = () => Content
                .From(null as byte[]);

            action.ShouldThrow<ArgumentNullException>();
        }

        [Test]
        public void given_bytes_less_than_5_when_getting_content_from_bytes_should_throw()
        {
            Action action = () => Content
                .From(new byte[] {1, 2, 3});

            action.ShouldThrow<ArgumentException>();
        }

        [Test]
        public void given_null_stream_when_getting_content_from_stream_should_throw()
        {
            Action action = () => Content
                .From(null as Stream);

            action.ShouldThrow<ArgumentNullException>();
        }

        [Test]
        public void given_multiple_threads_when_checking_number_of_pages_for_current_content_should_return_true()
        {
            Action action1 = () =>
            {
                var filePath = Path.Combine(Environment.CurrentDirectory, "Sample.pdf");
                Content
                    .From(filePath);

                Content
                    .Current()
                    .Has(1)
                    .Pages()
                    .Should().BeTrue();
            };

            Action action2 = () =>
            {
                var filePath = Path.Combine(Environment.CurrentDirectory, "Sample2.pdf");
                Content
                    .From(filePath);

                Content
                    .Current()
                    .Has(2)
                    .Pages()
                    .Should().BeTrue();
            };

            Parallel.Invoke(action1, action2);
        }

        [Test]
        public void given_multiple_content_loads_when_checking_number_of_pages_for_current_content_should_return_true_for_last_content_pages()
        {
            var filePath = Path.Combine(Environment.CurrentDirectory, "Sample.pdf");
            Content
                .From(filePath);

            filePath = Path.Combine(Environment.CurrentDirectory, "Sample2.pdf");
            Content
                .From(filePath);

            Content
                .Current()
                .Has(2)
                .Pages()
                .Should().BeTrue();
        }
    }
}