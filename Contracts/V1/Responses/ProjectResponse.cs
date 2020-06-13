using AutoMapper;
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
        public DragonflyUserResponse Creator { get; set; }
        public DragonflyUserResponse Owner { get; set; }
        public OrganizationResponse ParentOrganization { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool Private { get; set; }
        public List<IssueTypeResponse> Types { get; set; }
        public List<IssueStageResponse> Stages { get; set; }
        public List<ProjectAdminResponse> Admins { get; set; }
        public List<ProjectMaintainerResponse> Maintainers { get; set; }
    }
}
