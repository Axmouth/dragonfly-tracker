using AutoMapper;
using AutoMapper.Configuration.Annotations;
using DragonflyTracker.Contracts.V1.Requests.Queries;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonflyTracker.Domain
{
    [AutoMap(typeof(GetAllUsersQuery))]
    public class GetAllUsersFilter
    {
        [Ignore]
        public Guid UnderOrgId { get; set; }
        public string UnderOrgName { get; set; }
        [Ignore]
        public Guid UnderUserid { get; set; }
        public string UnderUsername { get; set; }
        [Ignore]
        public Guid MaintainedProjectId { get; set; }
        public string MaintainedProjectName { get; set; }
        [Ignore]
        public Guid AdminedProjectId { get; set; }
        public string AdminedProjectName { get; set; }
        [Ignore]
        public Guid OwnedProjectId { get; set; }
        public string OwnedProjectName { get; set; }
        public string Username { get; set; }
        [FromQuery(Name = "search")]
        public string SearchText { get; set; }
    }
}
