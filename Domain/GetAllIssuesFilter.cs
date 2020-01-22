using AutoMapper;
using AutoMapper.Configuration.Annotations;
using DragonflyTracker.Contracts.V1.Requests.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonflyTracker.Domain
{
    [AutoMap(typeof(GetAllIssuesQuery))]
    public class GetAllIssuesFilter
    {
        [Ignore]
        public Guid? AuthorId { get; set; }

        [Ignore]
        public string AuthorUsername { get; set; }

        [Ignore]
        public string AuthorEmail { get; set; }

        [Ignore]
        public Guid? ProjectId { get; set; }

        [Ignore]
        public string ProjectName { get; set; }

        [Ignore]
        public Guid? OrganizationId { get; set; }

        [Ignore]
        public string OrganizationName { get; set; }

        public string SearchText { get; set; }

        public bool? Open { get; set; }
    }
}
