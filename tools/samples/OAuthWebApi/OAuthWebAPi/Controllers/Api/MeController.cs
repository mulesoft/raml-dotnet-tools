using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace OAuthWebApi.Controllers
{
    [Authorize]
    public class MeController : ApiController
    {
        [Route("api/me")]
        [HttpGet]
        public string Get()
        {
            return "Hello " + this.User.Identity.Name + "!!!";
        }
    }
}
