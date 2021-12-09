using ADManager;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.DirectoryServices.Protocols;
using System.Linq;
using System.Threading.Tasks;

namespace Employee.Api
{
    public sealed class ADManagment<Entity, EntityMapper> : ADManagerBase, IADManagmentEntity<Entity, EntityMapper> 
        where Entity : class
        where EntityMapper : Entity<Entity, SearchResultEntry>
    {
        const string distinguishedName = "OU=Users,OU=CIS,DC=atrema,DC=deloitte,DC=com";

        const string filter = "(&(objectCategory=person)(|(employeeType=Full-Time Employee)(employeeType=Contractor)))";

        private readonly Entity<Entity, SearchResultEntry> entity;
        public ADManagment(
            IOptions<ADManagerOptions> aDManagmentOptions,
            ADManagerSecurityOptions aDManagerSecurityOptions,
            Entity<Entity, SearchResultEntry> entity) : base(aDManagmentOptions, aDManagerSecurityOptions) =>
                this.entity = entity;

        public async Task<List<Entity>> GetEntityOfT()
        {
            var searchResponse = await SendADRequestAsync(distinguishedName, filter, entity.Attributes());

            return Enumerable
                    .Range(0, searchResponse.Entries.Count)
                    .Select(index => entity.Map(searchResponse.Entries[index]))
                    .ToList();
        }

    }
}