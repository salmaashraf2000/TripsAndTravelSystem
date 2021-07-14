using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication3.Models;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace WebApplication3.Controllers
{
   
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("TripsAgencey"))
                {
                    return RedirectToAction("index", "TripAgency");
                }
                if (User.IsInRole("Admin"))
                {
                    return RedirectToAction("index", "Admin");
                }
                if (User.IsInRole("Traveller"))
                {
                    return RedirectToAction("index", "Traveler");
                }
            }
            var Posts = db.Posts.Where(a => a.Approved == true).ToList();
            ViewBag.p = Posts;
            return View();
        }
        [Authorize(Roles = "Admin")]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            var UserID = User.Identity.GetUserId();
            var posts = from p in db.Posts
                        join l in db.Likes
                        on p.Id equals l.PostId into eGroup
                        from l in eGroup.DefaultIfEmpty()
                        select new
                        {
                            post = p,
                            like = l != null && l.UserId == UserID ? l.like : false,
                            dislike = l != null && l.UserId == UserID ? l.Dislike : false
                          
                        };

            /*
    
            var posts = from p in db.Posts
                        join c in db.Comments
                        on p.Id equals c.PostId into eGroup
                        from c in eGroup.DefaultIfEmpty()
                        select new
                        {
                           first= p.User.FirstName,
                           TripTitle =p.TripTitle,
                           CommentDesc =p.comments 
                        };*/
            return View(posts);
        }
    }
}