using EmployeeApi.Domain;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Employee.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("private/api/trinity/employee")]
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
            EmployeeDto employee when employee is not null => Ok(employee),
            _ => NotFound($"employee with login {login} is absent")
        };
    }
}
