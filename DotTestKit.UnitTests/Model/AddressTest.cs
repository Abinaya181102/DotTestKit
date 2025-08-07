
using AutoFixture;
using FluentAssertions;
using OMSAPI.Models;
using Xunit;

namespace OMSAPI.UnitTests.Models
{
    public class AddressTests
    {
        private readonly Fixture _fixture;

        public AddressTests()
        {
            _fixture = new Fixture();
        }

        [Fact]
        public void TransferFields_CopiesAllFields()
        {
            var source = _fixture.Create<Address>();
            var target = new Address();

            target.TransferFields(source);

            target.Should().BeEquivalentTo(source, options => options.Excluding(a => a.Id));
        }
    }
}