using System;
using FluentAssertions;
using James.Testing.Rest.IntegrationTests.Models;
using NUnit.Framework;

namespace James.Testing.Rest.IntegrationTests
{
    [TestFixture]
    public class VerificationTests
    {
        [Test]
        public void given_value_and_verification_and_predicate_that_fails_when_verifying_should_throw_exception_with_verification_message()
        {
            var value = new Person() {FirstName = "Todd", LastName = "Meinershagen"};

            Action action = () => value.Verify("last name", x => x.LastName == "Barson");

            action.ShouldThrow<VerificationException>().WithMessage("Unable to verify last name");
        }

        [Test]
        public void given_value_and_predicate_that_fails_when_verifying_should_throw_exception_with_standard_message()
        {
            var value = new Person() { FirstName = "Todd", LastName = "Meinershagen" };

            Action action = () => value.Verify(x => x.LastName == "Barson");

            action.ShouldThrow<VerificationException>().WithMessage("Unable to verify custom verification");
        }

        [Test]
        public void given_value_and_verification_and_predicate_that_succeeds_when_verifying_should_not_throw_exception()
        {
            var value = new Person() { FirstName = "Todd", LastName = "Meinershagen" };

            Action action = () => value.Verify("last name", x => x.LastName == "Meinershagen");

            action.ShouldNotThrow();
        }

        [Test]
        public void given_value_and_predicate_that_succeeds_when_verifying_should_not_throw_exception()
        {
            var value = new Person() { FirstName = "Todd", LastName = "Meinershagen" };

            Action action = () => value.Verify(x => x.LastName == "Meinershagen");

            action.ShouldNotThrow();
        }

        [Test]
        public void
            given_object_and_output_parameter_and_expression_when_storing_should_take_value_from_expression_and_store_in_output_parameter()
        {
            var value = new Person {FirstName = "Todd", LastName = "Meinershagen"};
            string output;

            value.Store(out output, x => x.LastName);

            output.Should().Be(value.LastName);
        }
    }
}
