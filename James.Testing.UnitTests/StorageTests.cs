using FluentAssertions;
using James.Testing.Rest;
using NUnit.Framework;

namespace James.Testing.UnitTests
{
    [TestFixture]
    public class StorageTests
    {
        [Test]
        public void
            given_object_and_output_parameter_and_expression_when_storing_should_take_value_from_expression_and_store_in_output_parameter
            ()
        {
            var value = new Person { FirstName = "Todd", LastName = "Meinershagen" };
            string output;

            value.Store(out output, x => x.LastName);

            output.Should().Be(value.LastName);
        }
    }
}
