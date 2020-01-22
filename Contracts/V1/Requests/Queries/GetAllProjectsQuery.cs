using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonflyTracker.Contracts.V1.Requests.Queries
{
    public class GetAllProjectsQuery
    {
        public string CreatorUsername { get; set; }

        public string OrganizationName { get; set; }

        [FromQuery(Name = "search")]
        public string SearchText { get; set; }
    }
}
