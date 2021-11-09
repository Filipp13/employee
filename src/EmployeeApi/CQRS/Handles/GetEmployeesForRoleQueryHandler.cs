using ArmsHttpClient;
using EmployeeApi.Controllers;
using EmployeeApi.Domain;
using MediatR;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EmployeeApi
{
    internal sealed class GetEmployeesForRoleQueryHandler : IRequestHandler<GetEmployeesForRoleQuery, IEnumerable<EmployeeForRole>>
    {
        private const string V = "For Consulted Party role please choose a person authorized in DPM 3500A CIS - Consultation";
        private const string V1 = "Special Partner, QCR, Consulted Party and PSR cannot be equal to Engagement Partner or Certified Auditor. Please choose another person.";

        private readonly IArmsApi armsApi;
        private readonly IEmployeeRepository employeeRepository;
        private readonly IADManagment adManagment;
        private readonly IRolesManagment rolesManagment;
        private readonly string armsList;

        public GetEmployeesForRoleQueryHandler(
            IEmployeeRepository employeeRepository,
            IArmsApi armsApi,
            IADManagment adManagment,
            IRolesManagment rolesManagment,
            IConfiguration configuration)
        {
            this.employeeRepository = employeeRepository;
            this.armsApi = armsApi;
            this.adManagment = adManagment;
            this.rolesManagment = rolesManagment;
            armsList = configuration.GetValue<string>("ARMSActiveListGUID");
        }

        public async Task<IEnumerable<EmployeeForRole>> Handle(GetEmployeesForRoleQuery request, CancellationToken cancellationToken)
        {
            string listName = request.ArmsListId == armsList ? "Records%20Archive" : "Records";

            var employees = await employeeRepository.SearchEmployeeByDisplayName(request.Search);

            List<EmployeeForRole> retval = new();

            foreach (var empl in employees)
            {
                var canBeAssignToRole = request.RoleCode switch
                {
                    var role when rolesManagment.IsADRole(role) =>
                        (await adManagment.IsUsersInsideGroupAsync("CIS Trinity PPD", empl.LastName), V),

                    var role when rolesManagment.IsSPRole(role) &&
                        (empl.Id == await EmployeeFromSP(request.Armsid, listName, "EP") ||
                            empl.Id == await EmployeeFromSP(request.Armsid, listName, "CATL")) =>
                        (false, V1),

                    _ => (false, string.Empty)
                };

                var employeeForRole = new EmployeeForRole(empl);

                //builder
                employeeForRole.CanBeAssignedToRole = canBeAssignToRole.Item1;
                employeeForRole.Explanation = canBeAssignToRole.Item2;
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
