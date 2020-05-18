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

        [Required]
        [MaxLength(30)]
        [MinLength(2)]
        public string Name { get; set; }


        [MaxLength(350)]
        public string Description { get; set; }

        public bool Private { get; set; }

        [Required]
        [Column(TypeName = "timestamp")]
        public DateTime CreatedAt { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime UpdatedAt { get; set; }

        public Guid? OrganizationId { get; set; }
        [ForeignKey(nameof(OrganizationId))]
        public Organization ParentOrganization { get; set; }

        [Required]
        public Guid CreatorId { get; set; }
        [ForeignKey(nameof(CreatorId))]
        public DragonflyUser Creator { get; set; }

        [Required]
        public Guid OwnerId { get; set; }
        [ForeignKey(nameof(OwnerId))]
        public DragonflyUser Owner { get; set; }

        public virtual List<ProjectAdmin> Admins { get; set; }

        public virtual List<ProjectMaintainer> Maintainers { get; set; }

        public virtual List<Issue> Issues { get; set; }

        public virtual List<IssueType> Types { get; set; }

        public virtual List<IssueStage> Stages { get; set; }
    }
}
