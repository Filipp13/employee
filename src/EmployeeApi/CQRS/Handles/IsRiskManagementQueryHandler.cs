using ADManager;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace EmployeeApi
{
    internal sealed class IsRiskManagementQueryHandler : IRequestHandler<IsRiskManagementQuery, bool>
    {
        private readonly IADManager aDManager;

        public IsRiskManagementQueryHandler(IADManager aDManager)
        {
            this.aDManager = aDManager;
        }

        public async Task<bool> Handle(IsRiskManagementQuery request, CancellationToken cancellationToken)
        => await aDManager.IsUsersInsideGroupAsync("CIS Trinity Admins", request.Login) || 
            await aDManager.IsUsersInsideGroupAsync("CIS Trinity RM", request.Login);

    }
}
