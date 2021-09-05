using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CourseApplications.Tests
{
    [CollectionDefinition("Integration Tests")]
    // Bellow WebApplicationFactory is using a Startup class. That is the same Startup class used by the main program
    // So all of our service bindings and configuration will be part of our TestHost.
    public class TestCollection: ICollectionFixture<WebApplicationFactory<CourseApplications.Api.Startup>>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}
