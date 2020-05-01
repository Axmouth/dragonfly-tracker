using DragonflyTracker.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonflyTracker.Contracts.V1.Requests
{
    public class UpdateIssueRequest
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public virtual List<IssueType> Types { get; set; }

        public virtual IssueStage Stage { get; set; }
    }
}
