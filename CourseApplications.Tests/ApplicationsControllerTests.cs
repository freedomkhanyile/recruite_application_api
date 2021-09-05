using CourseApplications.Api;
using CourseApplications.Api.Models.Applications;
using CourseApplications.DAL.Context;
using CourseApplications.DAL.Enteties;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
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
        private Application _mockApplication;

        public ApplicationsControllerTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            
            // Used by the controller constructor.
            var dbContext = _factory.Services.GetRequiredService<CourseApplicationDbContext>();

            // Seed with required data.

            var course = new Course
            {
                CourseId = 1,
                Name = "Information Technology",
                Faculty = "Science and Mathamatics",
                Department = "Account and Informatics",
                Term = "Y1-S1",
            };

            dbContext.Courses.Add(course);

             _mockApplication = new Application
            {
                ApplicationId = 1,
                FullName = "Tim Carey",
                Address = "Tom baker street",
                Gender = "Male",
                Email = "tim@hotmail.com",
                PhoneNumber = "074421215",
                HighestGradePassed = "Grade 11",
                DateOfBirth = "2001 July 30",
                ApplicationDate = DateTime.Now,
                Status = "Pending",
                Course = course
            };

            dbContext.Applications.Add(_mockApplication);

            var app2 = new Application
            {
                ApplicationId = 2,
                FullName = "Tammy Duncins",
                Address = "New Road Ave",
                Gender = "Female",
                Email = "Tammy@gmail.com.com",
                PhoneNumber = "0741215451",
                HighestGradePassed = "Grade 12",
                DateOfBirth = "2000 September 10",
                ApplicationDate = DateTime.Now,
                Status = "Accepted",
                Course = course
            };

            dbContext.Applications.Add(app2);

            dbContext.SaveChanges();

        }

        [Fact]
        public async Task Get_ShouldReturn_AllApplications()
        {
            // Arrange 
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/api/Applications");
            // Assert
            response.EnsureSuccessStatusCode();
            Assert.NotNull(response.Content);

            var responseObj = JsonSerializer
                .Deserialize<List<ApplicationModel>>(
                await response.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true, });

            Assert.Equal(2, responseObj.Count);
            Assert.Equal(_mockApplication.ApplicationId, responseObj[0].ApplicationId);
            Assert.Equal(_mockApplication.Status, responseObj[0].Status);
            Assert.Equal(_mockApplication.Course.CourseId, responseObj[0].CourseId);
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

            Assert.Equal(_mockApplication.ApplicationId, responseObj.ApplicationId);
            Assert.Equal(_mockApplication.Status, responseObj.Status);
            Assert.Equal(_mockApplication.Course.CourseId, responseObj.CourseId);
        }
    }
}
