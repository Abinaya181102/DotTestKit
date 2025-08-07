//using FluentAssertions;
//using Microsoft.AspNetCore.Mvc.Testing;
//using Microsoft.Extensions.Configuration;
//using OMSAPI;
//using OMSAPI.Dtos.AddressDtos;
//using System.Net;
//using System.Net.Http.Json;
//using System.Threading.Tasks;
//using Xunit;

//namespace DotTestKit.IntegrationTests
//{
//    public class UserIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
//    {
//        private readonly HttpClient _client;

//        public UserIntegrationTests(WebApplicationFactory<Program> factory)

//        {

//            _client = factory

//             .WithWebHostBuilder(builder =>

//             {

//                 builder.ConfigureAppConfiguration((context, config) =>

//                 {

//                     config.AddJsonFile("appsettings.json");

//                 });

//             })

//             .CreateClient();

//        }

//        [Fact]

//        public async Task CreateAndGetAddress_ShouldSucceed()

//        {

//            // Arrange - dynamically build the create DTO

//            var createDto = new AddressCreateDto

//            {

//                Country = "India",

//                PostCode = "560001",

//                City = "Bangalore",

//                Street = "MG Road",

//                BuildingNo = "10",

//                AppartmentNo = "501",

//                CustomerId = 1 // Ensure this customer exists or mock accordingly

//            };

//            // Act - POST create

//            var postResponse = await _client.PostAsJsonAsync("/api/address", createDto);

//            // Assert - verify creation

//            postResponse.StatusCode.Should().Be(HttpStatusCode.Created);

//            var createdDto = await postResponse.Content.ReadFromJsonAsync<AddressReadFullDto>();

//            createdDto.Should().NotBeNull();

//            createdDto!.Country.Should().Be(createDto.Country);

//            createdDto.City.Should().Be(createDto.City);

//            // Act - GET the address by ID

//            var getResponse = await _client.GetAsync($"/api/address/{createdDto.Id}");

//            // Assert - verify get

//            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

//            var fetchedDto = await getResponse.Content.ReadFromJsonAsync<AddressReadFullDto>();

//            fetchedDto.Should().NotBeNull();

//            fetchedDto!.Street.Should().Be(createDto.Street);

//        }

//    }

//}

