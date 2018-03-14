using Microsoft.AspNet.Identity;
using System;
using System.ComponentModel;
using System.Security.Claims;
using System.Web;

namespace RS.Core.Controllers
{
    public static class IdentityClaimsValues
    {
        public static Guid IdentityUserId
        {
            get { return Guid.Parse(HttpContext.Current.User.Identity.GetUserId()); }
        }
        public static string UserName
        {
            get { return HttpContext.Current.User.Identity.GetUserName(); }
        }
        public static Y UserId<Y>()
            where Y:struct
        {
            return (Y)TypeDescriptor.GetConverter(typeof(Y)).ConvertFromInvariantString(((ClaimsIdentity)HttpContext.Current.User.Identity).FindFirst("userId").Value);
        }
    }
}