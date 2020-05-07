using AutoMapper;
using DragonflyTracker.Cache;
using DragonflyTracker.Contracts.V1;
using DragonflyTracker.Contracts.V1.Requests.Queries;
using DragonflyTracker.Contracts.V1.Responses;
using DragonflyTracker.Data;
using DragonflyTracker.Domain;
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
        private readonly IIssueService _issueService;
        private readonly IIssuePostService _issuePostService;

        public IssuePostsController(PgMainDataContext context, IIssueService issueService, IIssuePostService issuePostService, IMapper mapper, IUriService uriService)
        {
            _mapper = mapper;
            _uriService = uriService;
            _issueService = issueService;
            _issuePostService = issuePostService;
        }

        [AllowAnonymous]
        [HttpGet(ApiRoutes.IssuePosts.GetAllByOrgProjectIssue)]
        // [Cached(600)]
        public async Task<ActionResult<IEnumerable<IssuePost>>> GetAllIssuePosts([FromRoute]string organizationName, [FromRoute]string projectName, [FromRoute]int issuePostNumber, [FromQuery] GetAllIssuePostsQuery query, [FromQuery]PaginationQuery paginationQuery)
        {
            var pagination = _mapper.Map<PaginationFilter>(paginationQuery);
            var filter = _mapper.Map<GetAllIssuePostsFilter>(query);
            filter.ProjectName = projectName;
            filter.OrganizationName = organizationName;
            filter.IssueNumber = issuePostNumber;
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
