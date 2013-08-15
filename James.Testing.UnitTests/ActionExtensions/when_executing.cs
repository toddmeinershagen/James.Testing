using System;
using System.Diagnostics;
using FluentAssertions;
using NUnit.Framework;

namespace James.Testing.UnitTests.ActionExtensions
{
    [TestFixture]
    public class when_executing : BaseTest
    {
        [Test]
        public void given_an_action_that_does_not_throw_should_complete_action()
        {
            var counter = 0;
            Action action = () => counter++;

            action.Execute();

            counter.Should().Be(1);
        }

        [Test]
        public void given_an_action_that_does_not_throw_should_not_throw_an_exception()
        {
            var counter = 0;
            Action action = () => counter++;

            Action outerAction = () => action.Execute();

            outerAction.ShouldNotThrow();
        }

        [Test]
        public void given_an_action_that_does_not_throw_and_specifying_a_number_of_times_should_complete_action()
        {
            var counter = 0;
            const int times = 5;
            Action action = () => counter++;

            action.Execute(times);

            counter.Should().Be(1);
        }

        [Test]
        public void given_an_action_that_does_not_throw_and_specifying_a_number_of_times_should_not_throw()
        {
            var counter = 0;
            const int times = 5;
            Action action = () => counter++;

            Action outerAction = () => action.Execute(times);

            outerAction.ShouldNotThrow();
        }

        [Test]
        public void given_an_action_that_always_throws_and_specifying_a_number_of_times_should_complete_action_the_same_number_of_times()
        {
            var counter = 0;
            const int times = 5;
            Action action = () =>
            {
                counter++;
                throw new Exception();
            };

            Gulp(() => action.Execute(times));

            counter.Should().Be(5);
        }

        [Test]
        public void given_an_action_that_always_throws_and_specifying_a_number_of_times_with_a_given_wait_time_should_complete_action_in_multiple_of_wait_time()
        {
            var counter = 0;
            const int times = 5;
            const int waitTimeInSeconds = 2;
            const int multipleOfWaitTime = waitTimeInSeconds * (times);

            Action action = () =>
            {
                counter++;
                throw new Exception();
            };

            var watch = new Stopwatch();
            watch.Start();
            Gulp(() => action.Execute(times, waitTimeInSeconds));
            watch.Stop();

            watch.Elapsed.TotalSeconds.Should().BeGreaterOrEqualTo(multipleOfWaitTime);
        }

        [Test]
        public void given_an_action_that_always_throws_and_specifying_a_number_of_times_with_a_given_wait_time_should_bubble_up_exception()
        {
            var counter = 0;
            const int times = 5;

            Action action = () =>
            {
                counter++;
                throw new ArgumentException();
            };

            Action outerAction = () => action.Execute(times);

            outerAction.ShouldThrow<ArgumentException>();
        }

        [Test]
        public void given_an_action_that_throws_only_once_and_specifying_a_number_of_times_should_complete_action_twice()
        {
            var counter = 0;
            const int times = 5;
            Action action = () =>
            {
                counter++;

                if (counter == 1)
                    throw new Exception();
            };

            action.Execute(times);

            counter.Should().Be(2);
        }
    }
}
