using AutoMapper;
using DragonflyTracker.Cache;
using DragonflyTracker.Contracts.V1;
using DragonflyTracker.Contracts.V1.Requests;
using DragonflyTracker.Contracts.V1.Requests.Queries;
using DragonflyTracker.Contracts.V1.Responses;
using DragonflyTracker.Data;
using DragonflyTracker.Domain;
using DragonflyTracker.Extensions;
using DragonflyTracker.Helpers;
using DragonflyTracker.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonflyTracker.Controllers.V1
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class ProjectMaintainersController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUriService _uriService;
        private readonly IProjectService _projectService;
        private readonly IProjectMaintainerService _projectMaintainerService;
        private readonly IUserService _userService;
        public ProjectMaintainersController(IMapper mapper, IUriService uriService, IProjectService projectService, IProjectMaintainerService projectMaintainerService, IUserService userService)
        {
            _mapper = mapper;
            _uriService = uriService;
            _projectService = projectService;
            _projectMaintainerService = projectMaintainerService;
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpGet(ApiRoutes.Users.Projects.ProjectMaintainers.GetAll)]
        // [Cached(600)]
        public async Task<IActionResult> GetAllProjectMaintainersByUserProject([FromRoute] string username, [FromRoute] string projectName, [FromQuery] GetAllProjectMaintainersQuery query, [FromQuery] PaginationQuery paginationQuery)
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
            // var filter = _mapper.Map<GetAllProjectMaintainersFilter>(query);
            var (list, count) = await _projectMaintainerService.GetAllProjectMaintainersAsync(project.Id, pagination).ConfigureAwait(false);
            var maintainersResponse = _mapper.Map<List<DragonflyUserResponse>>(list);

            if (pagination == null || pagination.PageNumber < 1 || pagination.PageSize < 1)
            {
                return Ok(new PagedResponse<DragonflyUserResponse>(maintainersResponse, count));
            }

            var paginationResponse = PaginationHelpers.CreatePaginatedResponse(_uriService, pagination, maintainersResponse, count);
            return Ok(paginationResponse);
        }

        [AllowAnonymous]
        [HttpGet(ApiRoutes.Users.Projects.ProjectMaintainers.Get)]
        // [Cached(600)]
        public async Task<IActionResult> GetProjectMaintainerByUserProject([FromRoute] string username, [FromRoute] string projectName, [FromRoute] string maintainerUserName, [FromQuery] GetAllProjectMaintainersQuery query, [FromQuery] PaginationQuery paginationQuery)
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
            // var filter = _mapper.Map<GetAllProjectMaintainersFilter>(query);
            var maintainer = await _projectMaintainerService.GetProjectMaintainerAsync(project.Id, maintainerUserName).ConfigureAwait(false);
            var maintainerResponse = _mapper.Map<DragonflyUserResponse>(maintainer);
            return Ok(maintainerResponse);
        }

        [HttpPost(ApiRoutes.Users.Projects.ProjectMaintainers.Create)]
        public async Task<IActionResult> CreateProjectMaintainernByUserProjectIssue([FromRoute] string username, [FromRoute] string projectName, [FromBody] CreateProjectMaintainerRequest projectMaintainerRequest)
        {
            if (projectMaintainerRequest == null)
            {
                return new BadRequestResult();
            }

            var project = await _projectService.GetProjectByUserAsync(username, projectName).ConfigureAwait(false);

            if (project == null)
            {
                return new NotFoundResult();
            }

            if (! await _projectService.UserCanAdminProject(project).ConfigureAwait(false))
            {
                return Unauthorized();
            }

            var user = await _userService.GetUserByUserNameAsync(username).ConfigureAwait(false);

            if (user == null)
            {
                return NotFound();
            }
            var maintainer = await _userService.GetUserByUserNameAsync(projectMaintainerRequest.UserName).ConfigureAwait(false);
            if (maintainer == null)
            {
                return NotFound();
            }

            var projectMaintainer = await _projectMaintainerService.GetProjectMaintainerAsync(project.Id, projectMaintainerRequest.UserName).ConfigureAwait(false);

            if (projectMaintainer != null)
            {
                return new ConflictResult();
            }

            var newProjectMaintainer = new ProjectMaintainer
            {
                ProjectId = project.Id,
                MaintainerId = user.Id,
            };

            var created = await _projectMaintainerService.CreateProjectMaintainerAsync(newProjectMaintainer).ConfigureAwait(false);

            var locationUri = _uriService.GetUri(username);

            if (created)
            {
                return Created(locationUri, new Response<DragonflyUserResponse>(_mapper.Map<DragonflyUserResponse>(user)));
            }
            return NotFound();
        }

        [HttpDelete(ApiRoutes.Users.Projects.ProjectMaintainers.Delete)]
        public async Task<IActionResult> DeleteProjectMaintainerByUserProjectIssue([FromRoute] string username, [FromRoute] string projectName, [FromRoute] string maintainerUserName)
        {
            var project = await _projectService.GetProjectByUserAsync(username, projectName).ConfigureAwait(false);

            if (project == null)
            {
                return new NotFoundResult();
            }

            if (!await _projectService.UserCanAdminProject(project).ConfigureAwait(false))
            {
                return Unauthorized();
            }

            var user = await _userService.GetUserByUserNameAsync(username).ConfigureAwait(false);

            if (user == null)
            {
                return NotFound();
            }

            var projectMaintainer = await _projectMaintainerService.GetProjectMaintainerAsync(project.Id, maintainerUserName).ConfigureAwait(false);

            if (projectMaintainer == null)
            {
                return new NotFoundResult();
            }

            var maintainer = await _userService.GetUserByUserNameAsync(maintainerUserName).ConfigureAwait(false);
            if (maintainer == null)
            {
                return NotFound();
            }

            var deleted = await _projectMaintainerService.DeleteProjectMaintainerAsync(project.Id, maintainer.Id).ConfigureAwait(false);

            if (deleted)
            {
                return NoContent();
            }
            return NotFound();
        }
    }
}
