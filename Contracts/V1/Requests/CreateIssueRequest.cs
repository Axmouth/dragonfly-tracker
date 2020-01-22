using DragonflyTracker.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonflyTracker.Contracts.V1.Requests
{
    public class CreateIssueRequest
    {
        public string Title { get; set; }

        public string PostContent { get; set; }

        public virtual List<IssueType> Types { get; set; }
    }
}
