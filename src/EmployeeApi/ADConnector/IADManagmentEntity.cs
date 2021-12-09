using System.Collections.Generic;
using System.Threading.Tasks;

namespace Employee.Api
{
    public interface IADManagmentEntity<Entity, EntityMapper> where Entity : class
    {
        Task<List<Entity>> GetEntityOfT();
    }
}