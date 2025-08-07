using AutoFixture;
using FluentAssertions;
using OMSAPI.Models;
using Xunit;

namespace OMSAPI.UnitTests.Models
{
    public class UnitOfMeasureTests
    {
        private readonly Fixture _fixture;

        public UnitOfMeasureTests()
        {
            _fixture = new Fixture();
        }

        [Fact]
        public void ShouldCreateUnitOfMeasureWithValidProperties()
        {
            var code = _fixture.Create<string>().Substring(0, 5);
            var name = _fixture.Create<string>();

            var uom = new UnitOfMeasure { Code = code, Name = name };
            uom.Code.Should().Be(code);
            uom.Name.Should().Be(name);
        }

        [Fact]
        public void ShouldSupportPropertyMutation()
        {
            var uom = new UnitOfMeasure { Code = "EA", Name = "Each" };
            var newName = _fixture.Create<string>();

            uom.Name = newName;
            uom.Name.Should().Be(newName);
        }
    }
}