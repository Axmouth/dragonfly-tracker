using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonflyTracker.Contracts.V1.Requests.Queries
{
    public class GetAllIssuesQuery
    {
        public string AuthorUsername { get; set; }

        public string ProjectName { get; set; }

        public string OrganizationName { get; set; }

        [FromQuery(Name = "search")]
        public string SearchText { get; set; }
        
        [FromQuery(Name = "open")]
        public bool? Open { get; set; }
    }
}
