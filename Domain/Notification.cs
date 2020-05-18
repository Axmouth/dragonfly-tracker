using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DragonflyTracker.Domain
{
    public class Notification
    {
        [Key]
        public Guid Id { get; set; }

        [MaxLength(45)]
        [MinLength(2)]
        public string Name { get; set; }

        [MaxLength(45)]
        [MinLength(7)]
        public string Link { get; set; }

        [Column(TypeName = "Date")]
        public DateTime CreatedAt { get; set; }

        public Guid UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public DragonflyUser Receiver { get; set; }
    }
}
