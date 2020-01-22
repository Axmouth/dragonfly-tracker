using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonflyTracker.Domain
{
    public class ProjectMaintainer
    {
        public Guid ProjectId { get; set; }
        public virtual Project Project { get; set; }

        public string MaintainerId { get; set; }
        public virtual DragonflyUser Maintainer { get; set; }
    }
}
