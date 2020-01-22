using AutoMapper;
using DragonflyTracker.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonflyTracker.Contracts.V1.Responses
{
    [AutoMap(typeof(IssuePost))]
    public class IssuePostResponse
    {
        public DateTime CreatedAt { get; set; }

        public UserResponse Author { get; set; }

        public IssueResponse ParentIssue { get; set; }

        public virtual List<IssuePostReactionResponse> Reactions { get; set; }
    }
}
