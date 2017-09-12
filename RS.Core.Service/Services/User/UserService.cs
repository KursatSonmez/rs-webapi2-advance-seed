using AutoMapper;
using RS.Core.Const;
using RS.Core.Domain;
using RS.Core.Service.DTOs;
using System;
using System.Data.Entity;
using System.Threading.Tasks;

namespace RS.Core.Service
{
    public interface IUserService : IBaseService<UserAddDto, UserUpdateDto,UserCardDto,User,Guid>
    {
        Task<APIResult> Add(UserAddDto model, Guid identityUserID);
    }

    public class UserService : BaseService<UserAddDto, UserUpdateDto, UserCardDto, User, Guid>, IUserService
    {
        public UserService(EntityUnitofWork<Guid> _uow) : base(_uow)
        {
        }

        public async Task<APIResult> Add(UserAddDto model, Guid identityUserID)
        {
            var duplicateUserCheck = await uow.Repository<User>().Query().AnyAsync(x => x.Email == model.Email);
            if (duplicateUserCheck)
                return new APIResult { Message = Messages.GNE0003 };

            User entity = Mapper.Map<User>(model);
            entity.ID = Guid.NewGuid();
            entity.IdentityUserID = identityUserID;
            entity.CreateBy = entity.ID;
            entity.CreateDT = DateTime.Now;

            uow.Repository<User>().Add(entity);
            await uow.SaveChangesAsync();

            return new APIResult { Data = entity.ID, Message = Messages.Ok };
        }
    }
}