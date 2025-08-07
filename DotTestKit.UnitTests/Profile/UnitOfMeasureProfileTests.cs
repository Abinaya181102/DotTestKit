using AutoMapper;
using AutoFixture;
using FluentAssertions;
using OMSAPI.Dtos.UnitOfMeasureDtos;
using OMSAPI.Models;
using OMSAPI.Profiles;
using Xunit;

namespace OMSAPI.UnitTests.Profiles
{
    public class UnitOfMeasureProfileTests
    {
        private readonly IMapper _mapper;
        private readonly Fixture _fixture;

        public UnitOfMeasureProfileTests()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<UnitOfMeasureProfile>());
            _mapper = config.CreateMapper();
            _fixture = new Fixture();
        }

        [Fact]
        public void ShouldMapCreateDtoToModel()
        {
            var dto = _fixture.Create<UnitOfMeasureCreateDto>();
            var model = _mapper.Map<UnitOfMeasure>(dto);

            model.Code.Should().Be(dto.Code);
            model.Name.Should().Be(dto.Name);
        }

        [Fact]
        public void ShouldMapModelToReadDto()
        {
            var model = _fixture.Create<UnitOfMeasure>();
            var dto = _mapper.Map<UnitOfMeasureReadDto>(model);

            dto.Code.Should().Be(model.Code);
            dto.Name.Should().Be(model.Name);
        }

        [Fact]
        public void ShouldMapModelToFullReadDto()
        {
            var model = _fixture.Create<UnitOfMeasure>();
            var dto = _mapper.Map<UnitOfMeasureReadFullDto>(model);

            dto.Code.Should().Be(model.Code);
            dto.Name.Should().Be(model.Name);
        }

        [Fact]
        public void ShouldMapUpdateDtoToModel()
        {
            var dto = _fixture.Create<UnitOfMeasureUpdateDto>();
            var model = _fixture.Create<UnitOfMeasure>();

            _mapper.Map(dto, model);
            model.Name.Should().Be(dto.Name);
        }
    }
}
