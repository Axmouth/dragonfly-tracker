using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DragonflyTracker.Domain
{
    public class ProjectAdmin
    {
        [Required]
        public Guid ProjectId { get; set; }
        public Project Project { get; set; }

        [Required]
        public Guid AdminId { get; set; }
        public DragonflyUser Admin { get; set; }
    }
}
