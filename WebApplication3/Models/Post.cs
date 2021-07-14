using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication3.Models
{
    public class Post
    {
        public int Id { get; set; }
        [Required, StringLength(50, MinimumLength = 2), DataType(DataType.Text)]
        public string TripTitle { get; set; }
        [Required, StringLength(50, MinimumLength = 2), DataType(DataType.Text)]
        public string TripDestenation { get; set; }
        [Required, DataType(DataType.MultilineText)]
        public string TripDetails { get; set; }
        public string TripImage { get; set; }
        [Required, RegularExpression(@"[0-9]*\.?[0-9]+", ErrorMessage = "price must be number")]
        public double Price { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime TripDate { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime PostDate { get; set; }
        [DefaultValue(0)]
        public int RankLike { get; set; }
        [DefaultValue(0)]
        public int RankDislike { get; set; }
        public bool Approved { get; set; }
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual ICollection<Comment> comments { get; set; }
        public virtual ICollection<Like> Likes { get; set; }
        public virtual ICollection<SavePost> savePosts { get; set; }
    }
    public class PostComentModel
    {
        public string TripTitle { get; set; }
       
        public string TripDestenation { get; set; }
  
        public string TripDetails { get; set; }

        public string TripImage { get; set; }
      
        public double Price { get; set; }

        public DateTime TripDate { get; set; }
      
        public DateTime PostDate { get; set; }

        public bool  like { get; set; }
        public bool dislike { get; set; }


    }
  
}