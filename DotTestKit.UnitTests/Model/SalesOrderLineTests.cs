
using FluentAssertions;
using OMSAPI.Models;
using Xunit;

namespace OMSAPI.UnitTests.Models
{
    public class SalesOrderLineTests
    {
        [Fact]
        public void TransferFields_CopiesCorrectly()
        {
            var source = new SalesOrderLine
            {
                Quantity = 5,
                Amount = 200,
                ItemId = 1
            };
            var target = new SalesOrderLine();

            target.TransferFields(source);

            target.Quantity.Should().Be(source.Quantity);
            target.Amount.Should().Be(source.Amount);
            target.ItemId.Should().Be(source.ItemId);
        }
    }
}