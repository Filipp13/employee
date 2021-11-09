using EmployeeApi.Domain;
using Moq;
using Xunit;

namespace EmployeeApi.Tests.EmployeeApi
{
    public class GetEmployeeQueryHandlerTests
    {
        [Fact]
        public async void UpdateCustomerCommand_CustomerDataUpdatedOnDatabase()
        {
            var repository = new Mock<IEmployeeRepository>();
            var query = new GetEmployeeQuery("login");
            var handler = new GetEmployeeQueryHandler(repository.Object);

            await handler.Handle(query, new System.Threading.CancellationToken());

            repository.Verify(x => x.EmployeeByLoginAsync(It.IsAny<string>()), Times.Once);
        }
    }
}
