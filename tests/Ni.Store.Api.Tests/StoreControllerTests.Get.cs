﻿using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using Ni.Store.Api.Data;
using Xunit;

namespace Ni.Store.Api.Tests
{
    public partial class StoreControllerTests 
    {
        [Fact]
        internal async Task Get_Should_Return_Existing_Record()
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
            var response = await client.GetAsync(requestUri);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var responseString = await response.Content.ReadAsStringAsync();

            Assert.NotEmpty(responseString);

            var jObject = JObject.Parse(responseString);

            Assert.Equal(4, jObject.Count);

            Assert.True(jObject.ContainsKey("id"));
            Assert.NotEqual(0, jObject["id"].Value<int>());

            Assert.True(jObject.ContainsKey("key"));
            Assert.Equal("TestKey1", jObject["key"].Value<string>());

            Assert.True(jObject.ContainsKey("value"));
            Assert.Equal("TestValue1", jObject["value"].Value<string>());

            Assert.True(jObject.ContainsKey("expirationTime"));
        }

        [Fact]
        internal async Task Get_Should_Return_BadRequest_When_Entity_Does_Not_Exist()
        {
            // Arrange
            var client = _factory.CreateClient();

            const string requestUri = "/api/keys/999";

            // Act
            var response = await client.GetAsync(requestUri);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
