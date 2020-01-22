using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DragonflyTracker.Domain
{
    public class ProjectAdmin
    {
        public Guid ProjectId { get; set; }
        public Project Project { get; set; }

        public string AdminId { get; set; }
        public DragonflyUser Admin { get; set; }
    }
}
