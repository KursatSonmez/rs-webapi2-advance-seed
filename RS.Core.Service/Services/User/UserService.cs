using AutoMapper;
using AutoMapper.QueryableExtensions;
using RS.Core.Const;
using RS.Core.Domain;
using RS.Core.Lib.Email;
using RS.Core.Service.DTOs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace RS.Core.Service
{
    public interface IUserService : IBaseService<UserAddDto, UserUpdateDto, UserCardDto, User, Guid>
    {
        Task<APIResult> Register(UserAddDto model, Guid identityUserID);
        APIResult RemindPassword(string email, string code, string id);
        Task<IList<UserListDto>> GetList(string name = null, string email = null);
    }

    public class UserService : BaseService<UserAddDto, UserUpdateDto, UserCardDto, User, Guid>, IUserService
    {
        private IEmailService emailService;
        public UserService(EntityUnitofWork<Guid> _uow, IEmailService _emailService) : base(_uow)
        {
            emailService = _emailService;
        }

        public async Task<APIResult> Register(UserAddDto model, Guid identityUserID)
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

                emailService.SendMailBasicTemplate(emailModel);
                return new APIResult { Message = Messages.Ok };
            }
            catch (Exception ex)
            {
                return new APIResult { Message = ex.Message };
            }
        }

        public async Task<IList<UserListDto>> GetList(string name = null, string email = null)
        {
            var query = uow.Repository<User>().Query();

            if (name != null)
                query = query.Where(x => x.Name.Contains(name));
            if (email != null)
                query = query.Where(x => x.Email.Contains(email));

            return await query.ProjectTo<UserListDto>().ToListAsync();
        }

    }
}