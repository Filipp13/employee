using System.DirectoryServices.Protocols;

namespace Employee.Api
{
    public abstract class Entity<T, U>
        where T : class
        where U : SearchResultEntry
    {
        public abstract T Map(U searchResultEntry);

        public abstract string[] Attributes();
    }


}
