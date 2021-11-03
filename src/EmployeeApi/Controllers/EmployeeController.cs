using EmployeeApi.Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Employee.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly ILogger<EmployeeController> logger;
        private readonly IMediator mediator;

        public EmployeeController(
            ILogger<EmployeeController> logger,
            IMediator mediator)
        {
            this.logger = logger;
            this.mediator = mediator;
        }

        [HttpGet("user-info")]
        public async Task<ActionResult<EmployeeDto>> GetUserInfoAsync()
        => await mediator.Send(new GetEmployeeQuery(User?.Identity?.Name?.Split('@')[0]
            ?? throw new NullReferenceException("User identity is absent, claim: Name"))) switch
        {
            EmployeeDto employee when employee is not null => Ok(employee),
            _ => NotFound($"employee with login {User?.Identity?.Name?.Split('@')[0]} is absent")
        };

        [HttpGet("user-info/{login}")]
        public async Task<ActionResult<EmployeeDto>> GetEmployeeByLoginAsync(string login)
        => await mediator.Send(new GetEmployeeQuery(login)) switch
        {
            EmployeeDto employee when employee is not null => Ok(employee),
            _ => NotFound($"employee with login {login} is absent")
        };

        [HttpGet("is-admin/{login}")]
        public async Task<ActionResult<bool>> IsAdminAsync(string login)
        => await mediator.Send(new IsAdminQuery(login));

        [HttpGet("is-risk-management/{login}")]
        public async Task<ActionResult<bool>> IsRiskManagementAsync(string login)
        {
            if (string.IsNullOrEmpty(login))
            {
                login = (User?.Identity?.Name?.Split('@')[0] == null ? Environment.UserName : User.Identity.Name.Split('@')[0]);
            }

            return await mediator.Send(new IsRiskManagementQuery(login));
        }

        [HttpGet("view/{searchString}")]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetEmployeesAsync(string searchString) 
        => Ok(await mediator.Send(new GetEmployeesQuery(searchString)));

        [HttpGet("view/{searchString}/{roleCode}/{armsid}/{ARMSListId}")]
        public async Task<ActionResult<IEnumerable<EmployeeForRole>>> GetEmployeesForRoleAsync(string searchString, string roleCode, int armsid, string armsListId)
        => Ok(await mediator.Send(new GetEmployeesForRoleQuery(
            searchString,
            roleCode,
            armsid,
            armsListId)));

    }
}
