using System;
using System.Diagnostics;
using FluentAssertions;
using NUnit.Framework;

namespace James.Testing.UnitTests.ActionExtensions
{
    public class when_executing_with_timeout : BaseTest
    {
        [Test]
        public void given_an_action_that_does_not_throw_should_execute_only_once()
        {
            int counter = 0;
            Action action = () => counter++;

            const int timeoutInSeconds = 5;

            action.ExecuteWithTimeout(timeoutInSeconds);

            counter.Should().Be(1);
        }

        [Test]
        public void given_an_action_that_always_throws_should_execute_until_timeout()
        {
            Action action = () => { throw new Exception(); };

            const int timeoutInSeconds = 5;

            var watch = new Stopwatch();
            watch.Start();
            Gulp(() => action.ExecuteWithTimeout(timeoutInSeconds));
            watch.Stop();

            watch.Elapsed.TotalSeconds.Should().BeGreaterOrEqualTo(timeoutInSeconds);
        }

        [Test]
        public void given_an_action_that_always_throws_should_bubble_up_exception()
        {
            Action action = () => { throw new Exception(); };

            const int timeoutInSeconds = 1;

            Action outerAction = () => action.ExecuteWithTimeout(timeoutInSeconds);
            outerAction.ShouldThrow<Exception>();
        }

        [Test]
        public void
            given_an_action_that_throws_only_once_and_wait_time_of_2_seconds_should_only_take_wait_time_to_execute
            ()
        {
            int counter = 0;
            Action action = () =>
            {
                if (++counter == 1)
                {
                    throw new Exception();
                }
            };

            const int timeoutInSeconds = 5;
            const int waitTimeInSeconds = 2;

            var watch = new Stopwatch();
            watch.Start();
            action.ExecuteWithTimeout(timeoutInSeconds, waitTimeInSeconds);
            watch.Stop();

            watch.Elapsed.TotalSeconds.Should().BeGreaterOrEqualTo(waitTimeInSeconds);
        }

        [Test]
        public void given_an_action_that_throws_only_once_should_not_throw_exception()
        {
            int counter = 0;
            Action action = () =>
            {
                if (++counter == 1)
                {
                    throw new Exception();
                }
            };

            const int timeoutInSeconds = 5;

            Action outerAction = () => action.ExecuteWithTimeout(timeoutInSeconds);
            outerAction.ShouldNotThrow();
        }
    }
}
