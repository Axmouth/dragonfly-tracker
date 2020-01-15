using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DragonflyTracker.Domain
{
    public class IssuePost
    {
        [Key]
        public Guid Id { get; set; }

        public string Content;

        [Column(TypeName = "Date")]
        public DateTime CreatedAt { get; set; }

        public Guid AuthorId { get; set; }
        [ForeignKey(nameof(AuthorId))]
        public IdentityUser Author { get; set; }

        public Guid IssueId { get; set; }
        [ForeignKey(nameof(IssueId))]
        public Issue ParentIssue { get; set; }

        public virtual List<IssuePostReactions> Reactions { get; set; }
    }
}
