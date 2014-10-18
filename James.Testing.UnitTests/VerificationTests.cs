using System;
using FluentAssertions;
using NUnit.Framework;

namespace James.Testing.UnitTests
{
    [TestFixture]
    public class VerificationTests
    {
        [Test]
        public void given_value_and_verification_and_predicate_that_fails_when_verifying_should_throw_exception_with_verification_message()
        {
            var value = new Person() {FirstName = "Todd", LastName = "Meinershagen"};

            Action action = () => value.Verify("last name.", x => x.LastName == "Barson");

            action.ShouldThrow<VerificationException>().WithMessage("Unable to verify last name.");
        }

        [Test]
        public void given_value_and_predicate_that_fails_when_verifying_should_throw_exception_with_standard_message()
        {
            var value = new Person() { FirstName = "Todd", LastName = "Meinershagen" };

            Action action = () => value.Verify(x => x.LastName == "Barson");

            action.ShouldThrow<VerificationException>().WithMessage("Unable to verify custom verification.");
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
        public void given_assertion_that_does_not_fail_should_return_original_value()
        {
            var message = new {Name = "Todd"};
            var result = message.VerifyThat(m => m.Name.Should().Be("Todd"));

            result.Should().Be(message);
        }

        [Test]
        public void given_assertion_that_fails_should_throw()
        {
            var message = new { Name = "Todd" };
            Action action = () => message.VerifyThat(m => m.Name.Should().Be("Tammy"));

            action
                .ShouldThrow<VerificationException>()
                .WithMessage("Unable to verify.\r\nExpected string to be \"Tammy\" with a length of 5, but \"Todd\" has a length of 4.");
        }
    }
}
