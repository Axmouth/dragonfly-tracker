using AutoMapper;
using AutoMapper.Configuration.Annotations;
using DragonflyTracker.Contracts.V1.Requests.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonflyTracker.Domain
{
    [AutoMap(typeof(GetAllIssueTypesQuery))]
    public class GetAllIssueTypesFilter
    {
        [Ignore]
        public Guid IssueId { get; set; }
        [Ignore]
        public Guid ProjectId { get; set; }
        [Ignore]
        public string ProjectName { get; set; }
        [Ignore]
        public string OrganizationId { get; set; }
        [Ignore]
        public string OrganizationName { get; set; }
    }
}
