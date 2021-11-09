using EmployeeApi.Controllers;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace EmployeeApi.Tests.EmployeeApi
{
    public class EmployeeControllerTests
    {
        [Fact]
        public async void GetUserInfoAsyncMediatorCallOnce()
        {
            var mockMediator = new Mock<IMediator>();

            mockMediator
                .Setup(m => m.Send(It.IsAny<GetEmployeeQuery>(), default(CancellationToken)));
                //.Verifiable("Notification was not sent.")
                //.Returns()

            var controller = new EmployeeController(mockMediator.Object);

            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext { 
                User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>() {
                new Claim(ClaimTypes.Name, "Filipp") })) };

            await controller.GetUserInfoAsync();

            mockMediator.Verify(x => x.Send(It.IsAny<GetEmployeeQuery>(), default(CancellationToken)), Times.Once());
        }

        [Fact]
        public async void GetUserInfoAsyncAbsentIdentityThrowException()
        {
            var mockMediator = new Mock<IMediator>();

            mockMediator
                .Setup(m => m.Send(It.IsAny<GetEmployeeQuery>(), default(CancellationToken)));

            var controller = new EmployeeController(mockMediator.Object);

            var ex = await Assert.ThrowsAsync<NullReferenceException>(async () =>
                await controller.GetUserInfoAsync());

            Assert.Equal("User identity is absent, claim: Name", ex.Message);
        }


        [Fact]
        public async void GetUserInfoByLoginAsyncAsyncMediatorCallOnce()
        {
            var mockMediator = new Mock<IMediator>();

            mockMediator
                .Setup(m => m.Send(It.IsAny<GetEmployeeQuery>(), default(CancellationToken)));

            var controller = new EmployeeController(mockMediator.Object);

            await controller.GetEmployeeByLoginAsync("login");

            mockMediator.Verify(x => x.Send(It.IsAny<GetEmployeeQuery>(), default(CancellationToken)), Times.Once());
        }

        [Fact]
        public async void IsAdminAsyncMediatorCallOnce()
        {
            var mockMediator = new Mock<IMediator>();

            mockMediator
                .Setup(m => m.Send(It.IsAny<IsAdminQuery>(), default(CancellationToken)));

            var controller = new EmployeeController(mockMediator.Object);

            await controller.IsAdminAsync("login");

            mockMediator.Verify(x => x.Send(It.IsAny<IsAdminQuery>(), default(CancellationToken)), Times.Once());
        }

        [Fact]
        public async void IsRiskManagementAsyncMediatorCallOnce()
        {
            var mockMediator = new Mock<IMediator>();

            mockMediator
                .Setup(m => m.Send(It.IsAny<IsRiskManagementQuery>(), default(CancellationToken)));

            var controller = new EmployeeController(mockMediator.Object);

            await controller.IsRiskManagementAsync("login");

            mockMediator.Verify(x => x.Send(It.IsAny<IsRiskManagementQuery>(), default(CancellationToken)), Times.Once());
        }

        [Fact]
        public async void GetEmployeesAsyncMediatorCallOnce()
        {
            var mockMediator = new Mock<IMediator>();

            mockMediator
                .Setup(m => m.Send(It.IsAny<GetEmployeesQuery>(), default(CancellationToken)));

            var controller = new EmployeeController(mockMediator.Object);

            await controller.GetEmployeesAsync("search");

            mockMediator.Verify(x => x.Send(It.IsAny<GetEmployeesQuery>(), default(CancellationToken)), Times.Once());
        }

        [Fact]
        public async void GetEmployeesForRoleAsyncMediatorCallOnce()
        {
            var mockMediator = new Mock<IMediator>();

            mockMediator
                .Setup(m => m.Send(It.IsAny<GetEmployeesForRoleQuery>(), default(CancellationToken)));

            var controller = new EmployeeController(mockMediator.Object);

            await controller.GetEmployeesForRoleAsync(
                "search",
                "role",
                default,
                "");

            mockMediator.Verify(x => x.Send(It.IsAny<GetEmployeesForRoleQuery>(), default(CancellationToken)), Times.Once());
        }

    }
}
