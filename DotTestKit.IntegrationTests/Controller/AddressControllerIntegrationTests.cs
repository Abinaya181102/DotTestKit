//using AutoFixture;
//using FluentAssertions;
//using Microsoft.AspNetCore.Mvc.Testing;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.DependencyInjection;
//using Newtonsoft.Json;
//using OMSAPI;
//using OMSAPI.DataContext;
//using OMSAPI.Dtos.AddressDtos;
//using OMSAPI.Models;
//using System.Collections.Generic;
//using System.Net;
//using System.Net.Http;
//using System.Net.Http.Json;
//using System.Text;
//using System.Threading.Tasks;
//using Xunit;

//namespace OMSAPI.IntegrationTests.Controllers
//{
//    public class AddressControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
//    {
//        private readonly HttpClient _client;
//        private readonly IFixture _fixture;
//        private readonly WebApplicationFactory<Program> _factory;

//        public AddressControllerIntegrationTests(WebApplicationFactory<Program> factory)
//        {
//            _factory = factory;
//            _client = factory.CreateClient();
//            _fixture = new Fixture();
//        }

//        private async Task<Customer> CreateCustomerAsync()
//        {
//            using var scope = _factory.Services.CreateScope();
//            var dbContext = scope.ServiceProvider.GetRequiredService<OMSDbContext>();

//            // Ensure unique values to avoid PK/constraint errors
//            var customer = _fixture.Build<Customer>()
//                                   .Without(c => c.Id) // Ensure EF Core generates Id
//                                   .Create();

//            dbContext.Customers.Add(customer);
//            await dbContext.SaveChangesAsync();

//            return customer;
//        }

//        [Fact]
//        public async Task CreateAddress_ReturnsCreated()
//        {
//            var customer = await CreateCustomerAsync();
//            var addressDto = _fixture.Build<AddressCreateDto>()
//                .With(a => a.CustomerId, customer.Id)
//                .Create();

//            var response = await _client.PostAsJsonAsync("/api/address", addressDto);
//            response.StatusCode.Should().Be(HttpStatusCode.Created);
//        }

//        [Fact]
//        public async Task Create_ShouldReturnCreatedAddress()
//        {
//            var customer = await CreateCustomerAsync();

//            var addressDto = _fixture.Build<AddressCreateDto>()
//                .With(x => x.CustomerId, customer.Id)
//                .Create();

//            var response = await _client.PostAsJsonAsync("/api/address", addressDto);
//            response.StatusCode.Should().Be(HttpStatusCode.Created);

//            var createdAddress = JsonConvert.DeserializeObject<AddressReadDto>(
//                await response.Content.ReadAsStringAsync());

//            createdAddress.Should().NotBeNull();
//            createdAddress.Street.Should().Be(addressDto.Street);
//            createdAddress.City.Should().Be(addressDto.City);
//            createdAddress.Country.Should().Be(addressDto.Country);
//        }

//        [Fact]
//        public async Task UpdateAddress_ShouldReturnNoContent()
//        {
//            var customer = await CreateCustomerAsync();

//            var createDto = _fixture.Build<AddressCreateDto>()
//                .Create();

//            var createResponse = await _client.PostAsJsonAsync("/api/address", createDto);
//            createResponse.StatusCode.Should().Be(HttpStatusCode.Created);

//            var created = JsonConvert.DeserializeObject<AddressReadFullDto>(
//                await createResponse.Content.ReadAsStringAsync());

//            var updateDto = _fixture.Build<AddressUpdateDto>()
//                .With(x => x.CustomerId, customer.Id)
//                .Create();

//        }

//        [Fact]
//        public async Task DeleteAddress_ShouldReturnNoContent()
//        {
//            var customer = await CreateCustomerAsync();

//            var createDto = _fixture.Build<AddressCreateDto>()
//                .With(x => x.CustomerId, customer.Id)
//                .Create();

//            var createResponse = await _client.PostAsJsonAsync("/api/address", createDto);
//            var created = JsonConvert.DeserializeObject<AddressReadFullDto>(
//                await createResponse.Content.ReadAsStringAsync());

//        }

//        [Fact]
//        public async Task GetAllAddresses_ShouldReturnList()
//        {
//            var response = await _client.GetAsync("/api/address");
//            response.StatusCode.Should().Be(HttpStatusCode.OK);

//            var responseContent = await response.Content.ReadAsStringAsync();
//            var addressList = JsonConvert.DeserializeObject<List<AddressReadDto>>(responseContent);

//            addressList.Should().NotBeNull();
//        }
//    }
//}