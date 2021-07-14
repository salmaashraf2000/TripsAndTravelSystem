using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication3.Models
{
    public class SavePost
    {
        public int Id { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime SaveDate { get; set; }
        public string  UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        public int PostId { get; set; }
        public virtual Post post { get; set; }
    }
}