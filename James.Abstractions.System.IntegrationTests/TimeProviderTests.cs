using System;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace James.Abstractions.System.IntegrationTests
{
    [TestFixture]
    public class TimeProviderTests
    {
        [SetUp]
        public void SetUp()
        {
            TimeProvider.ResetToDefault();
        }

        [Test]
        public void given_initial_provider_when_getting_the_current_instance_should_return_default_provider()
        {
            TimeProvider.Current.Should().BeOfType<DefaultTimeProvider>();
        }

        [Test]
        public void given_initial_provider_when_getting_now_as_datetime_should_return_datetime_now()
        {
            TimeProvider.Current.NowAsDateTime.Should().BeCloseTo(DateTime.Now);
        }

        [Test]
        public void given_initial_provider_when_getting_now_as_datetimeoffset_should_return_datetimeoffset_now()
        {
            TimeProvider.Current.NowAsDateTimeOffset.Should().BeCloseTo(DateTimeOffset.Now);
        }
    }

    [TestFixture]
    public class given_provider_has_been_set_to_mock_provider
    {
        private TimeProvider _mockProvider;

        [SetUp]
        public void SetUp()
        {
            _mockProvider = Substitute.For<TimeProvider>();
            TimeProvider.Current = _mockProvider;
        }

        [Test]
        public void when_getting_the_current_instance_should_return_the_mock_provider()
        {
            TimeProvider.Current.Should().Be(_mockProvider);
        }

        [Test]
        public void when_getting_now_as_datetime_should_return_the_mock_provider_now_as_datetime()
        {
            var expectedDate = 20.July(2015);
            _mockProvider.NowAsDateTime.Returns(expectedDate);

            TimeProvider.Current.NowAsDateTime.Should().Be(expectedDate);
        }

        [Test]
        public void when_getting_now_as_datetimeoffset_should_return_the_mock_provider_now_as_datetimeoffset()
        {
            var expectedDate = new DateTimeOffset(20.July(2015));
            _mockProvider.NowAsDateTimeOffset.Returns(expectedDate);

            TimeProvider.Current.NowAsDateTimeOffset.Should().Be(expectedDate);
        }

        [Test]
        public void and_given_provider_reset_when_getting_the_current_instance_should_return_the_default_time_provider()
        {
            TimeProvider.ResetToDefault();
            TimeProvider.Current.Should().BeOfType<DefaultTimeProvider>();
        }
    }
}
