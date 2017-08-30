using Microsoft.AspNet.Identity;
using System;
using System.Security.Claims;
using System.Web;

namespace RS.Core.Controllers
{
    public static class RSTokenInformation
    {
        public static Guid IdentityUserID
        {
            get { return Guid.Parse(HttpContext.Current.User.Identity.GetUserId()); }
        }
        public static string UserName
        {
            get { return HttpContext.Current.User.Identity.GetUserName(); }
        }
        public static Guid UserID
        {
            get { return Guid.Parse(((ClaimsIdentity)HttpContext.Current.User.Identity).FindFirst("UserID").Value); }
        }
    }
}