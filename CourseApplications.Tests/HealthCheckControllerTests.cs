using CourseApplications.Api;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace CourseApplications.Tests
{
    [Collection("Integration Tests")]

    public class HealthCheckControllerTests
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public HealthCheckControllerTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Health_Check_ReturnsGoodResult()
        {
            // Arrange

            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/health");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.NotNull(response.Content);

            var responseObj = JsonSerializer
           .Deserialize<ResponseType>(
            await response.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true, });

            Assert.Equal("Healthy", responseObj?.Status);

        }

        private class ResponseType
        {
            public string Status { get; set; }
        }
    }
}
