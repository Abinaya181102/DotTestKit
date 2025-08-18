using AutoFixture;
using FluentAssertions;
using OMSAPI.Models;
using Xunit;

namespace OMSAPI.UnitTests.Models
{
    public class CustomerTests
    {
        private readonly Fixture _fixture;

        public CustomerTests()
        {
            _fixture = new Fixture();
        }

        [Fact]
        public void Customer_ShouldHaveNameAndAddressList()
        {
            var customer = _fixture.Create<Customer>();
            customer.Name.Should().NotBeNullOrWhiteSpace();
            customer.Addresses.Should().NotBeNull();
        }
    }
}