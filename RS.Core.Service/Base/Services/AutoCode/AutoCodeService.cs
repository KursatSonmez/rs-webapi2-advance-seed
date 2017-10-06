using AutoMapper.QueryableExtensions;
using RS.Core.Const;
using RS.Core.Domain;
using RS.Core.Service.DTOs;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace RS.Core.Service
{
    public interface IAutoCodeService:IBaseService<AutoCodeAddDto,AutoCodeUpdateDto,AutoCodeGetDto,AutoCode,Guid>
    {
        Task<IList<AutoCodeGetDto>> GetList(string screenCode = null);
        Task<string> AutoCodeGenerate(string screenCode, Guid userID);
    }
    public class AutoCodeService : BaseService<AutoCodeAddDto, AutoCodeUpdateDto, AutoCodeGetDto, AutoCode, Guid>, IAutoCodeService
    {
        private IAutoCodeLogService autoCodeLogService = null;
        public AutoCodeService(EntityUnitofWork<Guid> _uow, IAutoCodeLogService _autoCodeLogService) : base(_uow)
        {
            autoCodeLogService = _autoCodeLogService;
        }

        /// <summary>
        /// This controls the writing status of '{0}' in the code format.
        /// </summary>
        /// <param name="codeFormat"></param>
        /// <returns></returns>
        public bool CheckCodeFormat(string codeFormat)
        {
            return codeFormat.Contains("{0}");
        }

        /// <summary>
        /// Controls whether the screen code is in the <see cref="ScreenCodes"/> class.
        /// </summary>
        /// <param name="screenCode"></param>
        /// <returns></returns>
        public bool CheckScreenCode(string screenCode)
        {
            return typeof(ScreenCodes).GetFields().Any(x => x.Name == screenCode);
        }

        public override Task<APIResult> Add(AutoCodeAddDto model, Guid userID, bool isCommit = true)
        {
            if (!CheckScreenCode(model.ScreenCode))
                return Task.FromResult(new APIResult { Message = Messages.GNW0002 });

            if (!CheckCodeFormat(model.CodeFormat))
                return Task.FromResult(new APIResult { Message = Messages.ACW0001 });

            return base.Add(model, userID, isCommit);
        }

        public override Task<APIResult> Update(AutoCodeUpdateDto model, Guid? userID = null, bool isCommit = true, bool checkAuthorize = false)
        {
            if (!CheckCodeFormat(model.CodeFormat))
                return Task.FromResult(new APIResult { Message = Messages.ACW0001 });

            return base.Update(model, userID, isCommit, checkAuthorize);
        }

        public async Task<IList<AutoCodeGetDto>> GetList(string screenCode = null)
        {
            var query = uow.Repository<AutoCode>().Query();

            if (screenCode != null)
                query = query.Where(x => x.ScreenCode.Contains(screenCode));

            return await query.ProjectTo<AutoCodeGetDto>().ToListAsync();
        }

        public async Task<string> AutoCodeGenerate(string screenCode, Guid userID)
        {
            string code = null;

            AutoCode entity = await uow.Repository<AutoCode>().Get().
                FirstOrDefaultAsync(x => x.ScreenCode == screenCode);
                
            if (entity != null)
            {
                int lastCodeNumber = ++entity.LastCodeNumber;
                code = String.Format(entity.CodeFormat, lastCodeNumber);

                entity.LastCodeNumber = lastCodeNumber;

                //Log
                await autoCodeLogService.Add(new AutoCodeLog
                {
                    CodeNumber= lastCodeNumber,
                    CodeGenerationDate = DateTime.Now,
                    AutoCodeID = entity.ID,
                    GeneratedBy = userID
                }, false);

                await uow.SaveChangesAsync();
            }

            return code;
        }
    }
}