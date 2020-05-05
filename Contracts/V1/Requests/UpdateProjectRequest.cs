using DragonflyTracker.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonflyTracker.Contracts.V1.Requests
{
    public class UpdateProjectRequest
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public bool Public { get; set; }

        public virtual List<IssueType> Types { get; set; }

        public virtual List<IssueStage> Stages { get; set; }

        public virtual List<DragonflyUser> Admins { get; set; }

        public virtual List<DragonflyUser> Maintainers { get; set; }
    }
}
