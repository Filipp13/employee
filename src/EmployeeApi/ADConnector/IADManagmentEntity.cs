using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmployeeApi
{
    public interface IADManagmentEntity<Entity, EntityMapper> where Entity : class
    {
        Task<List<Entity>> GetEntityOfT();
    }
}