using EmployeeApi.Infra;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using Xunit;

namespace EmployeeApi.Tests
{
    public class ServiceCollectionExtensionsTests
    {
        [Fact]
        public void AddPracticeManagementContextNullConfigThrowArgumentNullException()
        {
            Mock<IConfiguration> configurationSectionStub = new Mock<IConfiguration>();
            IServiceCollection services = new ServiceCollection();
            Assert.Throws<ArgumentNullException>(() => { 
                services.AddDatabaseContext(configurationSectionStub.Object);
                var context = services.BuildServiceProvider().GetService<PracticeManagementContext>();
            });
        }
    }
}
