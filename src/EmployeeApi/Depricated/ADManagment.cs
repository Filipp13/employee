using Microsoft.Extensions.Options;
using System;
using System.DirectoryServices.Protocols;
using System.Net;
using System.Threading.Tasks;

namespace EmployeeApi
{
    public sealed class ADManagment : IADManagment
    {
        private readonly LdapDirectoryIdentifier identifier;
        private readonly NetworkCredential credential;
        public ADManagment(IOptions<ADManagmentOptions> aDManagmentOptions)
        {
            _ = aDManagmentOptions
                ?? throw new ArgumentNullException(nameof(aDManagmentOptions.Value));
            identifier = new LdapDirectoryIdentifier(aDManagmentOptions.Value.Address, aDManagmentOptions.Value.Port);
            credential = new NetworkCredential(
                Environment.GetEnvironmentVariable("UserADLogin"),
                Environment.GetEnvironmentVariable("UserADPassword"));
        }

        public Task<bool> IsAdminAsync(string login)
        => IsUsersInsideGroupAsync("Users", "CIS Trinity Admins", login);

        public Task<bool> IsUsersInsideGroupAsync(string groupName, string login)
        => IsUsersInsideGroupAsync("Users", groupName, login);

        public Task<bool> IsUsersInsideServiceGroupAsync(string groupName, string login)
        => IsUsersInsideGroupAsync("CIS IT", groupName, login);

        private async Task<bool> IsUsersInsideGroupAsync(string ou, string groupName, string login)
        {
            string group = groupName;

            if (groupName.Split(',').Length == 2)
            {
                group = groupName.Split(',')[1];
            }

            string DistinguishedName = $"OU={ou},OU=CIS,DC=atrema,DC=deloitte,DC=com";
            string Filter = $"(&(objectCategory=person)(sAMAccountName={login})(memberOf=CN={group},OU=Distribution Groups,OU=CIS,DC=atrema,DC=deloitte,DC=com))";

            return await SendADRequestAsync(DistinguishedName, Filter) switch
            {
                SearchResponse searchResponse when
                                            searchResponse is not null
                                            && searchResponse.Entries.Count > 0 => true,
                _ => false
            };

        }

        private async Task<SearchResponse> SendADRequestAsync(string DistinguishedName, string Filter)
        {
            using (LdapConnection ldap = new LdapConnection(identifier, credential))
            {
                try
                {
                    var searchRequest = new SearchRequest();
                    searchRequest.DistinguishedName = DistinguishedName;
                    searchRequest.Filter = Filter;
                    searchRequest.SizeLimit = Int32.MaxValue;

                    var response = await Task<DirectoryResponse>.Factory.FromAsync(
                        ldap.BeginSendRequest,
                        (iar) => ldap.EndSendRequest(iar),
                        searchRequest,
                        PartialResultProcessing.NoPartialResultSupport,
                        null) as SearchResponse;

                    return response!;

                }
                catch
                {
                    //log
                    throw;
                }
            }
        }

    }
}