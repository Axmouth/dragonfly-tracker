using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DragonflyTracker.Domain
{
    public class IssueIssueType
    {
        [Required]
        public Guid IssueId { get; set; }
        public Issue Issue { get; set; }

        [Required]
        public Guid IssueTypeId { get; set; }
        public IssueType IssueType { get; set; }
    }
}
