using System;
using System.IO;
using System.Text;
using FluentAssertions;
using NUnit.Framework;

namespace James.Testing.UnitTests
{
    [TestFixture]
    public class DebuggingTests
    {
        [Test]
        public void given_value_and_function_for_getting_an_object_when_printing_to_debug_should_write_property_value_to_console()
        {
            var builder = new StringBuilder();
            using (var writer = new StringWriter(builder))
            {
                Console.SetOut(writer);
                var value = new Person {FirstName = "Todd", LastName = "Meinershagen"};

                var result = value.DebugPrint(x => x.LastName);
                result.Should().Be(value);
            }

            builder.ToString().Should().Be("Meinershagen\r\n");
        }

        [Test]
        public void given_value_when_printing_to_debug_should_write_value_to_console()
        {
            var builder = new StringBuilder();
            using (var writer = new StringWriter(builder))
            {
                Console.SetOut(writer);
                var value = new Person { FirstName = "Todd", LastName = "Meinershagen" };

                var result = value.DebugPrint();
                result.Should().Be(value);
            }

            builder.ToString().Should().Be("James.Testing.UnitTests.Person\r\n");
        }

        [Test]
        public void given_value_and_message_when_printing_to_debug_should_write_message_to_console()
        {
            var builder = new StringBuilder();
            using (var writer = new StringWriter(builder))
            {
                Console.SetOut(writer);
                var value = new Person { FirstName = "Todd", LastName = "Meinershagen" };

                var result = value.DebugPrint("This is a message.");
                result.Should().Be(value);
            }

            builder.ToString().Should().Be("This is a message.\r\n");
        }

    }
}
