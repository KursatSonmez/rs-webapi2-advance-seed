using RS.Core.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace RS.Core.Controllers
{
    [RoutePrefix("api/Test")]
    public class TestController : ApiController
    {
        private IUserService _service;
        public TestController(IUserService service)
        {
            _service = service;
        }

        [Route("Get")]
        public async Task<IHttpActionResult> GetByIDTest()
        {
            await _service.GetTest();
            return Ok();
        }
    }
}
