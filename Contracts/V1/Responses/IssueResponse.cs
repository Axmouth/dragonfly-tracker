using AutoMapper;
using AutoMapper.Configuration.Annotations;
using DragonflyTracker.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonflyTracker.Contracts.V1.Responses
{
    [AutoMap(typeof(Issue))]
    public class IssueResponse
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public int Number { get; set; }
        public bool Open { get; set; }
        public DateTime CreatedAt { get; set; }
        public UserResponse Author { get; set; }
        public ProjectResponse ParentProject { get; set; }
        public IssueStageResponse CurrentStage { get; set; }
        [Ignore]
        public virtual List<IssueTypeResponse> Types { get;  }
        public virtual List<IssueUpdateResponse> Updates { get;  }
    }
}
