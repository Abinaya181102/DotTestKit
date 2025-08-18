
//using AutoFixture;
//using FluentAssertions;
//using Microsoft.EntityFrameworkCore;
//using OMSAPI.DataContext;
//using OMSAPI.Models;
//using OMSAPI.Services;
//using OMSAPI.UnitTests.TestHelpers;
//using System;
//using System.Linq;
//using Xunit;

//namespace OMSAPI.UnitTests.Services
//{
//    public class AddressServiceTests
//    {
//        private readonly AddressService _service;
//        private readonly Fixture _fixture;
//        private readonly OMSDbContext _context;

//        public AddressServiceTests()
//        {
//            _context = TestDbContextFactory.CreateInMemoryDbContext();
//            _service = new AddressService(_context);
//            _fixture = new Fixture();
//        }

//        [Fact]
//        public void Create_AddsAddressToContext()
//        {
//            var address = _fixture.Create<Address>();
//            _service.Create(address);
//            _service.SaveChanges();

//            _context.Addresses.Should().Contain(address);
//        }

//        [Fact]
//        public void GetAll_ReturnsAllAddresses()
//        {
//            var address = _fixture.Create<Address>();
//            _context.Addresses.Add(address);
//            _context.SaveChanges();

//            var result = _service.GetAll();
//            result.Should().Contain(address);
//        }

//        [Fact]
//        public void Get_ReturnsCorrectAddress()
//        {
//            var address = _fixture.Create<Address>();
//            _context.Addresses.Add(address);
//            _context.SaveChanges();

//            var result = _service.Get(address.Id);
//            result.Should().BeEquivalentTo(address);
//        }

//        [Fact]
//        public void Delete_RemovesAddress()
//        {
//            var address = _fixture.Create<Address>();
//            _context.Addresses.Add(address);
//            _context.SaveChanges();

//            _service.Delete(address);
//            _service.SaveChanges();

//            _context.Addresses.Should().NotContain(address);
//        }
//    }
//}


using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using OMSAPI.DataContext;
using OMSAPI.Models;
using OMSAPI.Services;
using OMSAPI.UnitTests.TestHelpers;
using System;
using System.Linq;
using Xunit;

namespace OMSAPI.UnitTests.Services
{
    public class AddressServiceTests
    {
        private readonly OMSDbContext _context;
        private readonly AddressService _service;
        private readonly Fixture _fixture;

        public AddressServiceTests()
        {
            _context = TestDbContextFactory.CreateInMemoryDbContext();
            _service = new AddressService(_context);
            _fixture = new Fixture();
        }

        [Fact]
        public void Create_AddsAddressToContext()
        {
            var address = _fixture.Create<Address>();

            _service.Create(address);
            _service.SaveChanges();

            _context.Addresses.Should().ContainEquivalentOf(address);
        }

        [Fact]
        public void Create_ThrowsArgumentNullException_WhenAddressIsNull()
        {
            Action act = () => _service.Create(null!);

            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Get_ReturnsCorrectAddress_WhenAddressExists()
        {
            var address = _fixture.Create<Address>();
            _context.Addresses.Add(address);
            _context.SaveChanges();

            var result = _service.Get(address.Id);

            result.Should().BeEquivalentTo(address);
        }

        [Fact]
        public void Get_ReturnsNull_WhenAddressDoesNotExist()
        {
            var result = _service.Get(-1);
            result.Should().BeNull();
        }

        [Fact]
        public void GetAll_ReturnsAllAddresses()
        {
            var addresses = _fixture.CreateMany<Address>(3).ToList();
            _context.Addresses.AddRange(addresses);
            _context.SaveChanges();

            var result = _service.GetAll();

            result.Should().BeEquivalentTo(addresses);
        }

        [Fact]
        public void GetAllForCustomer_ReturnsCorrectAddresses()
        {
            var customerId = 1;
            var matching = _fixture.Build<Address>().With(a => a.CustomerId, customerId).CreateMany(2).ToList();
            var others = _fixture.Build<Address>().With(a => a.CustomerId, customerId + 1).CreateMany(2).ToList();

            _context.Addresses.AddRange(matching);
            _context.Addresses.AddRange(others);
            _context.SaveChanges();

            var result = _service.GetAllForCustomer(customerId);

            result.Should().BeEquivalentTo(matching);
        }

        [Fact]
        public void Delete_RemovesAddress_WhenAddressExists()
        {
            var address = _fixture.Create<Address>();
            _context.Addresses.Add(address);
            _context.SaveChanges();

            _service.Delete(address);
            _service.SaveChanges();

            _context.Addresses.Should().NotContain(address);
        }

        [Fact]
        public void Delete_ThrowsArgumentNullException_WhenAddressIsNull()
        {
            Action act = () => _service.Delete(null!);

            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Update_ChangesEntityStateToModified()
        {
            var address = _fixture.Create<Address>();
            _context.Addresses.Add(address);
            _context.SaveChanges();

            _service.Update(address);
            _context.Entry(address).State.Should().Be(EntityState.Modified);
        }

        [Fact]
        public void SaveChanges_ReturnsTrue_WhenChangesSaved()
        {
            var address = _fixture.Create<Address>();
            _context.Addresses.Add(address);

            var result = _service.SaveChanges();

            result.Should().BeTrue();
        }
    }
}
