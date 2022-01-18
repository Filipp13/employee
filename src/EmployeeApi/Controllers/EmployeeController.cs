using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Employee.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/employee")]
    public class EmployeeController : ControllerBase
    {
        private readonly IMediator mediator;

        public EmployeeController(
            IMediator mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// Получает инфу о пользователе по логину(логин берется из токена)
        /// </summary>
        /// <returns></returns>
        [HttpGet("user-info")]
        public async Task<ActionResult<EmployeeMvc>> GetUserInfoAsync()
        => await mediator.Send(new GetEmployeeQuery(User?.Identity?.Name?.Split('@')[0]
            ?? throw new NullReferenceException("User identity is absent, claim: Name"))) switch
        {
            EmployeeMvc employee when employee is not null => Ok(employee),
            _ => NotFound($"employee with login {User?.Identity?.Name?.Split('@')[0]} is absent")
        };

        /// <summary>
        /// Получает инфу о пользователе по логину
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        [HttpGet("user-info/{login}")]
        public async Task<ActionResult<EmployeeMvc>> GetEmployeeByLoginAsync(string login)
        => await mediator.Send(new GetEmployeeQuery(login)) switch
        {
            EmployeeMvc employee when employee is not null => Ok(employee),
            _ => NotFound($"employee with login {login} is absent")
        };

        /// <summary>
        /// Администратор? вхождение в группу AD 'CIS Trinity Admins'
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        [HttpGet("is-admin/{login}")]
        public async Task<ActionResult<bool>> IsAdminAsync(string login)
        => await mediator.Send(new IsAdminQuery(login));

        /// <summary>
        /// Менеджер? вхождение в группу AD 'CIS Trinity Admins' или 'CIS Trinity RM'
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        [HttpGet("is-risk-management/{login}")]
        public async Task<ActionResult<bool>> IsRiskManagementAsync(string login)
        {
            if (string.IsNullOrEmpty(login))
            {
                login = User?.Identity?.Name?.Split('@')[0] == null ? throw new NullReferenceException("User identity is absent, claim: Name") : User.Identity.Name.Split('@')[0];
            }

            return await mediator.Send(new IsRiskManagementQuery(login));
        }

        /// <summary>
        /// поиск сотрудников по ФИО начиная с трех символов, отображая первые 10
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns></returns>
        [HttpGet("view/{searchString}")]
        public async Task<ActionResult<IEnumerable<EmployeeMvc>>> GetEmployeesAsync(string searchString) 
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
