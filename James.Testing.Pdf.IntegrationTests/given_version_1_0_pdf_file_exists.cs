using System;
using System.IO;
using FluentAssertions;
using NUnit.Framework;

namespace James.Testing.Pdf.IntegrationTests
{
    [TestFixture]
    public class given_version_1_0_pdf_file_exists
    {
        [SetUp]
        public void SetUp()
        {
            var filePath = Path.Combine(Environment.CurrentDirectory, "Sample.pdf");
            Content
                .From(filePath);
        }

        [Test]
        public void when_checking_for_pdf_should_be_true()
        {
            Content
                .Current()
                .IsPdf()
                .Should().BeTrue();
        }

        [Test]
        public void when_checking_for_pdf_with_correct_version_should_be_true()
        {
            Content
                .Current()
                .IsPdf(1.0)
                .Should().BeTrue();
        }

        [Test]
        public void when_checking_for_pdf_with_incorrect_version_should_be_false()
        {
            Content
                .Current()
                .IsPdf(1.1)
                .Should().BeFalse();
        }

        [Test]
        public void when_checking_for_correct_number_of_pages_should_be_true()
        {
            Content
                .Current()
                .Has(1)
                .Pages()
                .Should().BeTrue();
        }

        [Test]
        public void when_checking_for_incorrect_number_of_pages_should_be_false()
        {
            Content
                 .Current()
                 .Has(2)
                 .Pages()
                 .Should().BeFalse();
        }

        [Test]
        public void when_checking_if_page_contains_text_contained_within_page_should_be_true()
        {
            Content
                .Current()
                .Page(1)
                .Contains("MONGE")
                .Should().BeTrue();
        }

        [Test]
        public void when_checking_if_page_contains_text_not_contained_within_page_should_be_false()
        {
            Content
                .Current()
                .Page(1)
                .Contains("MONGER")
                .Should().BeFalse();
        }

        [Test]
        public void when_checking_if_page_contains_integer_contained_within_page_should_be_true()
        {
            Content
                .Current()
                .Page(1)
                .Contains(48)
                .Should().BeTrue();
        }

        [Test]
        public void when_checking_if_page_contains_integer_not_contained_within_page_should_be_false()
        {
            Content
                .Current()
                .Page(1)
                .Contains(49)
                .Should().BeFalse();
        }

        [Test]
        public void when_checking_if_page_contains_double_contained_within_page_should_be_true()
        {
            Content
                .Current()
                .Page(1)
                .Contains(48.05)
                .Should().BeTrue();
        }

        [Test]
        public void when_checking_if_page_contains_double_not_contained_within_page_should_be_false()
        {
            Content
                .Current()
                .Page(1)
                .Contains(48.04)
                .Should().BeFalse();
        }

        [Test]
        public void when_checking_if_page_text_contains_a_value_should_be_true()
        {
            Content
                .Current()
                .Page(1)
                .Text()
                .Contains("48.05")
                .Should().BeTrue();
        }
    }
}
