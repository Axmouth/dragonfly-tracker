using AutoMapper;
using DragonflyTracker.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonflyTracker.Contracts.V1.Responses
{
    [AutoMap(typeof(IssuePostReaction))]
    public class IssuePostReactionResponse
    {
        public string Name { get; set; }
        public DragonflyUserResponse Creator { get; set; }
    }
}
