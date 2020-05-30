using AutoMapper;
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
    public class IssueStageController
    {
        private readonly IMapper _mapper;
        private readonly IUriService _uriService;
        private readonly IProjectService _projectService;
        private readonly IIssueService _issueService;
        private readonly IIssuePostService _issuePostService;
        private readonly IIssueStageService _issueStageService;

        public IssueStageController(IIssueService issueService, IIssuePostService issuePostService, IMapper mapper, IUriService uriService, IProjectService projectService, IIssueStageService issueStageService)
        {
            _mapper = mapper;
            _uriService = uriService;
            _projectService = projectService;
            _issueService = issueService;
            _issuePostService = issuePostService;
            _issueStageService = issueStageService;

        }
    }
}
