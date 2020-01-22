using AutoMapper;
using DragonflyTracker.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonflyTracker.Contracts.V1.Responses
{
    [AutoMap(typeof(IssueUpdate))]
    public class IssueUpdateResponse
    {

        public string Content { get; set; }

        public UpdateType Type { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
