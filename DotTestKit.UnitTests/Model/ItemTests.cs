using FluentAssertions;
using OMSAPI.Models;
using Xunit;

namespace OMSAPI.UnitTests.Models
{
    public class ItemTests
    {
        [Fact]
        public void TransferFields_ShouldCopyAllValues()
        {
            var source = new Item
            {
                Name = "Item",
                Description = "Desc",
                UnitPrice = 10,
                UnitCost = 5,
                UnitOfMeasureCode = "PCS"
            };

            var destination = new Item();
            destination.TransferFields(source);

            destination.Should().BeEquivalentTo(source, opt => opt.Excluding(x => x.Id));
        }
    }
}