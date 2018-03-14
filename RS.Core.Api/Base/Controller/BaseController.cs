﻿using RS.Core.Const;
using RS.Core.Service;
using RS.Core.Service.DTOs;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace RS.Core.Controllers
{
    [Authorize]
    public class BaseController<A, U, G, C, P, Y, S> : ApiController
        where Y : struct
        where U : EntityUpdateDto<Y>
        where G : EntityGetDto<Y>
        where C : EntityGetDto<Y>
        where S : ICRUDService<A, U, G, C, P, Y>
    {
        protected S service;
        public BaseController(S _service)
        {
            service = _service;
        }
        [Route("Add"), HttpPost]
        public virtual async Task<IHttpActionResult> Add(A model)
        {
            ///İlgili modelin şartlarını kontrol eder.
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            ///Generic olarak gelen servisin Add metoduna ilgili dto ve
            ///token bilgisini okuyarak elde edilen, User ID gönderilmiştir.
            var result = await service.Add(model, IdentityClaimsValues.UserID<Y>());

            ///Kayıt işleminin sonucunu kontrol eder.
            if (result.Message != Messages.Ok)
                return Content(HttpStatusCode.BadRequest, result);

            return Ok(result);
        }
        [Route("Update"), HttpPut]
        public virtual async Task<IHttpActionResult> Update(U model)
        {
            ///İlgili modelin şartlarını kontrol eder.
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            ///Generic olan gelen servisin Update metoduna ilgili view model 
            ///ve token bilgisini okuyarak elde edilen User ID gönderilmiştir.
            var result = await service.Update(model, IdentityClaimsValues.UserID<Y>());

            ///Update işleminin sonucunu kontrol eder.
            if (result.Message != Messages.Ok)
                return Content(HttpStatusCode.BadRequest, result);

            return Ok(result);
        }
        [Route("Delete"), HttpDelete]
        public virtual async Task<IHttpActionResult> Delete(Y id)
        {
            ///Generic olan gelen servisin Delete metoduna ilgili id
            ///ve token bilgisini okuyarak elde edilen User ID gönderilmiştir.
            var result = await service.Delete(id, IdentityClaimsValues.UserID<Y>());

            ///Delete işleminin sonucunu kontrol eder.
            if (result.Message != Messages.Ok)
                return Content(HttpStatusCode.BadRequest, result);

            return Ok(result);
        }
        [Route("Get"), HttpGet]
        public virtual async Task<IHttpActionResult> GetByID(Y id)
        {
            var result = await service.GetByID(id);

            if (result == null)
                return BadRequest(Messages.GNE0001);

            return Ok(result);
        }
        [Route("AutoCompleteList"), HttpGet]
        public virtual async Task<IHttpActionResult> AutoCompleteList([FromUri]P parameters,
            Y? id = default(Y?), string text = default(string))
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await service.AutoCompleteList(parameters, id, text);

            if (result == null)
                return Content(HttpStatusCode.OK, new string[0]);

            return Ok(result);
        }
        [Route("Get"), HttpGet]
        public async Task<IHttpActionResult> Get([FromUri]P parameters, string sortField, bool sortOrder)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await service.Get(parameters, sortField, sortOrder);

            if (result == null)
                result = new List<G>();

            return Ok(result);
        }
        [Route("GetPaging"), HttpGet]
        public async Task<IHttpActionResult> GetPaging([FromUri]P parameters,
            string sortField, bool sortOrder, string sumField, int? first = null, int? rows = null)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await service.GetPaging(parameters, sortField, sortOrder, sumField, first, rows);

            if (result == null)
                result = new EntityGetPagingDto<Y, G>();

            return Ok(result);
        }
    }
}
