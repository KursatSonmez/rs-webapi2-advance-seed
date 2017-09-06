using AutoMapper;
using RS.Core.Domain;
using RS.Core.Service.DTOs;

namespace RS.Core.Service.AutoMapper
{
    public class AutoMapperConfig
    {
        public static void Configure()
        {
            Mapper.Initialize(x =>
            {
                #region User
                x.CreateMap<UserAddDto, User>();
                x.CreateMap<UserUpdateDto, User>();
                x.CreateMap<User, UserListDto>();
                x.CreateMap<User, UserCardDto>();
                #endregion

            });
        }
    }
}