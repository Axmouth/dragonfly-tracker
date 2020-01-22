using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DragonflyTracker.Domain
{
    public class IssueType
    {
        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public Guid ProjectId { get; set; }
        [ForeignKey(nameof(ProjectId))]
        public Project ParentProject { get; set; }

        public virtual List<IssueIssueType> Issues { get; set; }
    }
}
