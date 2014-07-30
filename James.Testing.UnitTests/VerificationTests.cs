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
        public void given_empty_guid_when_verifying_that_it_is_not_empty_should_throw_exception()
        {
            var message = new {Id = Guid.Empty};
            Action action = () => message.VerifyThat(v => v.Id).IsNotEmpty();
            action.ShouldThrow<VerificationException>().WithMessage("Unable to verify that guid is not empty.");
        }

        [Test]
        public void given_non_empty_guid_when_verifying_that_it_is_not_empty_should_not_throw()
        {
            var message = new { Id = Guid.NewGuid() };
            Action action = () => message.VerifyThat(v => v.Id).IsNotEmpty();
            action.ShouldNotThrow();
        }

        [Test]
        public void given_empty_guid_when_verifying_that_it_is_empty_should_not_thrown()
        {
            var message = new { Id = Guid.Empty };
            Action action = () => message.VerifyThat(v => v.Id).IsEmpty();
            action.ShouldNotThrow();
        }

        [Test]
        public void given_non_empty_guid_when_verifying_that_it_is_empty_should_throw_exception()
        {
            var message = new { Id = Guid.NewGuid() };
            Action action = () => message.VerifyThat(v => v.Id).IsEmpty();
            action.ShouldThrow<VerificationException>().WithMessage("Unable to verify that guid is empty.");
        }

        [Test]
        public void given_empty_string_when_verifying_that_it_is_not_empty_should_throw_exception()
        {
            var message = new { Name = string.Empty };
            Action action = () => message.VerifyThat(v => v.Name).IsNotEmpty();
            action.ShouldThrow<VerificationException>().WithMessage("Unable to verify that string is not empty.");
        }

        [Test]
        public void given_non_empty_string_when_verifying_that_it_is_not_empty_should_not_throw()
        {
            var message = new { Name = "Todd" };
            Action action = () => message.VerifyThat(v => v.Name).IsNotEmpty();
            action.ShouldNotThrow();
        }

        [Test]
        public void given_empty_string_when_verifying_that_it_is_empty_should_not_thrown()
        {
            var message = new { Name = string.Empty };
            Action action = () => message.VerifyThat(v => v.Name).IsEmpty();
            action.ShouldNotThrow();
        }

        [Test]
        public void given_non_empty_string_when_verifying_that_it_is_empty_should_throw_exception()
        {
            var message = new { Name = "Todd" };
            Action action = () => message.VerifyThat(v => v.Name).IsEmpty();
            action.ShouldThrow<VerificationException>().WithMessage("Unable to verify that string is empty.");
        }
    }
}
