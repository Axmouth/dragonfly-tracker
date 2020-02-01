using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DragonflyTracker.Contracts.V1;
using DragonflyTracker.Contracts.V1.Requests;
using DragonflyTracker.Contracts.V1.Responses;
using DragonflyTracker.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;

namespace DragonflyTracker.Controllers.V1
{
    public class IdentityController : Controller
    {
        private readonly IIdentityService _identityService;
        
        public IdentityController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        [HttpPost(ApiRoutes.Identity.Register)]
        public async Task<IActionResult> Register([FromBody] UserRegistrationRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new AuthFailedResponse
                {
                    Errors = ModelState.Values.SelectMany(x => x.Errors.Select(xx => xx.ErrorMessage))
                });
            }
            
            var authResponse = await _identityService.RegisterAsync(request.Email, request.Password).ConfigureAwait(false);

            if (!authResponse.Success)
            {
                return BadRequest(new AuthFailedResponse
                {
                    Errors = authResponse.Errors
                });
            }
            
            return Ok(new AuthSuccessResponse
            {
                Token = authResponse.Token,
                RefreshToken = authResponse.RefreshToken
            });
        }
        
        [HttpPost(ApiRoutes.Identity.Login)]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest request)
        {
            var authResponse = await _identityService.LoginAsync(request.Email, request.Password).ConfigureAwait(false);

            if (!authResponse.Success)
            {
                return BadRequest(new AuthFailedResponse
                {
                    Errors = authResponse.Errors
                });
            }

            HttpContext.Response.Cookies.Append(
             "authRefreshToken",
             authResponse.RefreshToken,
             new CookieOptions
             {
                 HttpOnly = true
             });

            return Ok(new AuthSuccessResponse
            {
                Token = authResponse.Token,
                RefreshToken = authResponse.RefreshToken
            });
        }
        
        [HttpPost(ApiRoutes.Identity.Refresh)]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request)
        {
            var RefreshToken = Request.Cookies["authRefreshToken"];

            // var authResponse = await _identityService.RefreshTokenAsync(request.Token, request.RefreshToken).ConfigureAwait(false);
            var authResponse = await _identityService.RefreshTokenAsync(request.Token, RefreshToken).ConfigureAwait(false);

            if (!authResponse.Success)
            {
                return BadRequest(new AuthFailedResponse
                {
                    Errors = authResponse.Errors
                });
            }

            HttpContext.Response.Cookies.Append(
             "authRefreshToken",
             authResponse.RefreshToken,
             new CookieOptions
             {
                 HttpOnly = true
             });

            return Ok(new AuthSuccessResponse
            {
                Token = authResponse.Token,
                RefreshToken = authResponse.RefreshToken
            });
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpDelete(ApiRoutes.Identity.Logout)]
        public async Task<IActionResult> Logout()
        {
            var RefreshToken = Request.Cookies["authRefreshToken"];

            var authResponse = await _identityService.LogoutAsync(RefreshToken).ConfigureAwait(false);

            if (!authResponse.Success)
            {
                return BadRequest(new AuthFailedResponse
                {
                    Errors = authResponse.Errors
                });
            }

            HttpContext.Response.Cookies.Append(
             "authRefreshToken",
             "derp",
             new CookieOptions
             {
                 HttpOnly = true
             });

            return NoContent();
        }
    }
}