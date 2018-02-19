using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace OktaWebApp.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet, Route("account/login")]
        public IActionResult Login(string returnUrl)
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return Challenge(OpenIdConnectDefaults.AuthenticationScheme);
            }

            return RedirectToActionPermanent("Get", "Document",
                new
                {
                    documentId = returnUrl.Split('/', StringSplitOptions.RemoveEmptyEntries).Skip(2).FirstOrDefault() ?? string.Empty
                });
        }

        [HttpPost]
        public void Logout()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                SignOut(CookieAuthenticationDefaults.AuthenticationScheme, OpenIdConnectDefaults.AuthenticationScheme);
            }
        }
    }
}
