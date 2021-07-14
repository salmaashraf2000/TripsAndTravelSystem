using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication3.Models
{
    public class Comment
    {
        public int Id { get; set; }
        [Required]
        public string CommentDescription { get; set; }
        public string Answer { get; set; }

        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        public int PostId { get; set; }
        public virtual Post post { get; set; }
    }
}