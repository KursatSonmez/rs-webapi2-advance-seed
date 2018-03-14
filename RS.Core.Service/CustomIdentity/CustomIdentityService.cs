using RS.Core.Data;
using RS.Core.Domain;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace RS.Core.Service.CustomIdentity
{
    public class CustomIdentityService
    {
        protected RSCoreDBContext _con; 
        protected EntityUnitofWork<Guid> _uow;
        public CustomIdentityService()
        {
            _con = new RSCoreDBContext();
            _uow = new EntityUnitofWork<Guid>(_con);
        }
        public async Task<Guid> UserId(Guid identityUserId)
        {
            return await _uow.Repository<User>().Query()
                .Where(x => x.IdentityUserId == identityUserId)
                .Select(us => us.Id).SingleAsync();
        }
        public async Task UpdateLoginDate(Guid identityUserId, bool isCommit = true)
        {
            User userEntity = await _uow.Repository<User>().Get()
                .Where(x => x.IdentityUserId == identityUserId)
                .SingleOrDefaultAsync();

            if (userEntity != null)
            {
                userEntity.LastLoginDate = DateTime.Now;

                if (isCommit)
                    await _uow.SaveChangesAsync();
            }

        }
    }
}
