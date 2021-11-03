using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Employee
{
    internal sealed class IsAdminQueryHandler : IRequestHandler<IsAdminQuery, bool>
    {
        private readonly IADManagment adManagment;

        public IsAdminQueryHandler(IADManagment adManagment)
        {
            this.adManagment = adManagment;
        }

        public async Task<bool> Handle(IsAdminQuery request, CancellationToken cancellationToken)
        => await adManagment.IsAdminAsync(request.Login);


    }
}
