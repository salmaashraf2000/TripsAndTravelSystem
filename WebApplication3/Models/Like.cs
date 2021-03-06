using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication3.Models
{
    public class Like
    {
        public int Id { get; set; }
        [Required]
        public bool like { get; set; }
        [Required]
        public bool Dislike { get; set; }

        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        public int PostId { get; set; }
        public virtual Post post { get; set; }

    }
}