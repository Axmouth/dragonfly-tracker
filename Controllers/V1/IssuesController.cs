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
        //private readonly PgMainDataContext _pgMainDataContext;
        private readonly IMapper _mapper;
        private readonly IUriService _uriService;
        private readonly IIssueService _issueService;
        private readonly IProjectService _projectService;
        private readonly IUserService _userService;

        public IssuesController(IIssueService issueService, IProjectService projectService, IMapper mapper, IUriService uriService, IUserService userService)
        {
            //_pgMainDataContext = pgMainDataContext;
            _mapper = mapper;
            _uriService = uriService;
            _issueService = issueService;
            _projectService = projectService;
            _userService = userService;
        }

        // GET: api/Issues
        [AllowAnonymous]
        [HttpGet(ApiRoutes.Issues.GetAllByUserProject)]
        // [Cached(600)]
        public async Task<IActionResult> GetAllProjectIssuesByUser([FromRoute]string username, [FromRoute]string projectName, [FromQuery] GetAllIssuesQuery query, [FromQuery]PaginationQuery paginationQuery)
        {
            if (string.IsNullOrEmpty(username))
            {
                return NotFound(new Response<string>()
                {
                    Errors = new List<string> { "No username" }
                });
            }
            var user = await _userService.GetUserByUserNameAsync(username).ConfigureAwait(false);
            if (user == null)
            {
                return NotFound(new Response<string>()
                {
                    Errors = new List<string> { "User not found" }
                });
            }
            var pagination = _mapper.Map<PaginationFilter>(paginationQuery);
            var filter = _mapper.Map<GetAllIssuesFilter>(query);
            var project = await _projectService.GetProjectByUserAsync(username, projectName).ConfigureAwait(false);
            if (project == null)
            {
                return NotFound(new Response<string>()
                {
                    Errors = new List<string> { "Project not found" }
                });
            }
            // filter.ProjectName = projectName;
            // filter.AuthorUsername = username;
            filter.ProjectId = project.Id;
            var issues = await _issueService.GetIssuesAsync(filter, pagination).ConfigureAwait(false);
            var issuesResponse = _mapper.Map<List<IssueResponse>>(issues.list);

            if (pagination == null || pagination.PageNumber < 1 || pagination.PageSize < 1)
            {
                return Ok(new PagedResponse<IssueResponse>(issuesResponse, issues.count));
            }

            var paginationResponse = PaginationHelpers.CreatePaginatedResponse(_uriService, pagination, issuesResponse, issues.count);
            return Ok(paginationResponse);
        }

        [AllowAnonymous]
        [HttpGet(ApiRoutes.Issues.GetAllByOrgProject)]
        // [Cached(600)]
        public async Task<IActionResult> GetAllIssuesByOrg([FromRoute]string organizationName, [FromRoute]string projectName, [FromQuery] GetAllIssuesQuery query, [FromQuery]PaginationQuery paginationQuery)
        {
            var pagination = _mapper.Map<PaginationFilter>(paginationQuery);
            var filter = _mapper.Map<GetAllIssuesFilter>(query);
            filter.ProjectName = projectName;
            filter.OrganizationName = organizationName;
            var issues = await _issueService.GetIssuesAsync(filter, pagination).ConfigureAwait(false);
            var issuesResponse = _mapper.Map<List<IssueResponse>>(issues.list);

            if (pagination == null || pagination.PageNumber < 1 || pagination.PageSize < 1)
            {
                return Ok(new PagedResponse<IssueResponse>(issuesResponse, issues.count));
            }

            var paginationResponse = PaginationHelpers.CreatePaginatedResponse(_uriService, pagination, issuesResponse, issues.count);
            return Ok(paginationResponse);
        }

        // GET: api/Issues/5
        [AllowAnonymous]
        [HttpGet(ApiRoutes.Issues.GetByUserProject)]
        // [Cached(600)]
        public async Task<IActionResult> GetIssueByUser([FromRoute]string username, [FromRoute]string projectName, [FromRoute]int issueNumber)
        {
            var issue = await _issueService.GetIssueByUserAsync(username, projectName, issueNumber).ConfigureAwait(false);

            if (issue == null)
            {
                return NotFound();
            }

            return Ok(new Response<IssueResponse>(_mapper.Map<IssueResponse>(issue)));
        }

        // GET: api/Issues/5
        [AllowAnonymous]
        [HttpGet(ApiRoutes.Issues.GetByOrgProject)]
        // [Cached(600)]
        public async Task<IActionResult> GetIssueByOrgr([FromRoute]string organizationName, [FromRoute]string projectName, [FromRoute]int issueNumber)
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
        [HttpPost(ApiRoutes.Issues.CreateByUserProject)]
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
                CreatedAt = DateTime.UtcNow,
                Content = issueRequest.Content
            };

            await _issueService.CreateIssueByUserAsync(issue, issueRequest.Types, username, projectName).ConfigureAwait(false);

            var locationUri = _uriService.GetUri(issue.Number.ToString());
            return Created(locationUri, new Response<IssueResponse>(_mapper.Map<IssueResponse>(issue)));
        }

        [HttpPost(ApiRoutes.Issues.CreateByOrgProject)]
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
                CreatedAt = DateTime.UtcNow,
                Content = issueRequest.Content,
            };

            var created = await _issueService.CreateIssueByOrgAsync(issue, issueRequest.Types, organizationName, projectName).ConfigureAwait(false);

            if (!created)
            {
                return NotFound();
            }

            var locationUri = _uriService.GetUri(issue.Number.ToString());
            return Created(locationUri, new Response<IssueResponse>(_mapper.Map<IssueResponse>(issue)));
        }

        // PUT: api/Issues/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut(ApiRoutes.Issues.UpdateByUserProject)]
        public async Task<IActionResult> UpdateByUserProject([FromRoute]string username, [FromRoute]string projectName, [FromRoute]int issueNumber, UpdateIssueRequest issueRequest)
        {
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

            issue.Title = issueRequest?.Title;
            issue.UpdatedAt = DateTime.UtcNow;
            issue.Content = issueRequest.Content;

            var updated = await _issueService.UpdateIssueAsync(issue, issueRequest.Types).ConfigureAwait(false);

            if (!updated)
            {
                return NotFound();
            }

            var locationUri = _uriService.GetUri(issue.Number.ToString());
            return Created(locationUri, new Response<IssueResponse>(_mapper.Map<IssueResponse>(issue)));
        }

        // DELETE: api/Issues/5
        [HttpDelete(ApiRoutes.Issues.DeleteByUserProject)]
        public async Task<IActionResult> DeleteIssueByUserProject([FromRoute]string username, [FromRoute]string projectName, [FromRoute]int issueNumber)
        {
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

            // _pgMainDataContext.Issues.Remove(issue);
            //await _pgMainDataContext.SaveChangesAsync().ConfigureAwait(false);
            await _issueService.DeleteIssueAsync(issue.Id).ConfigureAwait(false);

            return NoContent();
        }
    }
}
