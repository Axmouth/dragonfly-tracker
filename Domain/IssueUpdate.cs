using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace DragonflyTracker.Domain
{
    public enum UpdateType
    {
        StatusChange,
        IssueMention,
        Assigned,
    }

    public class IssueUpdate
    {
        [Key]
        public Guid Id { get; set; }

        public string Content { get; set; }

        public UpdateType Type { get; set; }

        public Guid IssueId { get; set; }
        [ForeignKey(nameof(IssueId))]
        public Project ParentIssue { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime CreatedAt { get; set; }

        public Guid OldStageId { get; set; }
        [ForeignKey(nameof(OldStageId))]
        public IssueStage OldStage { get; set; }

        public Guid NewStageId { get; set; }
        [ForeignKey(nameof(NewStageId))]
        public IssueStage NewStage { get; set; }
    }
}
