using AutoMapper;
using RS.Core.Domain;
using RS.Core.Service.DTOs;

namespace RS.Core.Service.AutoMapper
{
    public class AutoMapperConfig<Y>
        where Y:struct
    {
        public static void Configure()
        {
            Mapper.Initialize(x =>
            {
                #region AutoCompleteList
                x.CreateMap<AutoCompleteList<Y>, AutoCompleteListVM<Y>>().ReverseMap();

                #endregion

                #region AutoCode
                x.CreateMap<AutoCodeAddDto, AutoCode>();
                x.CreateMap<AutoCodeUpdateDto, AutoCode>();
                x.CreateMap<AutoCode, AutoCodeGetDto>();
                x.CreateMap<AutoCodeLog, AutoCodeLogListDto>().
                    ForMember(
                        y => y.Code,
                        opt => opt.MapFrom(
                            src => src.AutoCode.CodeFormat.Replace("{0}", src.CodeNumber.ToString())));

                #endregion

                #region User
                x.CreateMap<UserAddDto, User>();
                x.CreateMap<UserUpdateDto, User>();
                x.CreateMap<User, UserListDto>();
                x.CreateMap<User, UserCardDto>();
                x.CreateMap<User, AutoCompleteList<Y>>().
                    ForMember(
                        y => y.Search,
                        opt => opt.MapFrom(
                            src => src.Email + " " + src.Name + " " + src.Phone)).
                    ForMember(
                        y => y.Text,
                        opt => opt.MapFrom(
                            src => src.Name));

                #endregion

            });
        }
    }
}