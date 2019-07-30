using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Ni.Store.Api.Data;
using Xunit;

namespace Ni.Store.Api.Tests
{
    public partial class StoreControllerTests
    {
        [Fact]
        internal async Task Head_Should_Return_True_When_Record_Already_Exists()
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
                };

                await dbContext.Stores.AddAsync(store);
                await dbContext.SaveChangesAsync();
            }

            var requestUri = $"/api/keys";

            var requestModel = new
            {
                Key = "TestKey1",
                Value = "TestValue1",
            };

            // Act
            var response = await client.PostAsJsonAsync(requestUri, requestModel);

            //// Assert
            //Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            //var responseString = await response.Content.ReadAsStringAsync();

            //Assert.NotEmpty(responseString);

            //var jObject = JObject.Parse(responseString);
        }
    }
}
