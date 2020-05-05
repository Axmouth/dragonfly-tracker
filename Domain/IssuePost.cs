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

        [Required]
        [MaxLength(1000)]
        [MinLength(3)]
        public string Content { get; set; }

        [Required]
        public int Number { get; set; }

        [Required]
        [Column(TypeName = "timestamp")]
        public DateTime CreatedAt { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime UpdatedAt { get; set; }

        [Required]
        public string AuthorId { get; set; }
        [ForeignKey(nameof(AuthorId))]
        public DragonflyUser Author { get; set; }

        [Required]
        public Guid IssueId { get; set; }
        [ForeignKey(nameof(IssueId))]
        public Issue ParentIssue { get; set; }

        public virtual List<IssuePostReaction> Reactions { get; set; }
    }
}
