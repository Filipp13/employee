using System.Collections.Generic;

namespace Employee
{
    public sealed class RolesManagment : IRolesManagment
    {
        private readonly Dictionary<string, string> rolesSP;
        private readonly List<string> rolesAD;

        public RolesManagment()
        {
            rolesSP = new Dictionary<string, string> {
                { "EQCR", "EQCR"},
                { "EP", "EngagementPartner"},
                { "CATL", "GeneralDirector"},
                { "EM", "EngagementManager"},
                { "SP", "SpecialReviewPartner"},
                { "NEQCR", "NonCertifiedEQCR"},
                { "PSR", "PSR"},
                { "EA", "Author"}
            };

            rolesAD = new List<string> { "PPD", "CONSP" };
        }

        public bool IsSPRole(string role)
        => rolesSP.ContainsKey(role);

        public bool IsADRole(string role)
        => rolesAD.Contains(role);

        public string SPRole(string role)
        => rolesSP.ContainsKey(role) ? rolesSP.GetValueOrDefault(role) ?? string.Empty : string.Empty;
    }
}
