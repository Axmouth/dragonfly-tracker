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
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonflyTracker.Controllers.V1
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class IssueStagesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUriService _uriService;
        private readonly IProjectService _projectService;
        private readonly IIssueService _issueService;
        private readonly IIssuePostService _issuePostService;
        private readonly IIssueStageService _issueStageService;

        public IssueStagesController(IIssueService issueService, IIssuePostService issuePostService, IMapper mapper, IUriService uriService, IProjectService projectService, IIssueStageService issueStageService)
        {
            _mapper = mapper;
            _uriService = uriService;
            _projectService = projectService;
            _issueService = issueService;
            _issuePostService = issuePostService;
            _issueStageService = issueStageService;

        }


        [AllowAnonymous]
        // [HttpGet(ApiRoutes.IssueStages.GetAllByUserProject)]
        [HttpGet(ApiRoutes.Users.Projects.IssueStages.GetAll)]
        // [Cached(600)]
        public async Task<IActionResult> GetAllIssueStagesByUserProject([FromRoute] string username, [FromRoute] string projectName,[FromQuery] GetAllIssueStagesQuery query, [FromQuery] PaginationQuery paginationQuery)
        {

            var project = await _projectService.GetProjectByUserAsync(username, projectName).ConfigureAwait(false);

            if (project == null)
            {
                return NotFound();
            }

            var pagination = _mapper.Map<PaginationFilter>(paginationQuery);
            var filter = _mapper.Map<GetAllIssueStagesFilter>(query);
            filter.ProjectId = project.Id;
            var issueStages = await _issueStageService.GetIssueStagesByProjectAsync(project.Id, null).ConfigureAwait(false);
            var issueStagesResponse = _mapper.Map<List<IssueStageResponse>>(issueStages.list);

            if (pagination == null || pagination.PageNumber < 1 || pagination.PageSize < 1)
            {
                return Ok(new PagedResponse<IssueStageResponse>(issueStagesResponse, issueStages.count));
            }

            var paginationResponse = PaginationHelpers.CreatePaginatedResponse(_uriService, pagination, issueStagesResponse, issueStages.count);
            return Ok(paginationResponse);
        }


        [AllowAnonymous]
        // [HttpGet(ApiRoutes.IssueStages.GetByUserProjectIssue)]
        [HttpGet(ApiRoutes.Users.Projects.Issues.IssueStages.Get)]
        // [Cached(600)]
        public async Task<IActionResult> GetIssueStageByUserProjectIssue([FromRoute] string username, [FromRoute] string projectName, [FromRoute] int issueNumber)
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

            // var issueStage = await _issueStageService.GetIssueStageByIssueAsync(issue.Id).ConfigureAwait(false);
            var issueStage = issue.CurrentStage;

            return Ok(new Response<IssueStageResponse>(_mapper.Map<IssueStageResponse>(issueStage)));
        }


        //[HttpPost(ApiRoutes.IssueStages.CreateByUserProjectIssue)]
        [HttpPut(ApiRoutes.Users.Projects.Issues.IssueStages.Update)]
        public async Task<IActionResult> UpdateIssueStageByUserProjectIssue([FromRoute] string username, [FromRoute] string projectName, [FromRoute] int issueNumber, [FromBody] CreateIssueStageRequest issueStageRequest)
        {
            if (issueStageRequest == null)
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

            var issueStage = await _issueStageService.GetIssueStageByIssueAsync(issue.Id).ConfigureAwait(false);

            if (issueStage == null)
            {
                return new NotFoundResult();
            }

            issueStage.Name = issueStageRequest.Name;

            var updated = await _issueStageService.UpdateIssueStageAsync(issueStage).ConfigureAwait(false);

            var locationUri = _uriService.GetUri(issueStage.Name);

            if (updated)
            {
                return Created(locationUri, new Response<IssueStageResponse>(_mapper.Map<IssueStageResponse>(issueStage)));
            }
            return NotFound();
        }

        //[HttpPost(ApiRoutes.IssueStages.CreateByUserProject)]
        [HttpPost(ApiRoutes.Users.Projects.IssueStages.Create)]
        public async Task<IActionResult> CreateIssueStageByUserProject([FromRoute] string username, [FromRoute] string projectName, [FromBody] CreateIssueStageRequest issueStageRequest)
        {
            if (issueStageRequest == null)
            {
                return new BadRequestResult();
            }

            var project = await _projectService.GetProjectByUserAsync(username, projectName).ConfigureAwait(false);

            if (project == null)
            {
                return new NotFoundResult();
            }

            var issueStage = await _issueStageService.GetIssueStageByProjectAsync(project.Id, issueStageRequest.Name).ConfigureAwait(false);

            if (issueStage != null)
            {
                return new ConflictResult();
            }

            var newIssueStage = new IssueStage
            {
                Name = issueStageRequest.Name,
                ProjectId = project.Id,
            };

            var created = await _issueStageService.CreateIssueStageAsync(newIssueStage).ConfigureAwait(false);

            var locationUri = _uriService.GetUri(newIssueStage.Name);

            if (created)
            {
                return Created(locationUri, new Response<IssueStageResponse>(_mapper.Map<IssueStageResponse>(newIssueStage)));
            }
            return NotFound();
        }

        //[HttpPost(ApiRoutes.IssueStages.CreateByUserProject)]
        [HttpPut(ApiRoutes.Users.Projects.IssueStages.Update)]
        public async Task<IActionResult> UpdateIssueStageByUserProject([FromRoute] string username, [FromRoute] string projectName, [FromBody] UpdateIssueStageRequest issueStageRequest)
        {
            if (issueStageRequest == null)
            {
                return new BadRequestResult();
            }

            var project = await _projectService.GetProjectByUserAsync(username, projectName).ConfigureAwait(false);

            if (project == null)
            {
                return new NotFoundResult();
            }

            var issueStage = await _issueStageService.GetIssueStageByProjectAsync(project.Id, issueStageRequest.Name).ConfigureAwait(false);

            if (issueStage == null)
            {
                return new NotFoundResult();
            }

            issueStage.Name = issueStageRequest.Name;

            var updated = await _issueStageService.UpdateIssueStageAsync(issueStage).ConfigureAwait(false);

            var locationUri = _uriService.GetUri(issueStage.Name);

            if (updated)
            {
                return Created(locationUri, new Response<IssueStageResponse>(_mapper.Map<IssueStageResponse>(issueStage)));
            }
            return NotFound();
        }

        // DELETE: api/IssuePosts/5
        [HttpDelete(ApiRoutes.Users.Projects.IssueStages.Delete)]
        public async Task<IActionResult> DeleteIssueStageByUserProject([FromRoute] string username, [FromRoute] string projectName, [FromRoute] int issueNumber, [FromRoute] string issueStageName)
        {
            var project = await _projectService.GetProjectByUserAsync(username, projectName).ConfigureAwait(false);
            if (project == null) {
                return NotFound();
            }

            var issueStage = await _issueStageService.GetIssueStageByProjectAsync(project.Id, issueStageName).ConfigureAwait(false);

            var userId = HttpContext.GetUserId();

            if (userId != project.OwnerId)
            {
                return Unauthorized();
            }

            await _issueStageService.DeleteIssueStageAsync(issueStage.Id).ConfigureAwait(false);

            return NoContent();
        }


    }
}
