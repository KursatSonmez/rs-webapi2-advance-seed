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
        private RSCoreDBContext _con;
        public EntityUnitofWork(RSCoreDBContext con)
        {
            _con = con;
        }
        private Dictionary<Type, object> repositories = new Dictionary<Type, object>();
        public IRepository<T,Y> Repository<T>() where T : Entity<Y>
        {
            if (repositories.Keys.Contains(typeof(T)) == true)
                return repositories[typeof(T)] as IRepository<T,Y>;

            IRepository<T,Y> repository = new EntityRepository<T,Y>(_con);
            repositories.Add(typeof(T), repository);
            return repository;
        }
        public virtual async Task<int> SaveChangesAsync()
        {
            return await _con.SaveChangesAsync();
        }
        private bool disposed = false;
        public virtual void Dispose(bool disposing)
        {
            if (!disposed)
                if (disposing)
                    _con.Dispose();

            disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
