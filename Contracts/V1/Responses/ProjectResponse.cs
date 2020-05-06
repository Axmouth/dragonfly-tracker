﻿using AutoMapper;
using DragonflyTracker.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonflyTracker.Contracts.V1.Responses
{
    [AutoMap(typeof(Project))]
    public class ProjectResponse
    {
        public UserResponse Creator { get; set; }
        public OrganizationResponse ParentOrganization { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<IssueTypeResponse> Types { get; set; }
        public List<IssueStageResponse> Stages { get; set; }
        public List<ProjectAdmin> Admins { get; set; }
        public List<ProjectMaintainer> Maintainers { get; set; }
    }
}
