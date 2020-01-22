using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonflyTracker.Domain
{
    //[AutoMap(typeof(GetAllIssuePostReactionsQuery))]
    public class GetAllIssuePostReactionsFilter
    {
        public Guid IssueId { get; set; }

        public string IssueName { get; set; }

        public Guid IssuePostId { get; set; }

        public Guid AuthorId { get; set; }

        public string AuthorUsername { get; set; }

        public string AuthorEmail { get; set; }
    }
}
