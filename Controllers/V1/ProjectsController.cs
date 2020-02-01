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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DragonflyTracker.Controllers.V1
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ProjectsController : Controller
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IUriService _uriService;
        private readonly ProjectService _projectService;
        private readonly UserManager<DragonflyUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public ProjectsController(UserManager<DragonflyUser> userManager, RoleManager<IdentityRole> roleManager, DataContext context, ProjectService projectService, IMapper mapper, IUriService uriService)
        {
            _context = context;
            _mapper = mapper;
            _uriService = uriService;
            _projectService = projectService;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: api/Issues
        [AllowAnonymous]
        [HttpGet(ApiRoutes.Projects.GetAllByUser)]
        [Cached(600)]
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
            var projects = await _projectService.GetProjectsAsync(filter, pagination).ConfigureAwait(false);
            var projectsResponse = _mapper.Map<List<ProjectResponse>>(projects.Item1);

            if (pagination == null || pagination.PageNumber < 1 || pagination.PageSize < 1)
            {
                return Ok(new PagedResponse<ProjectResponse>(projectsResponse, projects.Item2));
            }

            var paginationResponse = PaginationHelpers.CreatePaginatedResponse(_uriService, pagination, projectsResponse, projects.Item2);
            return Ok(paginationResponse);
        }

        // GET: api/Issues
        [AllowAnonymous]
        [HttpGet(ApiRoutes.Projects.GetAllByOrg)]
        [Cached(600)]
        public async Task<ActionResult> GetAllProjectsByOrg([FromRoute]string organizationName, [FromQuery] GetAllProjectsQuery query, [FromQuery]PaginationQuery paginationQuery)
        {
            var pagination = _mapper.Map<PaginationFilter>(paginationQuery);
            var filter = _mapper.Map<GetAllProjectsFilter>(query);
            filter.OrganizationName = organizationName;
            var projects = await _projectService.GetProjectsAsync(filter, pagination).ConfigureAwait(false);
            var projectsResponse = _mapper.Map<List<ProjectResponse>>(projects);

            if (pagination == null || pagination.PageNumber < 1 || pagination.PageSize < 1)
            {
                return Ok(new PagedResponse<ProjectResponse>(projectsResponse));
            }

            var paginationResponse = PaginationHelpers.CreatePaginatedResponse(_uriService, pagination, projectsResponse);
            return Ok(paginationResponse);
        }



        // GET: api/Issues/5
        [AllowAnonymous]
        [HttpGet(ApiRoutes.Projects.GetByUser)]
        [Cached(600)]
        public async Task<ActionResult<Issue>> GetIssueByUser([FromRoute]string username, [FromRoute]string projectName)
        {
            var project = await _projectService.GetProjectByUserAsync(username, projectName).ConfigureAwait(false);

            if (project == null)
            {
                return NotFound();
            }

            return Ok(new Response<ProjectResponse>(_mapper.Map<ProjectResponse>(project)));
        }

        // POST: api/Issues
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost(ApiRoutes.Projects.CreateByUser)]
        [Cached(600)]
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

            var newIssueId = Guid.NewGuid();
            var project = new Project
            {
                Id = newIssueId,
                Name = projectRequest.Name,
                Description = projectRequest.Description,
                UserId = user.Id,
                CreatedAt = DateTime.UtcNow,
                ParentOrganization = null,
                Public = projectRequest.Public,
                // Types = issueRequest.Types
            };

            await _projectService.CreateProjectAsync(project, projectRequest.Types).ConfigureAwait(false);

            var locationUri = _uriService.GetPostUri(project.Id.ToString());
            return Created(locationUri, new Response<ProjectResponse>(_mapper.Map<ProjectResponse>(project)));
        }

        // DELETE: api/Issues/5
        [HttpDelete(ApiRoutes.Projects.DeleteByUser)]
        [Cached(600)]
        public async Task<IActionResult> Delete([FromRoute] string username, [FromRoute]string projectName)
        {
            var project = await _projectService.GetProjectByUserAsync(username, projectName).ConfigureAwait(false);

            var userOwnsProject = await _projectService.UserOwnsProjectAsync(project.Id, HttpContext.GetUserId()).ConfigureAwait(false);

            if (!userOwnsProject)
            {
                return BadRequest(new ErrorResponse(new ErrorModel { Message = "You do not own this project" }));
            }

            var deleted = await _projectService.DeleteProjectAsync(project.Id).ConfigureAwait(false);

            if (deleted)
            {
                return NoContent();
            }

            return NotFound();
        }


        // PUT: api/Issues/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut(ApiRoutes.Projects.UpdateByUser)]
        [Cached(600)]
        public async Task<IActionResult> Update([FromRoute] string username, [FromRoute]string projectName, [FromBody] UpdatePostRequest request)
        {
            var project = await _projectService.GetProjectByUserAsync(username, projectName);

            var userOwnsProject = await _projectService.UserOwnsProjectAsync(project.Id, HttpContext.GetUserId()).ConfigureAwait(false);

            if (!userOwnsProject)
            {
                return BadRequest(new ErrorResponse(new ErrorModel { Message = "You do not own this project" }));
            }

            var updatedProject = await _projectService.GetProjectByIdAsync(project.Id).ConfigureAwait(false);
            updatedProject.Name = request.Name;

            var updated = await _projectService.UpdateProjectAsync(updatedProject).ConfigureAwait(false);

            if (updated)
                return Ok(new Response<ProjectResponse>(_mapper.Map<ProjectResponse>(updatedProject)));

            return NotFound();
        }
    }
}