using AutoMapper;
using RS.Core.Const;
using RS.Core.Domain;
using RS.Core.Lib.Email;
using RS.Core.Service.DTOs;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Threading.Tasks;

namespace RS.Core.Service
{
    public interface IUserService : IBaseService<UserAddDto, UserUpdateDto, UserListDto,
        UserCardDto, UserFilterDto, User, Guid>
    {
        Task<APIResult> Register(UserAddDto model, Guid identityUserId);
        APIResult RemindPassword(string email, string code, string id);
    }

    public class UserService : BaseService<UserAddDto, UserUpdateDto, UserListDto,
        UserCardDto, UserFilterDto, User, Guid>, IUserService
    {
        private IEmailService _emailService;

        public UserService(EntityUnitofWork<Guid> uow, IEmailService emailService) : base(uow)
        {
            _emailService = emailService;
        }
        public async Task<APIResult> Register(UserAddDto model, Guid identityUserId)
        {
            bool duplicateUserCheck = await _uow.Repository<User>().Query().AnyAsync(x => x.Email == model.Email);
            if (duplicateUserCheck)
                return new APIResult { Message = Messages.GNE0003 };

            User entity = Mapper.Map<User>(model);
            entity.Id = Guid.NewGuid();
            entity.IdentityUserId = identityUserId;
            entity.CreateBy = entity.Id;
            entity.CreateDT = DateTime.Now;

            _uow.Repository<User>().Add(entity);
            await _uow.SaveChangesAsync();

            return new APIResult { Data = entity.Id, Message = Messages.Ok };
        }
        public APIResult RemindPassword(string email, string code, string id)
        {
            //Change `mailSettings` in web.config for send email.
            try
            {
                EmailBasicTemplateDto emailModel = new EmailBasicTemplateDto
                {
                    To = new List<string> { email },
                    Subject = "RS Support",
                    BackgroundColor = "b61528",
                    Header = "Password Change Request",
                    Content = "We understand you're having trouble logging into your account. We can help you regain access to your account. " +
                              "You can create a new password by clicking on the 'Change Password' button If this is not the case, you can ask our support team for help by emailing support@remmsoft.com",
                    ButtonValue = "Change Password",
                    //`resetPasswordUrl` is get from Web.Config.
                    URL = ConfigurationManager.AppSettings["resetPasswordUrl"] + "?id=" + id + "?code=" + code
                };

                _emailService.SendMailBasicTemplate(emailModel);
                return new APIResult { Message = Messages.Ok };
            }
            catch (Exception ex)
            {
                return new APIResult { Message = ex.Message };
            }
        }
    }
}