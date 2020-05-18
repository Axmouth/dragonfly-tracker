using AutoMapper;
using AutoMapper.Configuration.Annotations;
using DragonflyTracker.Contracts.V1.Requests.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonflyTracker.Domain
{
    [AutoMap(typeof(GetAllProjectsQuery))]
    public class GetAllProjectsFilter
    {
        [Ignore]
        public Guid OrganizationId { get; set; }

        public string OrganizationName { get; set; }

        [Ignore]
        public Guid AuthorId { get; set; }

        public string CreatorUsername { get; set; }

        [Ignore]
        public string AuthorEmail { get; set; }

        [Ignore]
        public Guid CurrentUserId { get; set; }

        public string SearchText { get; set; }
        public string Admin { get; set; }
        public string Maintainer { get; set; }
        public bool? Admined { get; set; }
        public bool? Maintained { get; set; }
    }
}
