using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace DragonflyTracker.Domain
{
    public class Tag
    {
        [Key]
        public string Name { get; set; }

        public Guid CreatorId { get; set; }
        
        [ForeignKey(nameof(CreatorId))]
        public DragonflyUser CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}