using RS.Core.Data;
using RS.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RS.Core.Service
{
    public class EntityUnitofWork<Y> : IDisposable
        where Y:struct
    {
        private RSCoreDBContext con;
        public EntityUnitofWork(RSCoreDBContext _con)
        {
            con = _con;
        }
        private Dictionary<Type, object> repositories = new Dictionary<Type, object>();
        public IRepository<T,Y> Repository<T>() where T : Entity<Y>
        {
            if (repositories.Keys.Contains(typeof(T)) == true)
            {
                return repositories[typeof(T)] as IRepository<T,Y>;
            }
            IRepository<T,Y> repository = new EntityRepository<T,Y>(con);
            repositories.Add(typeof(T), repository);
            return repository;
        }
        public virtual async Task<int> SaveChangesAsync()
        {
            return await con.SaveChangesAsync();
        }
        private bool disposed = false;
        public virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    con.Dispose();
                }
            }
            disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
