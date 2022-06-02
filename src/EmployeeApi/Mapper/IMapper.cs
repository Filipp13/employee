using Employee.Api.Domain;
using EmployeeGrpcService;
using System.Collections.Generic;

namespace Employee.Api
{
    public interface IMapper
    {
        public EmployeeUpdateDto Map(EmployeeAD e, in Dictionary<string, EmployeeSAPDto> dictEmployees);

        public EmployeeMvc? Map(EmployeeDto employee);

        public UserInfoResponse Map(EmployeeMvc employee);

        public EmployeesResponse Map(IEnumerable<EmployeeMvc> users);

    }
}
