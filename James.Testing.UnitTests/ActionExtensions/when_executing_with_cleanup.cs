using System;
using FluentAssertions;
using NUnit.Framework;

namespace James.Testing.UnitTests.ActionExtensions
{
    [TestFixture]
    public class when_executing_with_cleanup
    {
        private const string ExpectedMessage = "This is it.";
        private Action _action;
        private int _counter;

        [SetUp]
        public void SetUp()
        {
            Action innerAction = () => { throw new ArgumentException(ExpectedMessage); };

            _counter = 0;
            _action = () =>
            {
                Action cleanup = () => { _counter++; };
                innerAction.ExecuteWithCleanup(cleanup);
            };
        }

        [Test]
        public void given_action_that_throws_should_throw_exception()
        {
            _action
                .ShouldThrow<ArgumentException>()
                .WithMessage(ExpectedMessage);
        }

        [Test]
        public void given_action_that_throws_should_call_cleanup()
        {
            _action.GulpException();
            _counter.Should().Be(1);
        }
    }
}
