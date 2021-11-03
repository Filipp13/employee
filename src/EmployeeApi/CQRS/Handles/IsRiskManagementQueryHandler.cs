using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Employee
{
    internal sealed class IsRiskManagementQueryHandler : IRequestHandler<IsRiskManagementQuery, bool>
    {
        private readonly IADManagment adManagment;

        public IsRiskManagementQueryHandler(IADManagment adManagment)
        {
            this.adManagment = adManagment;
        }

        public async Task<bool> Handle(IsRiskManagementQuery request, CancellationToken cancellationToken)
        => await adManagment.IsAdminAsync(request.Login) || 
            await adManagment.IsUsersInsideGroupAsync("CIS Trinity RM", request.Login);

    }
}
