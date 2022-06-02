using Employee.Api.Domain;
using Moq;
using Xunit;

namespace Employee.Api.Tests.Employee.Api
{
    public class GetEmployeeQueryHandlerTests
    {
        [Fact]
        public async void UpdateCustomerCommand_CustomerDataUpdatedOnDatabase()
        {
            var mapper = new Mock<IMapper>();
            var repository = new Mock<IEmployeeRepository>();
            var query = new GetEmployeeQuery("login");
            var handler = new GetEmployeeQueryHandler(repository.Object, mapper.Object);

            await handler.Handle(query, new System.Threading.CancellationToken());

            repository.Verify(x => x.EmployeeByLoginAsync(It.IsAny<string>()), Times.Once);
        }
    }
}
