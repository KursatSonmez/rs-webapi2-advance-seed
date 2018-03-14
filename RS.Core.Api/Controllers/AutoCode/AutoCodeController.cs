using RS.Core.Service;
using RS.Core.Service.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace RS.Core.Controllers
{
    [Authorize]
    [RoutePrefix("api/AutoCode")]
    public class AutoCodeController : BaseController<AutoCodeAddDto, AutoCodeUpdateDto, AutoCodeGetDto, AutoCodeGetDto,
        AutoCodeFilterDto, Guid, IAutoCodeService>
    {
        private IAutoCodeLogService autoCodeLogService = null;
        public AutoCodeController(IAutoCodeService _service, IAutoCodeLogService _autoCodeLogService) : base(_service)
        {
            autoCodeLogService = _autoCodeLogService;
        }

        [Route("AutoCodeGenerate"), HttpGet]
        [ResponseType(typeof(string))]
        public async Task<IHttpActionResult> AutoCodeGenerate(string screenCode = null)
        {
            var result = await service.AutoCodeGenerate(screenCode, IdentityClaimsValues.UserID<Guid>());

            return Ok(result);
        }

        [Route("Logs"), HttpGet]
        [ResponseType(typeof(IEnumerable<AutoCodeLogListDto>))]
        public async Task<IHttpActionResult> AutoCodeLogs(string screenCode = null, Guid? generatedBy = null, DateTime? generationDate = null)
        {
            var result = await autoCodeLogService.GetList(screenCode, generatedBy, generationDate);

            if (result == null || result.Count <= 0)
                result = new List<AutoCodeLogListDto>();

            return Ok(result);
        }
    }
}
