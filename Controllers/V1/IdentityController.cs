using AutoMapper;
using DragonflyTracker.Contracts.V1;
using DragonflyTracker.Contracts.V1.Requests;
using DragonflyTracker.Contracts.V1.Responses;
using DragonflyTracker.Domain;
using DragonflyTracker.Extensions;
using DragonflyTracker.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonflyTracker.Controllers.V1
{

    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class IdentityController : ControllerBase
    {
        public const string refreshTokenCookieName = "dragonflyAuthRefreshToken";
        private readonly IIdentityService _identityService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public IdentityController(IIdentityService identityService, IUserService userService, IMapper mapper)
        {
            _identityService = identityService;
            _userService = userService;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpPost(ApiRoutes.Identity.Register)]
        public async Task<IActionResult> Register([FromBody] UserRegistrationRequest request)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return Conflict(
                new AuthFailedResponse
                {
                    Errors = new List<string>() { "You are already logged in." }
                });
            }
            if (!ModelState.IsValid)
            {
                Console.WriteLine(ModelState.Values.SelectMany(err => err.Errors.Select(errm => errm.ErrorMessage)).ToString());
                return BadRequest(new AuthFailedResponse
                {
                    Errors = ModelState.Values.SelectMany(err => err.Errors.Select(errm => errm.ErrorMessage))
                });
            }
            if (request == null)
            {
                return BadRequest();
            }
            var authResponse = await _identityService.RegisterAsync(request.UserName, request.Email, request.Password).ConfigureAwait(false);

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

        [AllowAnonymous]
        [HttpPost(ApiRoutes.Identity.Login)]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest request)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return Conflict(
                new AuthFailedResponse
                {
                    Errors = new List<string>() { "You are already logged in." }
                });
            }
            if (request == null)
            {
                return BadRequest(
                new AuthFailedResponse
                {
                    Errors = new List<string>() { "Empty Request." }
                }
                    );
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
                 Domain = getCookieDomain(),
                 SameSite = SameSiteMode.None,
                 Secure = false
             });

            return Ok(new AuthSuccessResponse
            {
                Token = authResponse.Token,
                RefreshToken = authResponse.RefreshToken
            });
        }

        [AllowAnonymous]
        [HttpPost(ApiRoutes.Identity.Refresh)]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request)
        {
            if (request == null)
            {
                return BadRequest(
                    new AuthFailedResponse
                    {
                        Errors = new List<string>() { "Empty Request." }
                    }
                );
            }
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
                 Domain = getCookieDomain(),
                 SameSite = SameSiteMode.None,
                 Secure = false
             });

            return Ok(new AuthSuccessResponse
            {
                Token = authResponse.Token,
                RefreshToken = authResponse.RefreshToken
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

        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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

        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet(ApiRoutes.Identity.Profile)]
        public async Task<IActionResult> Profile()
        {
            var user = await _userService.GetUserByIdAsync(HttpContext.GetUserId()).ConfigureAwait(false);

            return Ok(new Response<ProfileResponse>(_mapper.Map<ProfileResponse>(user)));
        }

        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost(ApiRoutes.Identity.PasswordChange)]
        public async Task<IActionResult> PasswordChange([FromBody] PasswordChangeRequest request)
        {
            if (request == null)
            {
                return BadRequest(
                new AuthFailedResponse
                {
                    Errors = new List<string>() { "Empty Request." }
                }
                    );
            }
            var oldUser = await _userService.GetUserByUserNameAsync(request.UserName).ConfigureAwait(false);
            if (oldUser == null)
            {
                return NotFound(
                new AuthFailedResponse
                {
                    Errors = new List<string>() { "Could not find this User." }
                }
               );
            }
            var userId = HttpContext.GetUserId();
            var user = await _userService.GetUserByIdAsync(userId).ConfigureAwait(false);
            if (user.UserName != request.UserName)
            {
                return Unauthorized(
                new AuthFailedResponse
                {
                    Errors = new List<string>() { "You are not Authorized to edit this User." }
                }
                    );
            }
            var passCheck = await _identityService.CheckUserPasswordAsync(user, request.OldPassword).ConfigureAwait(false);
            if (!passCheck.Success)
            {
                return Unauthorized(
                new AuthFailedResponse
                {
                    Errors = passCheck.Errors
                });
            }
            var passUpdated = await _identityService.UpdatePasswordAsync(user, request.NewPassword).ConfigureAwait(false);
            if (!passUpdated.Success)
            {
                return StatusCode(500,
                new AuthFailedResponse
                {
                    Errors = passUpdated.Errors
                });
            }
            return NotFound();
        }


        [AllowAnonymous]
        [HttpPost(ApiRoutes.Identity.PasswordReset)]
        public async Task<IActionResult> PasswordReset([FromBody] PasswordResetRequest request)
        {
            if (request == null)
            {
                return BadRequest(
                new AuthFailedResponse
                {
                    Errors = new List<string>() { "Empty Request." }
                }
                    );
            }
            if (string.IsNullOrEmpty(request.Token))
            {
                return BadRequest(
                new AuthFailedResponse
                {
                    Errors = new List<string>() { "No password change token included." }
                }
                    );
            }
            DragonflyUser user;
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                var userId = HttpContext.GetUserId();
                user = await _userService.GetUserByIdAsync(userId).ConfigureAwait(false);
                if (user != null && ((!string.IsNullOrEmpty(request.Email) && user.Email != request.Email) || (!string.IsNullOrEmpty(request.UserName) && user.UserName != request.UserName)))
                {
                    return BadRequest(
                new AuthFailedResponse
                {
                    Errors = new List<string>() { "Mismatched user data." }
                }
                        );
                }
            }
            else if (!string.IsNullOrEmpty(request.UserName))
            {
                user = await _userService.GetUserByUserNameAsync(request.UserName).ConfigureAwait(false);
            }
            else if (!string.IsNullOrEmpty(request.Email))
            {
                user = await _userService.GetUserByEmailAsync(request.Email).ConfigureAwait(false);
            }
            else
            {
                return BadRequest(
                new AuthFailedResponse
                {
                    Errors = new List<string>() { "No user data included." }
                }
                    );
            }
            if (user == null)
            {
                return BadRequest(
                new AuthFailedResponse
                {
                    Errors = new List<string>() { "Could not find user." }
                }
                    );
            }
            var sentResult = await _identityService.ResetPasswordAsync(user, request.Token, request.NewPassword).ConfigureAwait(false);
            if (!sentResult.Success)
            {
                return BadRequest(
                new AuthFailedResponse
                {
                    Errors = sentResult.Errors
                });
            }
            return Ok(
                new AuthenticationResult
                {
                    Success = true
                });
        }

        [AllowAnonymous]
        [HttpPost(ApiRoutes.Identity.PasswordResetEmail)]
        public async Task<IActionResult> PasswordResetEmail([FromBody] PasswordResetEmailRequest request)
        {
            if (request == null)
            {
                return BadRequest(
                new AuthFailedResponse
                {
                    Errors = new List<string>() { "Empty Request." }
                }
                    );
            }
            DragonflyUser user;
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                var userId = HttpContext.GetUserId();
                user = await _userService.GetUserByIdAsync(userId).ConfigureAwait(false);
                if (user != null && ((!string.IsNullOrEmpty(request.Email) && user.Email != request.Email) || (!string.IsNullOrEmpty(request.UserName) && user.UserName != request.UserName)))
                {
                    return BadRequest(
                new AuthFailedResponse
                {
                    Errors = new List<string>() { "Mismatched user data." }
                }
                        );
                }
            }
            else if (!string.IsNullOrEmpty(request.UserName))
            {
                user = await _userService.GetUserByUserNameAsync(request.UserName).ConfigureAwait(false);
            }
            else if (!string.IsNullOrEmpty(request.Email))
            {
                user = await _userService.GetUserByEmailAsync(request.Email).ConfigureAwait(false);
            }
            else
            {
                return BadRequest(
                new AuthFailedResponse
                {
                    Errors = new List<string>() { "No user data included." }
                }
                    );
            }
            if (user == null)
            {
                return BadRequest(
                new AuthFailedResponse
                {
                    Errors = new List<string>() { "Could not find user." }
                }
                    );
            }
            var sentResult = await _identityService.ResetPasswordEmailAsync(user).ConfigureAwait(false);
            if (!sentResult.Success)
            {
                return BadRequest(
                new AuthFailedResponse
                {
                    Errors = sentResult.Errors
                });
            }
            return Ok(
                new AuthenticationResult
                {
                    Success = true
                });
        }

        [AllowAnonymous]
        [HttpPost(ApiRoutes.Identity.EmailConfirmEmail)]
        public async Task<IActionResult> EmailConfirmEmail([FromBody] EmailConfirmEmailRequest request)
        {
            if (request == null)
            {
                return BadRequest(
                new AuthFailedResponse
                    {
                        Errors = new List<string>() { "Empty Request." }
                    }
                );
            }
            DragonflyUser user;
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                var userId = HttpContext.GetUserId();
                user = await _userService.GetUserByIdAsync(userId).ConfigureAwait(false);
                if (user != null && ((!string.IsNullOrEmpty(request.Email) && user.Email != request.Email) || (!string.IsNullOrEmpty(request.UserName) && user.UserName != request.UserName)))
                {
                    return BadRequest(
                new AuthFailedResponse
                {
                    Errors = new List<string>() { "Mismatched user data." }
                }
                        );
                }
            }
            else if (!string.IsNullOrEmpty(request.UserName))
            {
                user = await _userService.GetUserByUserNameAsync(request.UserName).ConfigureAwait(false);
            }
            else if (!string.IsNullOrEmpty(request.Email))
            {
                user = await _userService.GetUserByEmailAsync(request.Email).ConfigureAwait(false);
            }
            else
            {
                return BadRequest(
                new AuthFailedResponse
                {
                    Errors = new List<string>() { "No user data included." }
                }
                    );
            }
            if (user == null)
            {
                return BadRequest(
                new AuthFailedResponse
                {
                    Errors = new List<string>() { "Could not find user." }
                }
                    );
            }
            var sentResult = await _identityService.SendConfirmationEmailAsync(user).ConfigureAwait(false);
            if (!sentResult.Success)
            {
                return BadRequest(
                new AuthFailedResponse
                {
                    Errors = sentResult.Errors
                });
            }
            return Ok(
                new AuthenticationResult
                {
                    Success = true
                });
        }

        [AllowAnonymous]
        [HttpPost(ApiRoutes.Identity.EmailConfirm)]
        public async Task<IActionResult> EmailConfirm([FromBody] EmailConfirmRequest request)
        {
            if (request == null)
            {
                return BadRequest(
                new AuthFailedResponse
                    {
                        Errors = new List<string>() { "Empty Request." }
                    }
                 );
            }
            DragonflyUser user;
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                user = await _userService.GetUserByIdAsync(HttpContext.GetUserId()).ConfigureAwait(false);
            }
            else if (!string.IsNullOrEmpty(request.UserName))
            {
                user = await _userService.GetUserByUserNameAsync(request.UserName).ConfigureAwait(false);
            }
            else if (!string.IsNullOrEmpty(request.Email))
            {
                user = await _userService.GetUserByEmailAsync(request.Email).ConfigureAwait(false);
            }
            else
            {
                return BadRequest(
                    new AuthFailedResponse
                    {
                        Errors = new List<string>() { "No user data included." }
                    }
               );
            }
            if (user == null)
            {
                return BadRequest(
                    new AuthFailedResponse
                    {
                        Errors = new List<string>() { "Could not find user." }
                    }
                );
            }
            var result = await _identityService.ConfirmEmailAsync(user, request.Token).ConfigureAwait(false);
            if (!result.Success)
            {
                return BadRequest(
                new AuthFailedResponse
                {
                    Errors = result.Errors
                });
            }
            return Ok(
                new AuthenticationResult
                {
                    Success = true
                });
        }
    }
}