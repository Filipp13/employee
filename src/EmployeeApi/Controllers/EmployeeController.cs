using ArmsHttpClient;
using EmployeeApi.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Employee.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private const string V = "For Consulted Party role please choose a person authorized in DPM 3500A CIS - Consultation";
        private const string V1 = "Special Partner, QCR, Consulted Party and PSR cannot be equal to Engagement Partner or Certified Auditor. Please choose another person.";
        private readonly ILogger<EmployeeController> logger;
        private readonly IEmployeeRepository employeeRepository;
        private readonly IArmsApi armsApi;
        private readonly IADManagment adManagment;
        private readonly IRolesManagment rolesManagment;
        private readonly string armsList;

        public EmployeeController(
            ILogger<EmployeeController> logger,
            IEmployeeRepository employeeRepository,
            IArmsApi armsApi,
            IADManagment adManagment,
            IRolesManagment rolesManagment,
            IConfiguration configuration)
        {
            this.logger = logger;
            this.employeeRepository = employeeRepository;
            this.armsApi = armsApi;
            this.adManagment = adManagment;
            this.rolesManagment = rolesManagment;
            armsList = configuration.GetValue<string>("ARMSActiveListGUID");
        }

        [HttpGet("user-info")]
        public async Task<ActionResult<EmployeeDto>> GetUserInfo()
        => await employeeRepository.EmployeeByLoginAsync(User.Identity.Name.Split('@')[0]) switch
        {
            EmployeeDto employee when employee is not null => Ok(employee),
            _ => NotFound($"employee with login {User.Identity.Name.Split('@')[0]} is absent")
        };

        [HttpGet("user-info/{login}")]
        public async Task<ActionResult<EmployeeDto>> GetEmployeeByLoginAsync(string login)
        => await employeeRepository.EmployeeByLoginAsync(login) switch
        {
            EmployeeDto employee when employee is not null => Ok(employee),
            _ => NotFound($"employee with login {login} is absent")
        };

        [HttpGet("is-admin/{login}")]
        public async Task<ActionResult<bool>> IsAdmin(string login)
        => await adManagment.IsAdminAsync(login);

        [HttpGet("is-risk-management/{login}")]
        public async Task<ActionResult<bool>> IsRiskManagement(string login)
        {
            if (string.IsNullOrEmpty(login))
            {
                login = (User.Identity.Name.Split('@')[0] == null ? Environment.UserName : User.Identity.Name.Split('@')[0]);
            }

            return await adManagment.IsAdminAsync(login) || await adManagment.IsUsersInsideGroupAsync("CIS Trinity RM", login);
        }

        [HttpGet("view/{searchString}")]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetEmployeesAsync(string searchString)
        => await employeeRepository.SearchEmployeeByDisplayName(searchString);

        [HttpGet("view/{searchString}/{roleCode}/{armsid}/{ARMSListId}")]
        public async Task<ActionResult<IEnumerable<EmployeeForRole>>> GetEmployeesForRoleAsync(string searchString, string roleCode, int armsid, string armsListId)
        {
            string listName = armsListId == armsList ? "Records%20Archive" : "Records";

            var employees = await employeeRepository.SearchEmployeeByDisplayName(searchString);

            List<EmployeeForRole> retval = new List<EmployeeForRole>();

            foreach (var empl in employees)
            {
                var canBeAssToRole = roleCode switch
                {
                    var role when rolesManagment.IsADRole(role) =>
                        (await adManagment.IsUsersInsideGroupAsync("CIS Trinity PPD", empl.LastName), V),

                    var role when rolesManagment.IsSPRole(role) &&
                        (empl.Id == await EmployeeFromSP(armsid, listName, "EP") ||
                            empl.Id == await EmployeeFromSP(armsid, listName, "CATL")) =>
                        (false, V1),

                    _ => (false, string.Empty)
                };

                var employeeForRole = new EmployeeForRole(empl);

                //builder
                employeeForRole.CanBeAssignedToRole = canBeAssToRole.Item1;
                employeeForRole.Explanation = canBeAssToRole.Item2;
                retval.Add(employeeForRole);
            }
            return retval;
        }

        private async Task<int> EmployeeFromSP(int armsid, string listName, string role)
        {
            var login = (await armsApi.GetLoginAsync(listName, armsid, rolesManagment.SPRole(role))).Split('\\')[1];
            var empl = await employeeRepository.EmployeeByLoginAsync(login);
            return empl is not null ? empl.Id : 0;
        }
    }
}
