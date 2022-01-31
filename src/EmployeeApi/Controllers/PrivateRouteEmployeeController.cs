using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
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

        /// <summary>
        /// Получает инфу о пользователях по логинам, возвращает все что смог найти
        /// </summary>
        /// <param name="logins"></param>
        /// <returns></returns>
        [HttpPost("user-info/batch")]
        public async Task<ActionResult<IEnumerable<EmployeeMvc>>> GetEmployeesByLoginsAsync(string[] logins)
        => Ok(await mediator.Send(new GetEmployeesByLoginsQuery(logins)));

        /// <summary>
        /// Получает инфу о пользователе по логину
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        [HttpGet("user-info/{login}")]
        public async Task<ActionResult<EmployeeMvc>> GetUserInfoByLogin(string login)
        => await mediator.Send(new GetEmployeeQuery(login)) switch
        {
            var employee when employee is not null => Ok(employee),
            _ => NotFound($"employee with login {login} is absent")
        };

        /// <summary>
        /// запускает процесс актуализации таблицы сотрудников. Данные берутся из AD и БД SAP
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        [HttpGet("actualizeemployees")]
        public async Task<ActionResult<int>> ActualizeEmployees()
        => Ok(await mediator.Send(new ImportEmployeeCommand()));
    }
}
