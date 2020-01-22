using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonflyTracker.Domain
{
    // [AutoMap(typeof(GetAllIssueTypesQuery))]
    public class GetAllIssueTypesFilter
    {
        public Guid ProjectId { get; set; }

        public string ProjectName { get; set; }

        public string OrganizationId { get; set; }

        public string OrganizationName { get; set; }
    }
}
