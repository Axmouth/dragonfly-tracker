using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DragonflyTracker.Controllers.V1
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUriService _uriService;
        private readonly IProjectService _projectService;
        private readonly UserManager<DragonflyUser> _userManager;

        public ProjectsController(UserManager<DragonflyUser> userManager, IProjectService projectService, IMapper mapper, IUriService uriService)
        {
            _mapper = mapper;
            _uriService = uriService;
            _projectService = projectService;
            _userManager = userManager;
        }

        // GET: api/v1/users/{username}/projects
        [AllowAnonymous]
        [HttpGet(ApiRoutes.Projects.GetAllByUser)]
        // [Cached(600)]
        public async Task<ActionResult> GetAllProjectsByUser([FromRoute]string username, [FromQuery] GetAllProjectsQuery query, [FromQuery]PaginationQuery paginationQuery)
        {
            var user = await _userManager.FindByNameAsync(username).ConfigureAwait(false);
            if (user == null)
            {
                return new NotFoundResult();
            }
            var pagination = _mapper.Map<PaginationFilter>(paginationQuery);
            var filter = _mapper.Map<GetAllProjectsFilter>(query);
            filter.CreatorUsername = username;
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                filter.CurrentUserId = HttpContext.GetUserId();
            }
            var projects = await _projectService.GetProjectsAsync(filter, pagination).ConfigureAwait(false);
            var projectsResponse = _mapper.Map<List<ProjectResponse>>(projects.list);
            if (pagination == null || pagination.PageNumber < 1 || pagination.PageSize < 1)
            {
                return Ok(new PagedResponse<ProjectResponse>(projectsResponse, projects.count));
            }
            var paginationResponse = PaginationHelpers.CreatePaginatedResponse(_uriService, pagination, projectsResponse, projects.count);
            return Ok(paginationResponse);
        }

        // GET: api/api/v1/orgs/{organizationName}/projects
        [AllowAnonymous]
        [HttpGet(ApiRoutes.Projects.GetAllByOrg)]
        public async Task<ActionResult> GetAllProjectsByOrg([FromRoute]string organizationName, [FromQuery] GetAllProjectsQuery query, [FromQuery]PaginationQuery paginationQuery)
        {
            var pagination = _mapper.Map<PaginationFilter>(paginationQuery);
            var filter = _mapper.Map<GetAllProjectsFilter>(query);
            filter.OrganizationName = organizationName;
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                filter.CurrentUserId = HttpContext.GetUserId();
            }
            var projects = await _projectService.GetProjectsAsync(filter, pagination).ConfigureAwait(false);
            var projectsResponse = _mapper.Map<List<ProjectResponse>>(projects.list);
            if (pagination == null || pagination.PageNumber < 1 || pagination.PageSize < 1)
            {
                return Ok(new PagedResponse<ProjectResponse>(projectsResponse, projects.count));
            }
            var paginationResponse = PaginationHelpers.CreatePaginatedResponse(_uriService, pagination, projectsResponse, projects.count);
            return Ok(paginationResponse);
        }

        // GET: api/v1/projects-search
        [AllowAnonymous]
        [HttpGet(ApiRoutes.Projects.GetAll)]
        public async Task<ActionResult> GetAllProjects([FromQuery] GetAllProjectsQuery query, [FromQuery]PaginationQuery paginationQuery)
        {
            var pagination = _mapper.Map<PaginationFilter>(paginationQuery);
            var filter = _mapper.Map<GetAllProjectsFilter>(query);
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                filter.CurrentUserId = HttpContext.GetUserId();
            }
            var projects = await _projectService.GetProjectsAsync(filter, pagination).ConfigureAwait(false);
            var projectsResponse = _mapper.Map<List<ProjectResponse>>(projects.list);
            if (pagination == null || pagination.PageNumber < 1 || pagination.PageSize < 1)
            {
                return Ok(new PagedResponse<ProjectResponse>(projectsResponse, projects.count));
            }
            var paginationResponse = PaginationHelpers.CreatePaginatedResponse(_uriService, pagination, projectsResponse, projects.count);
            return Ok(paginationResponse);
        }



        // GET: api/v1/users/{username}/projects/{projectName}
        [AllowAnonymous]
        [HttpGet(ApiRoutes.Projects.GetByUser)]
        public async Task<ActionResult<Issue>> GetIssueByUser([FromRoute]string username, [FromRoute]string projectName)
        {
            var project = await _projectService.GetProjectByUserAsync(username, projectName).ConfigureAwait(false);

            if (project == null)
            {
                return NotFound();
            }

            return Ok(new Response<ProjectResponse>(_mapper.Map<ProjectResponse>(project)));
        }

        // POST: api/v1/users/{username}/projects
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost(ApiRoutes.Projects.CreateByUser)]
        public async Task<IActionResult> CreateByUser([FromRoute]string username, [FromBody] CreateProjectRequest projectRequest)
        {
            if (projectRequest == null)
            {
                return new BadRequestResult();
            }

            var user = await _userManager
                .FindByNameAsync(username)
                .ConfigureAwait(false);

            if (user.Id != HttpContext.GetUserId())
            {
                return new BadRequestResult();
            }

            var newProjectId = Guid.NewGuid();
            var project = new Project
            {
                Id = newProjectId,
                Name = projectRequest.Name,
                Description = projectRequest.Description,
                CreatorId = user.Id,
                OwnerId = user.Id,
                CreatedAt = DateTime.UtcNow,
                ParentOrganization = null,
                Private = projectRequest.Private,
            };

            await _projectService.CreateProjectAsync(project, projectRequest.Types, projectRequest.Stages, projectRequest.Admins, projectRequest.Maintainers).ConfigureAwait(false);

            var locationUri = _uriService.GetUri(project.Name);
            return Created(locationUri, new Response<ProjectResponse>(_mapper.Map<ProjectResponse>(await _projectService.GetProjectByIdAsync(newProjectId).ConfigureAwait(false))));
        }

        // DELETE: api/v1/users/{username}/projects/{projectName}
        [HttpDelete(ApiRoutes.Projects.DeleteByUser)]
        public async Task<IActionResult> Delete([FromRoute] string username, [FromRoute]string projectName)
        {
            var project = await _projectService.GetProjectByUserAsync(username, projectName).ConfigureAwait(false);

            if (project == null)
            {
                return NotFound();
            }

            var userOwnsProject = HttpContext.GetUserId() == project.Creator.Id; // await _projectService.UserOwnsProjectAsync(project.Id, HttpContext.GetUserId()).ConfigureAwait(false);

            if (!userOwnsProject)
            {
                return Unauthorized(new ErrorResponse(new ErrorModel { Message = "You do not own this project" }));
            }

            var deleted = await _projectService.DeleteProjectAsync(project.Id).ConfigureAwait(false);

            if (deleted)
            {
                return NoContent();
            }

            return NotFound();
        }


        // PUT: api/v1/users/{username}/projects/{projectName}
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut(ApiRoutes.Projects.UpdateByUser)]
        public async Task<IActionResult> Update([FromRoute] string username, [FromRoute]string projectName, [FromBody] UpdateProjectRequest projectRequest)
        {
            var project = await _projectService.GetProjectByUserAsync(username, projectName).ConfigureAwait(false);

            var userOwnsProject = HttpContext.GetUserId() == project.Creator.Id; // await _projectService.UserOwnsProjectAsync(project.Id, HttpContext.GetUserId()).ConfigureAwait(false);

            if (!userOwnsProject)
            {
                return BadRequest(new ErrorResponse(new ErrorModel { Message = "You do not own this project" }));
            }

            if (projectRequest == null)
            {
                return BadRequest();
            }

            var updatedProject = await _projectService.GetProjectByIdAsync(project.Id).ConfigureAwait(false);
            updatedProject.Name = projectRequest.Name;
            updatedProject.Description = projectRequest.Description;
            updatedProject.Private = projectRequest.Private;

            var updated = await _projectService.UpdateProjectAsync(updatedProject, projectRequest.Types, projectRequest.Stages, projectRequest.Admins, projectRequest.Maintainers).ConfigureAwait(false);

            if (updated)
            {
                return Ok(new Response<ProjectResponse>(_mapper.Map<ProjectResponse>(updatedProject))); 
            }
            return NotFound();
        }
    }
}