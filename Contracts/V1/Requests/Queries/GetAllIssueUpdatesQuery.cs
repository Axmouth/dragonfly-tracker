using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonflyTracker.Contracts.V1.Requests.Queries
{
    public class GetAllIssueUpdatesQuery
    {
        public string AuthorUsername { get; set; }

        public string ProjectName { get; set; }

        public string OrganizationName { get; set; }

        [FromQuery(Name = "search")]
        public string SearchText { get; set; }

        public int IssueNumber { get; set; }

        [FromQuery(Name = "start")]
        public DateTime start { get; set; }

        [FromQuery(Name = "end")]
        public DateTime end { get; set; }

    }
}
