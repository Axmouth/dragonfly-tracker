using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DragonflyTracker.Contracts.V1;
using DragonflyTracker.Contracts.V1.Requests.Queries;
using DragonflyTracker.Contracts.V1.Responses;
using DragonflyTracker.Domain;
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
            var usersResponse = _mapper.Map<List<UserResponse>>(users.list);
            if (pagination == null || pagination.PageNumber < 1 || pagination.PageSize < 1)
            {
                return Ok(new PagedResponse<UserResponse>(usersResponse, users.count));
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

            return Ok(new Response<UserResponse>(_mapper.Map<UserResponse>(user)));
        }
        // POST: api/v1/users/{username}
        [HttpPost(ApiRoutes.Users.Create)]
        public async Task<ActionResult> Post([FromRoute]string username, [FromBody] string value)
        {
            return NotFound();
        }

        // PUT: api/v1/users/{username}
        [HttpPut(ApiRoutes.Users.Update)]
        public async Task<ActionResult> Put([FromRoute]string username, [FromBody] string value)
        {
            return NotFound();
        }

        // DELETE: api/v1/users/{username}
        [HttpDelete(ApiRoutes.Users.Delete)]
        public async Task<ActionResult> Delete([FromRoute]string username)
        {
            return NotFound();
        }
    }
}
