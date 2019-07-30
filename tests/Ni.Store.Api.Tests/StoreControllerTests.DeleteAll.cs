using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Ni.Store.Api.Data;
using Xunit;

namespace Ni.Store.Api.Tests
{
    public partial class StoreControllerTests
    {
        [Fact]
        internal async Task DeleteAll_Should_Return_NoContent_Record_After_Deleting_Data()
        {
            // Arrange
            var client = _factory.CreateClient();

            Data.Entities.Store store;

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
            var response = await client.DeleteAsync(requestUri);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        internal async Task DeleteAll_Should_Return_NoContent_Record_When_NoRecords_Exists()
        {
            // Arrange
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Add("UserId", "1");
            var requestUri = "/api/keys";

            // Act
            var response = await client.DeleteAsync(requestUri);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }
    }
}
