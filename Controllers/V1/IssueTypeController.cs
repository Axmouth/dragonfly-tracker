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
    public class IssueTypeController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUriService _uriService;
        private readonly IProjectService _projectService;
        private readonly IIssueService _issueService;
        private readonly IIssuePostService _issuePostService;
        private readonly IIssueTypeService _issueTypeService;
        public IssueTypeController(IIssueService issueService, IIssuePostService issuePostService, IMapper mapper, IUriService uriService, IProjectService projectService, IIssueTypeService issueTypeService)
        {
            _mapper = mapper;
            _uriService = uriService;
            _projectService = projectService;
            _issueService = issueService;
            _issuePostService = issuePostService;
            _issueTypeService = issueTypeService;
        }



        [AllowAnonymous]
        [HttpGet(ApiRoutes.IssuePosts.GetAllByUserProjectIssue)]
        // [Cached(600)]
        public async Task<IActionResult> GetAllIssueTypesByUserProject([FromRoute] string username, [FromRoute] string projectName, [FromRoute] int issueNumber, [FromRoute] string issueTypeName, [FromQuery] GetAllIssuePostsQuery query, [FromQuery] PaginationQuery paginationQuery)
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
    }
}
