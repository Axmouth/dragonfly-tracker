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
using System;
using DragonflyTracker.Extensions;
using Org.BouncyCastle.Ocsp;

namespace DragonflyTracker.Controllers.V1
{

    [ApiController]
    public class IdentityController : ControllerBase
    {
        public const string refreshTokenCookieName = "dragonflyAuthRefreshToken";
        private readonly IIdentityService _identityService;
        private readonly IUserService _userService;

        public IdentityController(IIdentityService identityService, IUserService userService)
        {
            _identityService = identityService;
            _userService = userService;
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
            if (request == null)
            {
                return BadRequest();
            }
            var authResponse = await _identityService.RegisterAsync( request.UserName, request.Email, request.Password).ConfigureAwait(false);

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
            if (request == null)
            {
                return BadRequest();
            }
            if (request.UserName == null || request.Password == null)
            {
                return BadRequest();
            }
            var authResponse = await _identityService.LoginAsync(request.UserName, request.Password).ConfigureAwait(false);

            if (!authResponse.Success)
            {
                return Unauthorized(new AuthFailedResponse
                {
                    Errors = authResponse.Errors
                });
            }

            HttpContext.Response.Cookies.Append(
             refreshTokenCookieName,
             authResponse.RefreshToken,
             new CookieOptions
             {
                 HttpOnly = true,
                 //Domain = "localhost"
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
            var RefreshToken = Request.Cookies[refreshTokenCookieName];

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
             refreshTokenCookieName,
             authResponse.RefreshToken,
             new CookieOptions
             {
                 HttpOnly = true,
                 // Domain = ".dragonflytracker.com"
             });

            return Ok(new AuthSuccessResponse
            {
                Token = authResponse.Token,
                RefreshToken = authResponse.RefreshToken
            });
        }

        private string RemoveSubdomain(string host)
        {
            var splitHostname = host.Split('.');
            //if not localhost
            if (splitHostname.Length > 1)
            {
                return string.Join(".", splitHostname.Skip(1));
            }
            else
            {
                return host;
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpDelete(ApiRoutes.Identity.Logout)]
        public async Task<IActionResult> Logout()
        {
            var RefreshToken = Request.Cookies[refreshTokenCookieName];

            var authResponse = await _identityService.LogoutAsync(RefreshToken).ConfigureAwait(false);

            if (!authResponse.Success)
            {
                return Ok(new AuthFailedResponse
                {
                    Errors = authResponse.Errors
                });
            }

            HttpContext.Response.Cookies.Delete(refreshTokenCookieName);

            return NoContent();
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost(ApiRoutes.Identity.PasswordChange)]
        public async Task<IActionResult> PasswordChange([FromBody] PasswordChangeRequest request)
        {
            if (request == null)
            {
                return BadRequest(
                    new ErrorResponse(new ErrorModel { Message = "Empty Request." })
                    );
            }
            var oldUser = await _userService.GetUserAsync(request.UserName).ConfigureAwait(false);
            if (oldUser == null)
            {
                return NotFound(
                    new ErrorResponse(new ErrorModel { Message = "Could not find this User." })
                    );
            }
            var userId = HttpContext.GetUserId();
            var user = await _userService.GetUserByIdAsync(userId).ConfigureAwait(false);
            if (user.UserName != request.UserName)
            {
                return Unauthorized(
                    new ErrorResponse(new ErrorModel { Message = "You are not Authorized to edit this User." })
                    );
            }
            var passCheck = await _identityService.CheckUserPasswordAsync(user, request.OldPassword);
            if (!passCheck)
            {
                return Unauthorized(
                    new ErrorResponse(new ErrorModel { Message = "Wrong Password/User combination." })
                    );
            }
            var passUpdated = await _identityService.UpdatePasswordAsync(user, request.NewPassword).ConfigureAwait(false);
            if (!passUpdated)
            {
                return StatusCode(500,
                        new ErrorResponse(new ErrorModel { Message = "Failed to update Password." })
                        );
            }
            return NotFound();
        }


        [HttpPost(ApiRoutes.Identity.PasswordReset)]
        public async Task<IActionResult> PasswordReset([FromBody] PasswordResetRequest request)
        {
            throw new NotImplementedException();
        }


        [HttpPost(ApiRoutes.Identity.PasswordResetEmail)]
        public async Task<IActionResult> PasswordResetEmail([FromBody] PasswordResetEmailRequest request)
        {
            throw new NotImplementedException();
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost(ApiRoutes.Identity.EmailConfirm)]
        public async Task<IActionResult> EmailConfirm([FromBody] EmailConfirmRequest request)
        {
            var user = await _userService.GetUserByIdAsync(HttpContext.GetUserId()).ConfigureAwait(false);
            var result = await _identityService.ConfirmEmailAsync(user, request.Token).ConfigureAwait(false);
            if (!result)
            {
                return BadRequest(
                    new ErrorResponse(new ErrorModel { Message = "Failed to confirm Email." })
                    );
            }
            return Ok(
                
                );
        }
    }
}