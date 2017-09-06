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
        protected RSCoreDBContext con; 
        protected EntityUnitofWork<Guid> uow;
        public CustomIdentityService()
        {
            con = new RSCoreDBContext();
            uow = new EntityUnitofWork<Guid>(con);
        }
        public async Task<Guid> UserID(Guid identityUserID)
        {
            return await uow.Repository<User>().Query().Where(x => x.IdentityUserID == identityUserID).
                Select(us => us.ID).SingleAsync();
        }
        public async Task UpdateLoginDate(Guid identityUserID, bool isCommit = true)
        {
            User userEntity = await uow.Repository<User>().Get().Where(x => x.IdentityUserID == identityUserID).SingleOrDefaultAsync();

            if (userEntity != null)
            {
                userEntity.LastLoginDate = DateTime.Now;

                if (isCommit)
                    await uow.SaveChangesAsync();
            }

        }
    }
}
