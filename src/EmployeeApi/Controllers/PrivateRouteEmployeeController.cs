using EmployeeApi.Domain;
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
        private readonly IEmployeeRepository employeeRepository;

        public PrivateRouteEmployeeController(IEmployeeRepository employeeRepository)
        {
            this.employeeRepository = employeeRepository;
        }

        [HttpGet("user-info/{login}")]
        public async Task<ActionResult<EmployeeDto>> GetUserInfoByLgin(string login)
        => await employeeRepository.EmployeeByLoginAsync(login) switch
        {
            EmployeeDto employee when employee is not null => Ok(employee),
            _ => NotFound($"employee with login {login} is absent")
        };
    }
}
