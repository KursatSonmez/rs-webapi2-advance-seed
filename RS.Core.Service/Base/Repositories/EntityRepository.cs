using RS.Core.Data;
using RS.Core.Domain;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace RS.Core.Service
{
    public class EntityRepository<T,Y> : IRepository<T,Y>
          where T : Entity<Y>
          where Y : struct
    {
        private RSCoreDBContext con;
        public EntityRepository(RSCoreDBContext context)
        {
            con = context;
        }
        public RSCoreDBContext Context
        {
            get { return con; }
            set { con = value; }

        }
        public virtual async Task<T> GetByID(Y id, bool isDeleted=false)
        {
            return await con.Set<T>().
                Where(Predicate.Equal<T,Y>("ID",id)).
                Where(x=>x.IsDeleted==isDeleted).
                FirstOrDefaultAsync();
        } 
        public IQueryable<T> Get(bool isDeleted = false)
        {
            return con.Set<T>().Where(x=>x.IsDeleted==isDeleted).AsQueryable();
        }
        public IQueryable<T> Query(bool isDeleted = false)
        {
            return con.Set<T>().AsNoTracking().Where(x => x.IsDeleted==isDeleted);
        }
        public virtual void Add(T entity)
        {
            con.Set<T>().Add(entity);
        }
    
    }
}