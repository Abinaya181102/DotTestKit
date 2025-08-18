using AutoFixture;
using FluentAssertions;
using OMSAPI.Models;
using Xunit;

namespace OMSAPI.UnitTests.Models
{
    public class SalesOrderHeaderTests
    {
        private readonly Fixture _fixture;

        public SalesOrderHeaderTests()
        {
            _fixture = new Fixture();

            // Handle circular references
            _fixture.Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));

            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [Fact]
        public void TransferFields_ShouldCopyRelevantFields()
        {
            var source = _fixture.Build<SalesOrderHeader>()
                .With(h => h.OrderDate, System.DateTime.Now.AddDays(1))
                .With(h => h.ShipmentDate, System.DateTime.Now.AddDays(2))
                .With(h => h.CustomerId, 100)
                .With(h => h.AddressId, 200)
                .Without(h => h.Lines) // Optional: exclude navigation property for clarity
                .Create();

            var target = new SalesOrderHeader();
            target.TransferFields(source);

            target.OrderDate.Should().Be(source.OrderDate);
            target.ShipmentDate.Should().Be(source.ShipmentDate);
            target.CustomerId.Should().Be(source.CustomerId);
            target.AddressId.Should().Be(source.AddressId);
        }
    }
}
