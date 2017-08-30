//using System;
//using System.Collections.Generic;
//using System.Net;
//using System.Threading.Tasks;
//using System.Web.Http;
//using System.Web.Http.Description;
//using RS.Core.Const;
//using RS.Core.Service;
//using RS.Core.Service.DTOs;

//namespace RS.Core.Controllers
//{
//    [Authorize]
//        public class BaseController<A,U,G,S> : ApiController
//        where S:ICRUDService<A,U,G>
//    {
//        protected S service;
//        public BaseController(S _service)
//        {
//            service = _service;
//        }
//        [Route("Add"),HttpPost]
//        public virtual async Task<IHttpActionResult> Add(A model)
//        {
//            ///İlgili modelin şartlarını kontrol eder.
//            if (!ModelState.IsValid)
//                return BadRequest(ModelState);

//            ///Generic olan gelen servisin Add metoduna ilgili view model 
//            ///ve token bilgisini okuyarak elde edilen User ID gönderilmiştir.
//            var result = await service.Add(model,RSTokenInformation.UserID);

//            ///Kayıt işleminin sonucunu kontrol eder.
//            if (result.Message != Messages.Ok)
//                return Content(HttpStatusCode.BadRequest, result);

//            return Ok(result);
//        }
//        [Route("Update"), HttpPut]
//        public virtual async Task<IHttpActionResult> Update(U model)
//        {
          
//            ///İlgili modelin şartlarını kontrol eder.
//            if (!ModelState.IsValid)
//                return BadRequest(ModelState);

//            ///Generic olan gelen servisin Update metoduna ilgili view model 
//            ///ve token bilgisini okuyarak elde edilen User ID gönderilmiştir.
//            var result = await service.Update(model, RSTokenInformation.UserID);

//            ///Update işleminin sonucunu kontrol eder.
//            if (result.Message != Messages.Ok)
//                return Content(HttpStatusCode.BadRequest,result);

//            return Ok(result);
//        }
//        [Route("Delete"),HttpDelete]
//        public virtual async Task<IHttpActionResult> Delete(Guid id)
//        {
//            ///Generic olan gelen servisin Delete metoduna ilgili id
//            ///ve token bilgisini okuyarak elde edilen User ID gönderilmiştir.
//            var result = await service.Delete(id, RSTokenInformation.UserID);

//            ///Delete işleminin sonucunu kontrol eder.
//            if (result.Message != Messages.Ok)
//                return Content(HttpStatusCode.BadRequest, result);

//            return Ok(result);
//        }
//        [Route("Get"),HttpGet]
//        public virtual IHttpActionResult GetByID(Guid id)
//        {
//            var result = service.GetByID(id);
//            return Ok(result);
//        }
//        [Route("GetSelectList"), HttpGet,ResponseType(typeof(List<AutoCompleteListVM>))]
//        public virtual IHttpActionResult AutoCompleteList(Guid? ID = null, string Value = null)
//        {
//            var result = service.AutoCompleteList(ID, Value);

//            if (result == null)
//                return Content(HttpStatusCode.OK, new string[0]);
//            return Ok(result);
//        }
//    }
//}
