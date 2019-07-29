using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Ni.Store.Api.Data;
using Ni.Store.Api.Models.Responses;
using Xunit;

namespace Ni.Store.Api.Tests
{
    public partial class StoreControllerTests
    {
        [Fact]
        internal async Task GetAll_Should_Return_Existing_Record()
        {
            // Arrange
            var client = _factory.CreateClient();

            using (var scope = _factory.Server.Host.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<StoreDbContext>();

                var storeList = new List<Data.Entities.Store>
                {
                    new Data.Entities.Store
                    {

                        Key = "TestKey1",
                        Value = "TestValue1",
                        ExpirationTime = DateTime.Now.AddDays(1)
                    },

                    new Data.Entities.Store
                    {
                        Key = "TestKey2",
                        Value = "TestValue2",
                        ExpirationTime = DateTime.Now.AddDays(2)
                    }
                };

                await dbContext.Stores.AddRangeAsync(storeList);
                await dbContext.SaveChangesAsync();
            }

            var requestUri = "/api/keys";

            // Act
            var response = await client.GetAsync(requestUri);

            // Assert

            var responseString = await response.Content.ReadAsStringAsync();
            var resultItemList = JsonConvert.DeserializeObject<List<StoreGetResponse>>(responseString);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotEmpty(responseString);
            Assert.NotNull(resultItemList);
            Assert.True(resultItemList.Count > 1);
            Assert.Equal(1, resultItemList.FirstOrDefault()?.Id);
            Assert.Equal("TestKey1", resultItemList.FirstOrDefault()?.Key);
            Assert.Equal("TestValue1", resultItemList.FirstOrDefault()?.Value);
        }

        [Fact]
        internal async Task GetAll_Should_Return_NotFound_When_Entity_Does_Not_Exist()
        {
            // Arrange
            var client = _factory.CreateClient();

            const string requestUri = "/api/keys";

            // Act
            var response = await client.GetAsync(requestUri);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
