using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DragonflyTracker.Contracts.V1;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DragonflyTracker.Controllers.V1
{
    [ApiController]
    public class AntiForgeryController : ControllerBase
    {
        private IAntiforgery _antiForgery;
        public AntiForgeryController(IAntiforgery antiForgery)
        {
            _antiForgery = antiForgery;
        }

        [HttpGet(ApiRoutes.AntiForgery.Get)]
        public async Task<ActionResult> Get()
        {
            var tokens = _antiForgery.GetAndStoreTokens(HttpContext);
            HttpContext.Response.Cookies.Append(
            "X-XSRF-TOKEN", 
            tokens.RequestToken,
            new CookieOptions
            {
                HttpOnly = false,
                Domain = getCookieDomain(),
                SameSite = SameSiteMode.None,
                Secure = false
            });

            return Ok(new
            {
                token = tokens.RequestToken,
                tokenName = tokens.HeaderName,
                Domain = getCookieDomain()
            });
        }

        private string getCookieDomain()
        {
            return "." + RemoveSubdomain(Request.Host.ToString());
        }

        private string RemoveSubdomain(string host)
        {
            var splitHostname = host.Split('.');
            //if not localhost
            if (splitHostname.Length > 1)
            {
                return string.Join(".", splitHostname.TakeLast(2));
            }
            else
            {
                return host;
            }
        }
    }
}