using DragonflyTracker.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DragonflyTracker.Contracts.V1.Requests
{
    public class CreateProjectRequest
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public bool Public { get; set; }

        public virtual List<IssueType> Types { get; set; }

        public virtual List<IssueStage> Stages { get; set; }
    }
}
