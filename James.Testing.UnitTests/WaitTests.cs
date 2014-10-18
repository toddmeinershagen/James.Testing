using System;
using System.Diagnostics;
using FluentAssertions;
using NUnit.Framework;

namespace James.Testing.UnitTests
{
    [TestFixture]
    public class WaitTests
    {
        [TestCase(0)]
        [TestCase(250)]
        [TestCase(500)]
        public void given_positive_number_when_waiting_for_milliseconds_should_wait_for_seconds(int expected)
        {
            var watch = new Stopwatch();
            watch.Start();

            Wait.For(expected).Milliseconds();

            watch.Stop();
            watch.Elapsed.Should().BeCloseTo(TimeSpan.FromMilliseconds(expected));
        }

        [TestCase(0)]
        [TestCase(2)]
        public void given_positive_number_when_waiting_for_seconds_should_wait_for_seconds(int expected)
        {
            var watch = new Stopwatch();
            watch.Start();

            Wait.For(expected).Seconds();

            watch.Stop();
            watch.Elapsed.Should().BeCloseTo(TimeSpan.FromSeconds(expected));
        }

        [Test]
        public void given_negative_number_when_waiting_for_seconds_should_throw()
        {
            Action action = () => Wait.For(-1).Seconds();
            action.ShouldThrow<ArgumentOutOfRangeException>().WithMessage("Number must be non-negative.\r\nParameter name: numberOf");
        }

        [Test]
        public void given_predicate_when_waiting_until_predicate_is_true_should_wait_until_true()
        {
            int counter = 0;
            Wait.Until(() => ++counter == 5);

            counter.Should().Be(5);
        }

        [Test]
        public void given_predicate_never_returns_true_when_waiting_until_predicate_is_true_should_wait_until_default_timeout()
        {
            var defaultTimeout = TimeSpan.FromSeconds(15);

            var watch = new Stopwatch();
            watch.Start();

            Wait.Until(() => false);

            watch.Stop();
            watch.Elapsed.Should().BeCloseTo(defaultTimeout);
        }

        [Test]
        public void given_predicate_never_returns_true_and_timeout_as_timespan_when_waiting_until_predicate_is_true_should_wait_until_timeout()
        {
            var timeout = TimeSpan.FromMilliseconds(5);

            var watch = new Stopwatch();
            watch.Start();

            Wait.Until(() => false, timeout);

            watch.Stop();
            watch.Elapsed.Should().BeCloseTo(timeout);
        }

        [Test]
        public void given_null_predicate_and_timeout_as_timespan_when_waiting_until_predicate_is_true_should_wait_until_timeout()
        {
            var timeout = TimeSpan.FromMilliseconds(5);

            var watch = new Stopwatch();
            watch.Start();

            Wait.Until(null, timeout);

            watch.Stop();
            watch.Elapsed.Should().BeCloseTo(timeout);
        }

        [Test]
        public void given_predicate_never_returns_true_and_timeout_in_seconds_when_waiting_until_predicate_is_true_should_wait_until_timeout()
        {
            const int timeoutInSeconds = 7;

            var watch = new Stopwatch();
            watch.Start();

            Wait.Until(() => false, timeoutInSeconds);

            watch.Stop();
            watch.Elapsed.Should().BeCloseTo(TimeSpan.FromSeconds(timeoutInSeconds));
        }

        [Test]
        public void given_null_predicate_and_timeout_in_timespan_when_waiting_until_predicate_is_true_should_wait_until_timeout()
        {
            const int timeoutInSeconds = 2;

            var watch = new Stopwatch();
            watch.Start();

            Wait.Until(null, timeoutInSeconds);

            watch.Stop();
            watch.Elapsed.Should().BeCloseTo(TimeSpan.FromSeconds(timeoutInSeconds));
        }
    }
}
