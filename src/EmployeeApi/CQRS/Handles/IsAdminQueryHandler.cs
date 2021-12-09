using ADManager;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Employee.Api
{
    internal sealed class IsAdminQueryHandler : IRequestHandler<IsAdminQuery, bool>
    {
        private readonly IADManager aDManager;

        public IsAdminQueryHandler(IADManager aDManager)
        {
            this.aDManager = aDManager;
        }

        public async Task<bool> Handle(IsAdminQuery request, CancellationToken cancellationToken)
        => await aDManager.IsUsersInsideGroupAsync("CIS Trinity Admins", request.Login);


    }
}
