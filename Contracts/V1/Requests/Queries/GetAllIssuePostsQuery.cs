using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonflyTracker.Contracts.V1.Requests.Queries
{
    public class GetAllIssuePostsQuery
    {
        public string AuthorUsername { get; set; }

        public string ProjectName { get; set; }

        public string OrganizationName { get; set; }

        public int IssueNumber { get; set; }

        [FromQuery(Name = "search")]
        public string SearchText { get; set; }

        public bool? Open { get; set; }

    }
}
