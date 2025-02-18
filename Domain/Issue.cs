﻿using Microsoft.AspNetCore.Identity;
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
        List<string> defaultIssuesTypes = new List<string>(new string[] { "Question", "Bug", "Feature Request" });
        List<string> defaultIssuesStages = new List<string>(new string[] { "Open", "Being Worked On", "Closed" });

        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(150)]
        [MinLength(3)]
        public string Title { get; set; }

        [Required]
        [MaxLength(2500)]
        [MinLength(15)]
        [Column(TypeName = "text")]
        public string Content { get; set; }

        [Required]
        public int Number { get; set; }

        public bool Open { get; set; }

        [Required]
        [Column(TypeName = "timestamp")]
        public DateTime CreatedAt { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime UpdatedAt { get; set; }

        [Required]
        public Guid AuthorId { get; set; }
        [ForeignKey(nameof(AuthorId))]
        public DragonflyUser Author { get; set; }

        [Required]
        public Guid ProjectId { get; set; }
        [ForeignKey(nameof(ProjectId))]
        public Project ParentProject { get; set; }

        public Guid? OrganizationId { get; set; }
        [ForeignKey(nameof(OrganizationId))]
        public Organization ParentOrganization { get; set; }

        public Guid? StageId { get; set; }
        [ForeignKey(nameof(StageId))]
        public virtual IssueStage CurrentStage { get; set; }

        public virtual List<IssueIssueType> Types { get; set; }

        public virtual List<IssuePost> Posts { get; set; }

        public virtual List<IssueUpdate> Updates { get; set; }

    }
}
