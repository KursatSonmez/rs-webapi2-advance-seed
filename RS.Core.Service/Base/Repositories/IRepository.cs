using RS.Core.Data;
using RS.Core.Domain;
using System.Linq;
using System.Threading.Tasks;

namespace RS.Core.Service
{
    public interface IRepository<T,Y>
        where T : Entity<Y>
        where Y:struct
    {
        RSCoreDBContext Context { get; set; }
        Task<T> GetById(Y id, bool isDeleted = false);
        IQueryable<T> Get(bool isDeleted = false);
        IQueryable<T> Query(bool isDeleted = false);
        void Add(T entity);
    }
}
