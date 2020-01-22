using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonflyTracker.Domain
{
    // [AutoMap(typeof(GetAllUsersQuery))]
    public class GetAllUsersFilter
    {
        public Guid UnderCompanyId { get; set; }

        public string UnderCompanyName { get; set;}

        public Guid MaintainedProjectId { get; set; }

        public string MaintainedProjectName { get; set; }
    }
}
