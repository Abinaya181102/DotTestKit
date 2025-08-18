

//using FluentAssertions;
//using OMSAPI.DataContext;
//using OMSAPI.Models;
//using OMSAPI.Services;
//using OMSAPI.UnitTests.TestHelpers;
//using System.Linq;
//using Xunit;

//namespace OMSAPI.UnitTests.Services
//{
//    public class UnitOfMeasureServiceTests
//    {
//        private readonly UnitOfMeasureService _service;
//        private readonly OMSDbContext _context;

//        public UnitOfMeasureServiceTests()
//        {
//            _context = TestDbContextFactory.CreateInMemoryDbContext();
//            _service = new UnitOfMeasureService(_context);
//        }

//        [Fact]
//        public void Create_ShouldAddUom()
//        {
//            var uom = new UnitOfMeasure { Code = "PCS", Name = "Piece" };
//            _service.Create(uom);
//            _service.SaveChanges();
//            _context.UnitsOfMeasure.Should().Contain(uom);
//        }

//        [Fact]
//        public void Get_ShouldReturnUom()
//        {
//            var uom = new UnitOfMeasure { Code = "KG", Name = "Kilogram" };
//            _context.UnitsOfMeasure.Add(uom);
//            _context.SaveChanges();
//            var result = _service.Get(uom.Code);
//            result.Should().BeEquivalentTo(uom);
//        }

//        [Fact]
//        public void Delete_ShouldRemoveUom()
//        {
//            var uom = new UnitOfMeasure { Code = "L", Name = "Liter" };
//            _context.UnitsOfMeasure.Add(uom);
//            _context.SaveChanges();
//            _service.Delete(uom);
//            _service.SaveChanges();
//            _context.UnitsOfMeasure.Should().NotContain(uom);
//        }

//        [Fact]
//        public void GetAll_ShouldReturnAll()
//        {
//            _context.UnitsOfMeasure.Add(new UnitOfMeasure { Code = "G", Name = "Gram" });
//            _context.UnitsOfMeasure.Add(new UnitOfMeasure { Code = "M", Name = "Meter" });
//            _context.SaveChanges();
//            var result = _service.GetAll();
//            result.Count().Should().BeGreaterThan(1);
//        }

//        [Fact]
//        public void Update_ShouldModifyEntity()
//        {
//            var uom = new UnitOfMeasure { Code = "BX", Name = "Box" };
//            _context.UnitsOfMeasure.Add(uom);
//            _context.SaveChanges();

//            uom.Name = "Updated Box";
//            _service.Update(uom);
//            _service.SaveChanges();

//            _context.UnitsOfMeasure.First().Name.Should().Be("Updated Box");
//        }
//    }
//}



using FluentAssertions;
using OMSAPI.DataContext;
using OMSAPI.Models;
using OMSAPI.Services;
using OMSAPI.UnitTests.TestHelpers;
using System;
using System.Linq;
using Xunit;

namespace OMSAPI.UnitTests.Services
{
    public class UnitOfMeasureServiceTests
    {
        private readonly UnitOfMeasureService _service;
        private readonly OMSDbContext _context;

        public UnitOfMeasureServiceTests()
        {
            _context = TestDbContextFactory.CreateInMemoryDbContext();
            _service = new UnitOfMeasureService(_context);
        }

        private UnitOfMeasure CreateTestUom(string code = null, string name = null)
        {
            return new UnitOfMeasure
            {
                Code = code ?? Guid.NewGuid().ToString("N").Substring(0, 4).ToUpper(),
                Name = name ?? $"Name_{Guid.NewGuid():N}".Substring(0, 8)
            };
        }

        [Fact]
        public void Create_ShouldAddUom()
        {
            var uom = CreateTestUom();
            _service.Create(uom);
            _service.SaveChanges();

            _context.UnitsOfMeasure.Should().ContainSingle(u => u.Code == uom.Code && u.Name == uom.Name);
        }

        [Fact]
        public void Create_ShouldThrow_WhenNull()
        {
            Action act = () => _service.Create(null);
            act.Should().Throw<ArgumentNullException>().WithParameterName("unitOfMeasure");
        }

        [Fact]
        public void Get_ShouldReturnUom_WhenExists()
        {
            var uom = CreateTestUom();
            _context.UnitsOfMeasure.Add(uom);
            _context.SaveChanges();

            var result = _service.Get(uom.Code);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(uom);
        }

        [Fact]
        public void Get_ShouldReturnNull_WhenNotExists()
        {
            var result = _service.Get("NON_EXISTENT_CODE");
            result.Should().BeNull();
        }

        [Fact]
        public void Delete_ShouldRemoveUom()
        {
            var uom = CreateTestUom();
            _context.UnitsOfMeasure.Add(uom);
            _context.SaveChanges();

            _service.Delete(uom);
            _service.SaveChanges();

            _context.UnitsOfMeasure.Should().NotContain(u => u.Code == uom.Code);
        }

        [Fact]
        public void Delete_ShouldThrow_WhenNull()
        {
            Action act = () => _service.Delete(null);
            act.Should().Throw<ArgumentNullException>().WithParameterName("unitOfMeasure");
        }

        [Fact]
        public void GetAll_ShouldReturnAll()
        {
            var uom1 = CreateTestUom();
            var uom2 = CreateTestUom();

            _context.UnitsOfMeasure.AddRange(uom1, uom2);
            _context.SaveChanges();

            var result = _service.GetAll();

            result.Should().Contain(u => u.Code == uom1.Code)
                   .And.Contain(u => u.Code == uom2.Code);
        }

        [Fact]
        public void GetAll_ShouldReturnEmpty_WhenNoneExists()
        {
            var result = _service.GetAll();
            result.Should().BeEmpty();
        }

        [Fact]
        public void Update_ShouldModifyEntity()
        {
            var uom = CreateTestUom();
            _context.UnitsOfMeasure.Add(uom);
            _context.SaveChanges();

            var updatedName = "Updated_" + uom.Name;
            uom.Name = updatedName;

            _service.Update(uom);
            _service.SaveChanges();

            _context.UnitsOfMeasure.First().Name.Should().Be(updatedName);
        }

        [Fact]
        public void SaveChanges_ShouldReturnTrue_WhenChangesExist()
        {
            var uom = CreateTestUom();
            _context.UnitsOfMeasure.Add(uom);

            var result = _service.SaveChanges();

            result.Should().BeTrue();
        }
    }
}

