using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DragonflyTracker.Domain
{
    public class Project
    {
        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool Public { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime CreatedAt { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime UpdatedAt { get; set; }

        public Guid? OrganizationId { get; set; }
        [ForeignKey(nameof(OrganizationId))]
        public Organization ParentOrganization { get; set; }

        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public DragonflyUser Creator { get; set; }

        public virtual List<ProjectAdmin> Admins { get; set; }

        public virtual List<ProjectMaintainer> Maintainers { get; set; }

        public virtual List<Issue> Issues { get; set; }

        public virtual List<IssueType> Types { get; set; }

        public virtual List<IssueStage> Stages { get; set; }
    }
}
