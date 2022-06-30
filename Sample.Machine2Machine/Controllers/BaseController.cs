using Microsoft.AspNetCore.Mvc;
using Sample.Machine2Machine.Core;

namespace Sample.Machine2Machine.Controllers
{
    public class BaseController : Controller
    {
        protected string? GetToken()
        {
            string? token = null;
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString(ConstInfo._SessionKeyName))){
                token = HttpContext.Session.GetString(ConstInfo._SessionKeyName);
            }

            return token;
        }
    }
}
