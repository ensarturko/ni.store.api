using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Ni.Store.Api.Data;
using Xunit;

namespace Ni.Store.Api.Tests
{
    public partial class StoreControllerTests
    {
        [Fact]
        internal async Task Delete_Should_Return_NoContent_Record()
        {
            // Arrange
            var client = _factory.CreateClient();

            Data.Entities.Store store;

            using (var scope = _factory.Server.Host.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<StoreDbContext>();

                store = new Data.Entities.Store
                {
                    Key = "TestKey1",
                    Value = "TestValue1",
                    ExpirationTime = DateTime.Now.AddDays(1)
                };

                await dbContext.Stores.AddAsync(store);
                await dbContext.SaveChangesAsync();
            }

            var requestUri = $"/api/keys/{store.Id}";

            // Act
            var response = await client.DeleteAsync(requestUri);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        internal async Task Delete_Should_Return_NotFound_Record()
        {
            // Arrange
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Add("UserId", "1");
            var requestUri = $"/api/keys/999";

            // Act
            var response = await client.DeleteAsync(requestUri);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        internal async Task Delete_Should_Return_Error_When_Id_Not_Greater_Than_Zero()
        {
            // Arrange
            var client = _factory.CreateClient();
            var requestUri = "/api/keys/0";

            // Act
            var response = await client.DeleteAsync(requestUri);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var responseModel = await response.Content.ReadAsAsync<string[]>();
            Assert.Single(responseModel);
            Assert.Equal("Id must be greater than zero.", responseModel[0]);
        }
    }
}
