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
    public class IssueTypesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUriService _uriService;
        private readonly IProjectService _projectService;
        private readonly IIssueService _issueService;
        private readonly IIssueTypeService _issueTypeService;
        public IssueTypesController(IIssueService issueService, IMapper mapper, IUriService uriService, IProjectService projectService, IIssueTypeService issueTypeService)
        {
            _mapper = mapper;
            _uriService = uriService;
            _projectService = projectService;
            _issueService = issueService;
            _issueTypeService = issueTypeService;
        }

        [AllowAnonymous]
        [HttpGet(ApiRoutes.IssueTypes.GetAllByUserProjectIssue)]
        // [Cached(600)]
        public async Task<IActionResult> GetAllIssueTypesByUserProjectIssue([FromRoute] string username, [FromRoute] string projectName, [FromRoute] int issueNumber, [FromRoute] string issueTypeName, [FromQuery] GetAllIssueTypesQuery query, [FromQuery] PaginationQuery paginationQuery)
        {

            var project = await _projectService.GetProjectByUserAsync(username, projectName).ConfigureAwait(false);

            if (project == null)
            {
                return NotFound();
            }

            var issue = await _issueService.GetIssueByUserAsync(username, projectName, issueNumber).ConfigureAwait(false);

            if (issue == null)
            {
                return NotFound();
            }

            var pagination = _mapper.Map<PaginationFilter>(paginationQuery);
            var filter = _mapper.Map<GetAllIssueTypesFilter>(query);
            filter.ProjectId = project.Id;
            filter.IssueId = issue.Id;
            var issueTypes = await _issueTypeService.GetAllIssueTypesByIssueAsync(issue.Id, null).ConfigureAwait(false);
            var issueTypesResponse = _mapper.Map<List<IssueTypeResponse>>(issueTypes.list);

            if (pagination == null || pagination.PageNumber < 1 || pagination.PageSize < 1)
            {
                return Ok(new PagedResponse<IssueTypeResponse>(issueTypesResponse, issueTypes.count));
            }

            var paginationResponse = PaginationHelpers.CreatePaginatedResponse(_uriService, pagination, issueTypesResponse, issueTypes.count);
            return Ok(paginationResponse);
        }

        [AllowAnonymous]
        [HttpGet(ApiRoutes.IssueTypes.GetAllByUserProject)]
        // [Cached(600)]
        public async Task<IActionResult> GetAllIssueTypesByUserProject([FromRoute] string username, [FromRoute] string projectName, [FromRoute] int issueNumber, [FromQuery] GetAllIssueTypesQuery query, [FromQuery] PaginationQuery paginationQuery)
        {

            var project = await _projectService.GetProjectByUserAsync(username, projectName).ConfigureAwait(false);

            if (project == null)
            {
                return NotFound();
            }

            var pagination = _mapper.Map<PaginationFilter>(paginationQuery);
            var filter = _mapper.Map<GetAllIssueTypesFilter>(query);
            filter.ProjectId = project.Id;
            var issueTypes = await _issueTypeService.GetAllIssueTypesByProjectAsync(project.Id, null).ConfigureAwait(false);
            var issueTypesResponse = _mapper.Map<List<IssueTypeResponse>>(issueTypes.list);

            if (pagination == null || pagination.PageNumber < 1 || pagination.PageSize < 1)
            {
                return Ok(new PagedResponse<IssueTypeResponse>(issueTypesResponse, issueTypes.count));
            }

            var paginationResponse = PaginationHelpers.CreatePaginatedResponse(_uriService, pagination, issueTypesResponse, issueTypes.count);
            return Ok(paginationResponse);
        }

        // POST: api/IssueTypes
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost(ApiRoutes.Users.Projects.Issues.IssueTypes.Create)]
        public async Task<IActionResult> CreateIssueTypeByUserProjectIssue([FromRoute] string username, [FromRoute] string projectName, [FromRoute] int issueNumber, [FromBody] CreateIssueTypeRequest issueTypeRequest)
        {
            if (issueTypeRequest == null)
            {
                return new BadRequestResult();
            }

            var project = await _projectService.GetProjectByUserAsync(username, projectName).ConfigureAwait(false);

            if (project == null)
            {
                return new NotFoundResult();
            }

            var issue = await _issueService.GetIssueByUserAsync(username, projectName, issueNumber).ConfigureAwait(false);

            if (issue == null)
            {
                return new NotFoundResult();
            }

            var issueType = await _issueTypeService.GetIssueTypeByProjectAsync(project.Id, issueTypeRequest.Name).ConfigureAwait(false);

            if (issueType == null)
            {
                return new NotFoundResult();
            }

            var issueIssueType = new IssueIssueType
            {
                IssueId = issue.Id,
                IssueTypeId = issueType.Id,
            };

            var created = await _issueTypeService.CreateIssueIssueTypeAsync(issueIssueType).ConfigureAwait(false);

            var locationUri = _uriService.GetUri(issueIssueType.IssueType.Name);

            if (created)
            {
                return Created(locationUri, new Response<IssueIssueTypeResponse>(_mapper.Map<IssueIssueTypeResponse>(issueIssueType)));
            }
            return NotFound();
        }

        // POST: api/IssueTypes
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost(ApiRoutes.Users.Projects.IssueTypes.Create)]
        public async Task<IActionResult> CreateIssueTypeByUserProject([FromRoute] string username, [FromRoute] string projectName, [FromBody] CreateIssueTypeRequest issueTypeRequest)
        {
            if (issueTypeRequest == null)
            {
                return new BadRequestResult();
            }

            var project = await _projectService.GetProjectByUserAsync(username, projectName).ConfigureAwait(false);

            if (project == null)
            {
                return new NotFoundResult();
            }

            var issueType = await _issueTypeService.GetIssueTypeByProjectAsync(project.Id, issueTypeRequest.Name).ConfigureAwait(false);

            if (issueType != null)
            {
                return new ConflictResult();
            }

            var newIssueType = new IssueType
            {
                Name = issueTypeRequest.Name,
                ProjectId = project.Id,
            };

            var created = await _issueTypeService.CreateIssueTypeAsync(newIssueType).ConfigureAwait(false);

            var locationUri = _uriService.GetUri(newIssueType.Name);

            if (created)
            {
                return Created(locationUri, new Response<IssueTypeResponse>(_mapper.Map<IssueTypeResponse>(newIssueType)));
            }
            return NotFound();
        }

        // DELETE: api/IssueTypes/5
        [HttpDelete(ApiRoutes.Users.Projects.IssueTypes.Delete)]
        public async Task<IActionResult> DeleteIssueTypeByUserProject([FromRoute] string username, [FromRoute] string projectName, [FromRoute] string issueTypeName)
        {
            var project = await _projectService.GetProjectByUserAsync(username, projectName).ConfigureAwait(false);

            if (project == null)
            {
                return new NotFoundResult();
            }

            var issueType = await _issueTypeService.GetIssueTypeByProjectAsync(project.Id ,issueTypeName).ConfigureAwait(false);
            if (issueType == null)
            {
                return NotFound();
            }

            var userId = HttpContext.GetUserId();

            if (userId != project.OwnerId)
            {
                return Unauthorized();
            }

            await _issueTypeService.DeleteIssueTypeAsync(issueType.Id).ConfigureAwait(false);

            return NoContent();
        }

        // DELETE: api/IssueTypes/5
        [HttpDelete(ApiRoutes.IssueTypes.DeleteByUserProjectIssue)]
        public async Task<IActionResult> DeleteIssueIssueTypeByUserProjectIssue([FromRoute] string username, [FromRoute] string projectName, [FromRoute] int issueNumber, [FromRoute] string issueTypeName)
        {
            var project = await _projectService.GetProjectByUserAsync(username, projectName).ConfigureAwait(false);

            if (project == null)
            {
                return new NotFoundResult();
            }

            var issue = await _issueService.GetIssueByUserAsync(username, projectName, issueNumber).ConfigureAwait(false);

            if (issue == null)
            {
                return NotFound();
            }

            var issueType = await _issueTypeService.GetIssueTypeByProjectAsync(project.Id, issueTypeName).ConfigureAwait(false);

            if (issueType == null)
            {
                return NotFound();
            }

            var userId = HttpContext.GetUserId();

            if (userId != project.OwnerId)
            {
                return Unauthorized();
            }

            await _issueTypeService.DeleteIssueIssueTypeAsync(issue.Id, issueType.Id).ConfigureAwait(false);

            return NoContent();
        }
    }
}
