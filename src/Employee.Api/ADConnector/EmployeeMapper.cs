using System;
using System.DirectoryServices.Protocols;

namespace Employee.Api
{
    public class EmployeeMapper : Entity<EmployeeAD, SearchResultEntry>
    {
        public EmployeeMapper()
        {
        }

        public override EmployeeAD Map(SearchResultEntry searchResultEntry)
        {

            return new EmployeeAD
            {
                SamAccountName = ExtractProperty(searchResultEntry, "SamAccountName"),
                DisplayName = ExtractProperty(searchResultEntry, "DisplayName"),
                Email = ExtractProperty(searchResultEntry, "mail"),
                Title = ExtractProperty(searchResultEntry, "Title"),
                ObjectSid = searchResultEntry.Attributes["objectSid"]?[0] as byte[] ?? new byte[0],
                Grade = ExtractProperty(searchResultEntry, "extensionAttribute2"),
                City = ExtractProperty(searchResultEntry, "l"),
                UserAccountControl = Convert.ToInt32(ExtractProperty(searchResultEntry, "userAccountControl")),
            };
        }

        private static string ExtractProperty(SearchResultEntry searchResultEntry, string attribute)
        => searchResultEntry.Attributes[attribute]?[0] as string ?? string.Empty;

        public override string[] Attributes()
        => new[] { 
            "DistinguishedName",
            "objectSid",
            "mail",
            "DisplayName",
            "Title",
            "SamAccountName",
            "extensionAttribute2",
            "l",
            "userAccountControl" };
    }

    public class EmployeeAD
    {
        public byte[] ObjectSid { get; set; }

        public string Email { get; set; }

        public string DisplayName { get; set; }

        public string Title { get; set; }

        public string SamAccountName { get; set; }

        public string Grade { get; set; }

        public string City { get; set; }

        public int UserAccountControl { get; set; }
    }
}
