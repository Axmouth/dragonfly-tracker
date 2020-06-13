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
    public class IssuePostsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUriService _uriService;
        private readonly IProjectService _projectService;
        private readonly IIssueService _issueService;
        private readonly IIssuePostService _issuePostService;

        public IssuePostsController(IIssueService issueService, IIssuePostService issuePostService, IMapper mapper, IUriService uriService, IProjectService projectService)
        {
            _mapper = mapper;
            _uriService = uriService;
            _projectService = projectService;
            _issueService = issueService;
            _issuePostService = issuePostService;
        }

        [AllowAnonymous]
        [HttpGet(ApiRoutes.IssuePosts.GetAllByUserProjectIssue)]
        // [Cached(600)]
        public async Task<IActionResult> GetAllIssuePostsByUserProject([FromRoute] string username, [FromRoute] string projectName, [FromRoute] int issueNumber, [FromQuery] GetAllIssuePostsQuery query, [FromQuery] PaginationQuery paginationQuery)
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
            var filter = _mapper.Map<GetAllIssuePostsFilter>(query);
            filter.ProjectName = projectName;
            filter.AuthorUsername = username;
            filter.IssueNumber = issueNumber;
            filter.IssueId = issue.Id;
            var issuePosts = await _issuePostService.GetAllIssuePostsAsync(filter, pagination).ConfigureAwait(false);
            var issuePostsResponse = _mapper.Map<List<IssuePostResponse>>(issuePosts.list);

            if (pagination == null || pagination.PageNumber < 1 || pagination.PageSize < 1)
            {
                return Ok(new PagedResponse<IssuePostResponse>(issuePostsResponse, issuePosts.count));
            }

            var paginationResponse = PaginationHelpers.CreatePaginatedResponse(_uriService, pagination, issuePostsResponse, issuePosts.count);
            return Ok(paginationResponse);
        }

        [AllowAnonymous]
        [HttpGet(ApiRoutes.IssuePosts.GetAllByOrgProjectIssue)]
        // [Cached(600)]
        public async Task<IActionResult> GetAllIssuePostsByOrgProject([FromRoute]string organizationName, [FromRoute]string projectName, [FromRoute]int issueNumber, [FromQuery] GetAllIssuePostsQuery query, [FromQuery]PaginationQuery paginationQuery)
        {

            var project = await _projectService.GetProjectByOrgAsync(organizationName, projectName).ConfigureAwait(false);

            if (project == null)
            {
                return NotFound();
            }

            var issue = await _issueService.GetIssueByOrgAsync(organizationName, projectName, issueNumber).ConfigureAwait(false);

            if (issue == null)
            {
                return NotFound();
            }

            var pagination = _mapper.Map<PaginationFilter>(paginationQuery);
            var filter = _mapper.Map<GetAllIssuePostsFilter>(query);
            filter.ProjectName = projectName;
            filter.OrganizationName = organizationName;
            filter.IssueNumber = issueNumber;
            filter.IssueId = issue.Id;
            var issuePosts = await _issuePostService.GetAllIssuePostsAsync(filter, pagination).ConfigureAwait(false);
            var issuePostsResponse = _mapper.Map<List<IssuePostResponse>>(issuePosts.list); 

            if (pagination == null || pagination.PageNumber < 1 || pagination.PageSize < 1)
            {
                return Ok(new PagedResponse<IssuePostResponse>(issuePostsResponse, issuePosts.count));
            }

            var paginationResponse = PaginationHelpers.CreatePaginatedResponse(_uriService, pagination, issuePostsResponse, issuePosts.count);
            return Ok(paginationResponse);
        }



        // POST: api/IssuePosts
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost(ApiRoutes.IssuePosts.CreateByUserProjectIssue)]
        public async Task<IActionResult> CreateByUser([FromRoute] string username, [FromRoute] string projectName, [FromRoute] int issueNumber, [FromBody] CreateIssuePostRequest issueRequest)
        {
            if (issueRequest == null)
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

            var newIssuePostId = Guid.NewGuid();
            var issuePost = new IssuePost
            {
                Id = newIssuePostId,
                IssueId = issue.Id,
                AuthorId = HttpContext.GetUserId(),
                CreatedAt = DateTime.UtcNow,
                Content = issueRequest.Content
            };

            var created = await _issuePostService.CreateIssuePostAsync(issuePost).ConfigureAwait(false);

            var locationUri = _uriService.GetUri(issuePost.Number.ToString());

            if (created)
            {
                return Created(locationUri, new Response<IssuePostResponse>(_mapper.Map<IssuePostResponse>(issuePost)));
            }
            return NotFound();
        }

        // PUT: api/Issues/5/IssuePosts/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut(ApiRoutes.IssuePosts.UpdateByUserProjectIssue)]
        public async Task<IActionResult> PostIssue([FromRoute] string username, [FromRoute] string projectName, [FromRoute] int issueNumber, [FromRoute] int issuePostNumber, UpdateIssuePostRequest issuePostRequest)
        {
            if (issuePostRequest == null)
            {
                return BadRequest();
            }

            var issue = await _issueService.GetIssueByUserAsync(username, projectName, issueNumber).ConfigureAwait(false);
            if (issue == null)
            {
                return NotFound();
            }
            var userId = HttpContext.GetUserId();

            if (userId != issue.AuthorId)
            {
                return Unauthorized();
            }

            var issuePost = await _issuePostService.GetIssuePostByIssueIdAsync(issue.Id, issuePostNumber).ConfigureAwait(false);

            issuePost.UpdatedAt = DateTime.UtcNow;
            issuePost.Content = issuePostRequest.Content;

            var updated = await _issuePostService.UpdateIssuePostAsync(issuePost).ConfigureAwait(false);
            var locationUri = _uriService.GetUri(issue.Number.ToString());

            if (updated)
            {
                return Created(locationUri, new Response<IssuePostResponse>(_mapper.Map<IssuePostResponse>(issuePost)));
            }
            return NotFound();
        }

        // DELETE: api/IssuePosts/5
        [HttpDelete(ApiRoutes.Users.Projects.Issues.IssuePosts.Delete)]
        public async Task<IActionResult> DeleteIssuePostByUserProject([FromRoute] string username, [FromRoute] string projectName, [FromRoute] int issueNumber, [FromRoute] int issuePostNumber)
        {
            var issue = await _issueService.GetIssueByUserAsync(username, projectName, issueNumber).ConfigureAwait(false);
            if (issue == null)
            {
                return NotFound();
            }

            var issuePost = await _issuePostService.GetIssuePostByIssueIdAsync(issue.Id, issuePostNumber).ConfigureAwait(false);
            if (issuePost == null)
            {
                return NotFound();
            }

            var userId = HttpContext.GetUserId();

            if (userId != issuePost.AuthorId)
            {
                return Unauthorized();
            }

            await _issuePostService.DeleteIssuePostAsync(issuePost.Id).ConfigureAwait(false);

            return NoContent();
        }
    }
}
