using RS.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RS.Core.Testing
{
    public interface IInMemoryUnitofWork<Y>
        where Y:struct
    {
        IInMemoryData<T,Y> Repository<T>() where T : Entity<Y>;
    }
    public class InMemoryUnitofWork<Y>:IInMemoryUnitofWork<Y>
        where Y:struct
    {
        private Dictionary<Type, object> repositories = new Dictionary<Type, object>();
        public IInMemoryData<T,Y> Repository<T>() where T : Entity<Y>
        {
            if (repositories.Keys.Contains(typeof(T)) == true)
            {
                return repositories[typeof(T)] as IInMemoryData<T,Y>;
            }

            IInMemoryData<T,Y> repository = new InMemoryData<T,Y>();
            repositories.Add(typeof(T), repository);
            return repository;
        }
    }
}
