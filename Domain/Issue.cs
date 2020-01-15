using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace DragonflyTracker.Domain
{
    public class Issue
    {
        public List<string> defaultIssuesTypes = new List<string>(new string[] { "Question", "Bug", "Feature Request" });
        public List<string> defaultIssuesStages = new List<string>(new string[] { "Open", "Being Worked On", "Closed" });

        [Key]
        public Guid Id { get; set; }

        public string Title { get; set; }

        public int Number { get; set; }

        [Column(TypeName = "Date")]
        public DateTime CreatedAt { get; set; }

        public Guid AuthorId { get; set; }
        [ForeignKey(nameof(AuthorId))]
        public IdentityUser Author { get; set; }

        public Guid ProjectId { get; set; }
        [ForeignKey(nameof(ProjectId))]
        public Project ParentProject { get; set; }

        public Guid CompanyId { get; set; }
        [ForeignKey(nameof(CompanyId))]
        public Project ParentCompany { get; set; }

        public Guid StageId { get; set; }
        [ForeignKey(nameof(StageId))]
        public IssueStage CurrentStage { get; set; }

        public virtual List<IssueType> Types { get; set; }

        public virtual List<IssuePost> Posts { get; set; }

        public virtual List<IssueUpdate> Updates { get; set; }

    }
}
