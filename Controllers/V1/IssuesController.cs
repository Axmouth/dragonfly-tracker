using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DragonflyTracker.Data;
using DragonflyTracker.Domain;
using DragonflyTracker.Services;
using AutoMapper;
using DragonflyTracker.Contracts.V1.Requests.Queries;
using DragonflyTracker.Contracts.V1.Responses;
using DragonflyTracker.Helpers;
using DragonflyTracker.Contracts.V1;
using DragonflyTracker.Cache;
using DragonflyTracker.Contracts.V1.Requests;
using DragonflyTracker.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Extensions;

namespace DragonflyTracker.Controllers.V1
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class IssuesController : ControllerBase
    {
        private readonly PgMainDataContext _pgMainDataContext;
        private readonly IMapper _mapper;
        private readonly IUriService _uriService;
        private readonly IIssueService _issueService;
        private readonly IProjectService _projectService;

        public IssuesController(PgMainDataContext pgMainDataContext, IIssueService issueService, IProjectService projectService, IMapper mapper, IUriService uriService)
        {
            _pgMainDataContext = pgMainDataContext;
            _mapper = mapper;
            _uriService = uriService;
            _issueService = issueService;
            _projectService = projectService;
        }

        // GET: api/Issues
        [AllowAnonymous]
        [HttpGet(ApiRoutes.Issues.GetAllByUser)]
        // [Cached(600)]
        public async Task<ActionResult> GetAllIssuesByUser([FromRoute]string username, [FromRoute]string projectName, [FromQuery] GetAllIssuesQuery query, [FromQuery]PaginationQuery paginationQuery)
        {
            var pagination = _mapper.Map<PaginationFilter>(paginationQuery);
            var filter = _mapper.Map<GetAllIssuesFilter>(query);
            var project = await _projectService.GetProjectByUserAsync(username, projectName).ConfigureAwait(false);
            // filter.ProjectName = projectName;
            // filter.AuthorUsername = username;
            filter.ProjectId = project.Id;
            var issues = await _issueService.GetIssuesAsync(filter, pagination).ConfigureAwait(false);
            var issuesResponse = _mapper.Map<List<IssueResponse>>(issues.Item1);

            if (pagination == null || pagination.PageNumber < 1 || pagination.PageSize < 1)
            {
                return Ok(new PagedResponse<IssueResponse>(issuesResponse, issues.Item2));
            }

            var paginationResponse = PaginationHelpers.CreatePaginatedResponse(_uriService, pagination, issuesResponse, issues.Item2);
            return Ok(paginationResponse);
        }

        [AllowAnonymous]
        [HttpGet(ApiRoutes.Issues.GetAllByOrg)]
        // [Cached(600)]
        public async Task<ActionResult> GetAllIssuesByOrg([FromRoute]string organizationName, [FromRoute]string projectName, [FromQuery] GetAllIssuesQuery query, [FromQuery]PaginationQuery paginationQuery)
        {
            var pagination = _mapper.Map<PaginationFilter>(paginationQuery);
            var filter = _mapper.Map<GetAllIssuesFilter>(query);
            filter.ProjectName = projectName;
            filter.OrganizationName = organizationName;
            var issues = await _issueService.GetIssuesAsync(filter, pagination).ConfigureAwait(false);
            var issuesResponse = _mapper.Map<List<IssueResponse>>(issues.Item1);

            if (pagination == null || pagination.PageNumber < 1 || pagination.PageSize < 1)
            {
                return Ok(new PagedResponse<IssueResponse>(issuesResponse, issues.Item2));
            }

            var paginationResponse = PaginationHelpers.CreatePaginatedResponse(_uriService, pagination, issuesResponse, issues.Item2);
            return Ok(paginationResponse);
        }

        // GET: api/Issues/5
        [AllowAnonymous]
        [HttpGet(ApiRoutes.Issues.GetByUser)]
        // [Cached(600)]
        public async Task<ActionResult<Issue>> GetIssueByUser([FromRoute]string username, [FromRoute]string projectName, [FromRoute]int issueNumber)
        {
            var issue = await _issueService.GetIssueByUserAsync( username, projectName,issueNumber).ConfigureAwait(false);

            if (issue == null)
            {
                return NotFound();
            }

            return Ok(new Response<IssueResponse>(_mapper.Map<IssueResponse>(issue)));
        }

        // GET: api/Issues/5
        [AllowAnonymous]
        [HttpGet(ApiRoutes.Issues.GetByOrg)]
        // [Cached(600)]
        public async Task<ActionResult<Issue>> GetIssueByOrgr([FromRoute]string organizationName, [FromRoute]string projectName, [FromRoute]int issueNumber)
        {
            var issue = await _issueService.GetIssueByOrgAsync(organizationName, projectName, issueNumber).ConfigureAwait(false);

            if (issue == null)
            {
                return NotFound();
            }

            return Ok(new Response<IssueResponse>(_mapper.Map<IssueResponse>(issue)));
        }

        // POST: api/Issues
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost(ApiRoutes.Issues.CreateByUser)]
        public async Task<IActionResult> CreateByUser([FromRoute]string username, [FromRoute]string projectName, [FromBody] CreateIssueRequest issueRequest)
        {
            if (issueRequest == null)
            {
                return new BadRequestResult();
            }

            var newIssueId = Guid.NewGuid();
            var issue = new Issue
            {
                Id = newIssueId,
                Title = issueRequest.Title,
                AuthorId = HttpContext.GetUserId(),
                CreatedAt = DateTime.UtcNow
                // Types = issueRequest.Types
            };

            await _issueService.CreateIssueByUserAsync(issue, issueRequest.PostContent, issueRequest.Types, username, projectName).ConfigureAwait(false);

            var locationUri = _uriService.GetUri(issue.Number.ToString());
            return Created(locationUri, new Response<IssueResponse>(_mapper.Map<IssueResponse>(issue)));
        }

        [HttpPost(ApiRoutes.Issues.CreateByOrg)]
        public async Task<IActionResult> CreateByOrg([FromRoute]string organizationName, [FromRoute]string projectName, [FromBody] CreateIssueRequest issueRequest)
        {
            if (issueRequest == null)
            {
                return new BadRequestResult();
            }

            var newIssueId = Guid.NewGuid();
            var issue = new Issue
            {
                Id = newIssueId,
                Title = issueRequest.Title,
                AuthorId = HttpContext.GetUserId(),
                CreatedAt = DateTime.UtcNow
                // Types = issueRequest.Types.Select(x => new IssueType { ParentProject.Name = projectName, TagName = x).ToList()
            };

            await _issueService.CreateIssueByOrgAsync(issue, issueRequest.PostContent, issueRequest.Types, organizationName, projectName).ConfigureAwait(false);

            var locationUri = _uriService.GetUri(issue.Number.ToString());
            return Created(locationUri, new Response<PostResponse>(_mapper.Map<PostResponse>(issue)));
        }

        // PUT: api/Issues/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut(ApiRoutes.Issues.UpdateByUser)]
        public async Task<ActionResult<Issue>> PostIssue(Issue issue)
        {
            _pgMainDataContext.Issues.Add(issue);
            await _pgMainDataContext.SaveChangesAsync().ConfigureAwait(false);

            return CreatedAtAction("GetIssue", new { id = issue.Id }, issue);
        }

        // DELETE: api/Issues/5
        [HttpDelete(ApiRoutes.Issues.DeleteByUser)]
        public async Task<ActionResult<Issue>> DeleteIssue(Guid id)
        {
            var issue = await _pgMainDataContext.Issues.FindAsync(id);
            if (issue == null)
            {
                return NotFound();
            }

            _pgMainDataContext.Issues.Remove(issue);
            await _pgMainDataContext.SaveChangesAsync().ConfigureAwait(false);

            return issue;
        }

        private bool IssueExists(Guid id)
        {
            return _pgMainDataContext.Issues.Any(e => e.Id == id);
        }
    }
}
