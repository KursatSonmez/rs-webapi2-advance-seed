using RS.Core.Data;
using RS.Core.Domain;
using System;
using System.Data.Entity;
using System.Threading.Tasks;

namespace RS.Core.Service
{
    public interface IUserService
    {
        Task GetTest();
    }
    public class UserService : IUserService
    {
        public async Task GetTest()
        {
            RSCoreDBContext con = new RSCoreDBContext();
            EntityRepository<User, Guid> repo = new EntityRepository<User, Guid>(con);
            EntityUnitofWork<Guid> uow = new EntityUnitofWork<Guid>(con);

            //Asqueryable Test
            var asQuearyableResult = await repo.Get().ToListAsync();

            //GetByID Test
            var id = Guid.Parse("AAB456DC-AB24-4701-8367-23A42C0E657B");
            var getByIdResult = await repo.GetByID(id);

            //Uow Test
            var uoWResult = await uow.Repository<User>().Get().ToListAsync();

        }
        
    }
}