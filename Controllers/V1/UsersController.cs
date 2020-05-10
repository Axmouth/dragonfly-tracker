using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DragonflyTracker.Contracts.V1;
using DragonflyTracker.Contracts.V1.Requests;
using DragonflyTracker.Contracts.V1.Requests.Queries;
using DragonflyTracker.Contracts.V1.Responses;
using DragonflyTracker.Domain;
using DragonflyTracker.Extensions;
using DragonflyTracker.Helpers;
using DragonflyTracker.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DragonflyTracker.Controllers.V1
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IUriService _uriService;
        public UsersController(IUserService userService, IMapper mapper, IUriService uriService)
        {
            _userService = userService;
            _mapper = mapper;
            _uriService = uriService;
        }

        // GET: api/v1/users
        [AllowAnonymous]
        [HttpGet(ApiRoutes.Users.GetAll)]
        public async Task<ActionResult> GetAll([FromQuery] GetAllUsersQuery query, [FromQuery]PaginationQuery paginationQuery)
        {
            var pagination = _mapper.Map<PaginationFilter>(paginationQuery);
            var filter = _mapper.Map<GetAllUsersFilter>(query);
            var users = await _userService.GetUsersAsync(filter, pagination).ConfigureAwait(false);
            var usersResponse = _mapper.Map<List<DragonflyUserResponse>>(users.list);
            if (pagination == null || pagination.PageNumber < 1 || pagination.PageSize < 1)
            {
                return Ok(new PagedResponse<DragonflyUserResponse>(usersResponse, users.count));
            }
            var paginationResponse = PaginationHelpers.CreatePaginatedResponse(_uriService, pagination, usersResponse, users.count);
            return Ok(paginationResponse);
        }

        // GET: api/v1/users/{username}
        [AllowAnonymous]
        [HttpGet(ApiRoutes.Users.Get)]
        public async Task<ActionResult> Get([FromRoute]string username)
        {
            var user = await _userService.GetUserAsync(username).ConfigureAwait(false);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(new Response<DragonflyUserResponse>(_mapper.Map<DragonflyUserResponse>(user)));
        }
        // POST: api/v1/users/{username}
        [HttpPost(ApiRoutes.Users.Create)]
        public async Task<ActionResult> Post([FromRoute]string username, [FromBody] string value)
        {
            return NotFound();
        }

        // PUT: api/v1/users/{username}
        [HttpPut(ApiRoutes.Users.Update)]
        public async Task<ActionResult> Put([FromRoute]string username, [FromBody] UpdateUserRequest request)
        {
            if (request == null)
            {
                return BadRequest(
                    new ErrorResponse(new ErrorModel { Message = "Empty Request." })
                    );
            }
            var oldUser = await _userService.GetUserAsync(username).ConfigureAwait(false);
            if (oldUser == null)
            {
                return NotFound(
                    new ErrorResponse(new ErrorModel { Message = "Could not find this User." })
                    );
            }

            var userId = HttpContext.GetUserId();
            var user = await _userService.GetUserByIdAsync(userId).ConfigureAwait(false);
            if (user.UserName != username)
            {
                return Unauthorized(
                    new ErrorResponse(new ErrorModel { Message = "You are not Authorized to edit this User." })
                    );
            }
            var usernameOwned = _userService.GetUserAsync(request.UserName);
            if (usernameOwned == null)
            {
                user.UserName = request.UserName;
            }
            else
            {
                return Conflict(
                    new ErrorResponse(new ErrorModel { Message = "The attempted Username is taken." })
                    );
            }
            user.Title = request.Title;
            user.Description = request.Description;
            if (!string.IsNullOrEmpty(request.Email))
            {
                var mailUpdated = await _userService.UpdateEmailAsync(user, user.Email, request.Email).ConfigureAwait(false);
                if (!mailUpdated)
                {
                    return StatusCode(500,
                            new ErrorResponse(new ErrorModel { Message = "Failed to update Email." })
                            );
                }
            }/*
            if ( !string.IsNullOrEmpty(request.Password))
            {
                var passUpdated = await _userService.UpdatePasswordAsync(user, request.Password).ConfigureAwait(false);
                if (!passUpdated)
                {
                    return StatusCode(500,
                            new ErrorResponse(new ErrorModel { Message = "Failed to update Password." })
                            );
                }
            }*/
            var updated = await _userService.UpdateUserAsync(user).ConfigureAwait(false);
            if (updated)
            {
                Ok(
                    new Response<DragonflyUserResponse>(_mapper.Map<DragonflyUserResponse>(user))
                    );
            }
            return StatusCode(500,
                    new ErrorResponse(new ErrorModel { Message = "Something went wrong." })
                    );
        }

        // DELETE: api/v1/users/{username}
        [HttpDelete(ApiRoutes.Users.Delete)]
        public async Task<ActionResult> Delete([FromRoute]string username)
        {
            var userId = HttpContext.GetUserId();
            var user = await _userService.GetUserByIdAsync(userId).ConfigureAwait(false);
            if (user.UserName != username)
            {
                return Unauthorized();
            }
            return NotFound();
        }
    }
}
