﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DragonflyTracker.Domain
{
    public class ProjectMaintainer
    {
        [Required]
        public Guid ProjectId { get; set; }
        public virtual Project Project { get; set; }

        [Required]
        public Guid MaintainerId { get; set; }
        public virtual DragonflyUser Maintainer { get; set; }
    }
}
