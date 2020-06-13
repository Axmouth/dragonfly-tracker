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
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DragonflyTracker.Controllers.V1
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class ProjectAdminsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUriService _uriService;
        private readonly IProjectService _projectService;
        private readonly IProjectAdminService _projectAdminService;
        private readonly IUserService _userService;
        public ProjectAdminsController(IMapper mapper, IUriService uriService, IProjectService projectService, IProjectAdminService projectAdminService, IUserService userService)
        {
            _mapper = mapper;
            _uriService = uriService;
            _projectService = projectService;
            _projectAdminService = projectAdminService;
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpGet(ApiRoutes.Users.Projects.ProjectAdmins.GetAll)]
        // [Cached(600)]
        public async Task<IActionResult> GetAllProjectAdminsByUserProject([FromRoute] string username, [FromRoute] string projectName, [FromQuery] GetAllProjectAdminsQuery query, [FromQuery] PaginationQuery paginationQuery)
        {
            var user = await _userService.GetUserByUserNameAsync(username).ConfigureAwait(false);

            if (user == null)
            {
                return NotFound();
            }

            var project = await _projectService.GetProjectByUserAsync(username, projectName).ConfigureAwait(false);

            if (project == null)
            {
                return NotFound();
            }

            var pagination = _mapper.Map<PaginationFilter>(paginationQuery);
            // var filter = _mapper.Map<GetAllProjectAdminsFilter>(query);
            var (list, count) = await _projectAdminService.GetAllProjectAdminsAsync(project.Id, pagination).ConfigureAwait(false);
            var adminsResponse = _mapper.Map<List<DragonflyUserResponse>>(list);

            if (pagination == null || pagination.PageNumber < 1 || pagination.PageSize < 1)
            {
                return Ok(new PagedResponse<DragonflyUserResponse>(adminsResponse, count));
            }

            var paginationResponse = PaginationHelpers.CreatePaginatedResponse(_uriService, pagination, adminsResponse, count);
            return Ok(paginationResponse);
        }

        [AllowAnonymous]
        [HttpGet(ApiRoutes.Users.Projects.ProjectAdmins.Get)]
        // [Cached(600)]
        public async Task<IActionResult> GetProjectAdminByUserProject([FromRoute] string username, [FromRoute] string projectName, [FromRoute] string adminUserName, [FromQuery] GetAllProjectAdminsQuery query, [FromQuery] PaginationQuery paginationQuery)
        {
            var user = await _userService.GetUserByUserNameAsync(username).ConfigureAwait(false);

            if (user == null)
            {
                return NotFound();
            }

            var project = await _projectService.GetProjectByUserAsync(username, projectName).ConfigureAwait(false);

            if (project == null)
            {
                return NotFound();
            }

            var pagination = _mapper.Map<PaginationFilter>(paginationQuery);
            // var filter = _mapper.Map<GetAllProjectAdminsFilter>(query);
            var admin = await _projectAdminService.GetProjectAdminAsync(project.Id, adminUserName).ConfigureAwait(false);
            var adminsResponse = _mapper.Map<DragonflyUserResponse>(admin);
            return Ok(adminsResponse);
        }

        [HttpPost(ApiRoutes.Users.Projects.ProjectAdmins.Create)]
        public async Task<IActionResult> CreateProjectAdminByUserProjectIssue([FromRoute] string username, [FromRoute] string projectName, [FromBody] CreateProjectAdminRequest projectAdminRequest)
        {
            if (projectAdminRequest == null)
            {
                return new BadRequestResult();
            }

            var project = await _projectService.GetProjectByUserAsync(username, projectName).ConfigureAwait(false);

            if (project == null)
            {
                return new NotFoundResult();
            }

            if (HttpContext.GetUserId() != project.OwnerId) {
                return Unauthorized();
            }

            var user = await _userService.GetUserByUserNameAsync(username).ConfigureAwait(false);

            if (user == null)
            {
                return NotFound();
            }
            var admin = await _userService.GetUserByUserNameAsync(projectAdminRequest.UserName).ConfigureAwait(false);
            if (admin == null)
            {
                return NotFound();
            }

            var projectAdmin = await _projectAdminService.GetProjectAdminAsync(project.Id, projectAdminRequest.UserName).ConfigureAwait(false);

            if (projectAdmin != null)
            {
                return new ConflictResult();
            }

            var newProjectAdmin = new ProjectAdmin
            {
                ProjectId = project.Id,
                AdminId = user.Id,
            };

            var created = await _projectAdminService.CreateProjectAdminAsync(newProjectAdmin).ConfigureAwait(false);

            var locationUri = _uriService.GetUri(username);

            if (created)
            {
                return Created(locationUri, new Response<DragonflyUserResponse>(_mapper.Map<DragonflyUserResponse>(user)));
            }
            return NotFound();
        }

        [HttpDelete(ApiRoutes.Users.Projects.ProjectAdmins.Delete)]
        public async Task<IActionResult> DeleteProjectAdminByUserProjectIssue([FromRoute] string username, [FromRoute] string projectName, [FromRoute] string adminUserName)
        {
            var project = await _projectService.GetProjectByUserAsync(username, projectName).ConfigureAwait(false);

            if (project == null)
            {
                return new NotFoundResult();
            }

            if (HttpContext.GetUserId() != project.OwnerId)
            {
                return Unauthorized();
            }

            var user = await _userService.GetUserByUserNameAsync(username).ConfigureAwait(false);

            if (user == null)
            {
                return NotFound();
            }

            var projectAdmin = await _projectAdminService.GetProjectAdminAsync(project.Id, adminUserName).ConfigureAwait(false);

            if (projectAdmin == null)
            {
                return new NotFoundResult();
            }

            var admin = await _userService.GetUserByUserNameAsync(adminUserName).ConfigureAwait(false);
            if (admin == null)
            {
                return NotFound();
            }

            var deleted = await _projectAdminService.DeleteProjectAdminAsync(project.Id, admin.Id).ConfigureAwait(false);

            if (deleted)
            {
                return NoContent();
            }
            return NotFound();
        }
    }
}
