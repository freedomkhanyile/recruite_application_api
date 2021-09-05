using CourseApplications.Api;
using CourseApplications.Api.Models.Applications;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace CourseApplications.Tests
{
    [Collection("Integration Tests")]
    public class ApplicationsControllerTests
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public ApplicationsControllerTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GetById_ShoulReturn_ApplicationById()
        {
            // Arrange 
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/api/Applications/1");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.NotNull(response.Content);

            var responseObj = JsonSerializer
                .Deserialize<ApplicationModel>(
                await response.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true, });

            Assert.Equal(1, responseObj.ApplicationId);
            Assert.Equal("Pending", responseObj.Status);
            Assert.Equal(1, responseObj.CourseId);
        }
    }
}
