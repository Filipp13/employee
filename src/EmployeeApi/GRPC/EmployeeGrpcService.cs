using EmployeeGrpcService;
using Grpc.Core;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Employee.Api
{
    [Authorize]
    public class EmployeeGrpcService : EmployeeGrpc.EmployeeGrpcBase
    {
        private readonly ILogger<EmployeeGrpcService> logger;
        private readonly IMediator mediator;
        private readonly IMapper mapper;

        public EmployeeGrpcService(
            ILogger<EmployeeGrpcService> logger,
            IMediator mediator,
            IMapper mapper)
        {
            this.logger = logger;
            this.mediator = mediator;
            this.mapper = mapper;
        }

        public async override Task<UserInfoResponse> GetUserInfo(UserInfoRequest request, ServerCallContext context)
        => await mediator.Send(new GetEmployeeQuery(request.Login)) switch
        {
            EmployeeMvc employee when employee is not null => mapper.Map(employee),
            _ => throw new RpcException(new Status(StatusCode.NotFound, $"employee with login {request.Login} is absent"))
        };

        public async override Task<EmployeesResponse> GetEmployeesByLogins(EmployeesRequest request, ServerCallContext context)
        => mapper.Map(await mediator.Send(new GetEmployeesByLoginsQuery(request.Logins)));
    }
}