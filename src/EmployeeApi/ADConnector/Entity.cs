using System;
using System.Collections.Generic;
using System.DirectoryServices.Protocols;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee.Api
{
    public abstract class Entity<T, U>
        where T : class
        where U : SearchResultEntry
    {
        public abstract T Map(SearchResultEntry searchResultEntry);

        public abstract string[] Attributes();
    }


}
