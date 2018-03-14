using AutoMapper;
using RS.Core.Domain;
using RS.Core.Service.DTOs;
using System;

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
                x.CreateMap<AutoCodeAddDto, SysAutoCode>();
                x.CreateMap<AutoCodeUpdateDto, SysAutoCode>();
                x.CreateMap<SysAutoCode, AutoCodeGetDto>();
                x.CreateMap<SysAutoCodeLog, AutoCodeLogListDto>().
                    ForMember(
                        y => y.Code,
                        opt => opt.MapFrom(
                            src => src.AutoCode.CodeFormat.Replace("{0}", src.CodeNumber.ToString())));

                #endregion

                #region File
                x.CreateMap<FileDto, SysFile>().ReverseMap();
                x.CreateMap<SysFile, FileListDto>();

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