using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonflyTracker.Contracts.V1.Requests.Queries
{
    public class GetAllUsersQuery
    {
        public string Username { get; set; }
        [FromQuery(Name = "search")]
        public string SearchText { get; set; }
        [FromQuery(Name = "underorg")]
        public string UnderOrgName { get; set; }
        [FromQuery(Name = "underuser")]
        public string UnderUsername { get; set; }
        [FromQuery(Name = "maintainedproject")]
        public string MaintainedProjectName { get; set; }
        [FromQuery(Name = "adminedproject")]
        public string AdminedProjectName { get; set; }
        [FromQuery(Name = "ownedproject")]
        public string OwnedProjectName { get; set; }
    }
}
