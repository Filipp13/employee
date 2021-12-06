using Employee.Api.Domain;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Employee.Api.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("private/api/employee")]
    public class PrivateRouteEmployeeController : ControllerBase
    {
        private readonly IMediator mediator;

        public PrivateRouteEmployeeController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet("user-info/{login}")]
        public async Task<ActionResult<EmployeeDto>> GetUserInfoByLgin(string login)
        => await mediator.Send(new GetEmployeeQuery(login)) switch
        {
            EmployeeMvc employee when employee is not null => Ok(employee),
            _ => NotFound($"employee with login {login} is absent")
        };

        [HttpGet("actualizeemployees")]
        public async Task<ActionResult<int>> ActualizeEmployees()
        => Ok(await mediator.Send(new ImportEmployeeCommand()));
    }
}
