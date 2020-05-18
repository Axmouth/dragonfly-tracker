using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace DragonflyTracker.Domain
{
    public class Post
    {
        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public Guid UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public DragonflyUser User { get; set; }

        public virtual List<PostTag> Tags { get; set; }
    }
}