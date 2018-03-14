using RS.Core.Const;
using RS.Core.Domain;
using RS.Core.Service.DTOs;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace RS.Core.Service
{
    public interface IAutoCodeService : IBaseService<AutoCodeAddDto, AutoCodeUpdateDto, AutoCodeGetDto,AutoCodeGetDto,
        AutoCodeFilterDto, SysAutoCode, Guid>
    {
        Task<string> AutoCodeGenerate(string screenCode, Guid userId);
    }
    public class AutoCodeService : BaseService<AutoCodeAddDto, AutoCodeUpdateDto, AutoCodeGetDto, AutoCodeGetDto,
        AutoCodeFilterDto, SysAutoCode, Guid>, IAutoCodeService
    {
        private IAutoCodeLogService _autoCodeLogService = null;
        public AutoCodeService(EntityUnitofWork<Guid> uow, IAutoCodeLogService autoCodeLogService) : base(uow)
        {
            _autoCodeLogService = autoCodeLogService;
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
        public override Task<APIResult> Add(AutoCodeAddDto model, Guid userId, bool isCommit = true)
        {
            if (!CheckScreenCode(model.ScreenCode))
                return Task.FromResult(new APIResult { Message = Messages.GNW0002 });

            if (!CheckCodeFormat(model.CodeFormat))
                return Task.FromResult(new APIResult { Message = Messages.ACW0001 });

            return base.Add(model, userId, isCommit);
        }
        public override Task<APIResult> Update(AutoCodeUpdateDto model, Guid userId, bool isCommit = true, bool checkAuthorize = false)
        {
            if (!CheckCodeFormat(model.CodeFormat))
                return Task.FromResult(new APIResult { Message = Messages.ACW0001 });

            return base.Update(model, userId, isCommit, checkAuthorize);
        }
        public async Task<string> AutoCodeGenerate(string screenCode, Guid userId)
        {
            string code = null;

            SysAutoCode entity = await _uow.Repository<SysAutoCode>().Get()
                .FirstOrDefaultAsync(x => x.ScreenCode == screenCode);

            if (entity != null)
            {
                int lastCodeNumber = ++entity.LastCodeNumber;
                code = String.Format(entity.CodeFormat, lastCodeNumber);

                entity.LastCodeNumber = lastCodeNumber;

                //Log
                await _autoCodeLogService.Add(new SysAutoCodeLog
                {
                    CodeNumber = lastCodeNumber,
                    CodeGenerationDate = DateTime.Now,
                    AutoCodeId = entity.Id,
                    GeneratedBy = userId
                }, false);

                await _uow.SaveChangesAsync();
            }

            return code;
        }
    }
}