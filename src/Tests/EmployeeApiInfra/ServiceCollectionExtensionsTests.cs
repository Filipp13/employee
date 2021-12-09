using Employee.Api.Infra;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using Xunit;

namespace Employee.Api.Tests
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
