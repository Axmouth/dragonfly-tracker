using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonflyTracker.Domain
{
    public class IssueIssueType
    {
        public Guid IssueId { get; set; }
        public Issue Issue { get; set; }

        public Guid IssueTypeId { get; set; }
        public IssueType IssueType { get; set; }
    }
}
