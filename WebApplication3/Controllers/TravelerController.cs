using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication3.Models;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using System.Data.Entity;
using System.Threading.Tasks;

namespace WebApplication3.Controllers
{
    [Authorize(Roles = "Traveller")]
    public class TravelerController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Traveler
        public ActionResult Index()
        {
            var posts = db.Posts.Where(a => a.Approved == true);
            return View(posts.ToList());
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> SearchIndex(string agency, double? price, string date)
        {
            if (agency != null && price != null)
            {
                var result = from p in db.Posts
                             where (p.User.FirstName.Contains(agency)) &&
                             (p.Price == price)
                             select p;
                if (result != null)
                {
                    return View(result.ToList());
                }
           
            }
            if (agency != null)
            {
                var result = from p in db.Posts
                             where (p.User.FirstName.Contains(agency))
                             select p;
                if (result != null)
                {
                    return View(result.ToList());
                }
              
            }
            if (price != null)
            {
                var result = from p in db.Posts
                             where (p.Price == price)
                             select p;
                if (result != null)
                {
                    return View(result.ToList());
                }
            
            }
            ViewBag.NotResult = "not result search";
            return View();
        
    }
        // Hadeel Alaa is inviting you to a scheduled Zoom meeting.Topic:  97 Time: Jun 7, 2021 02:40 PM Cairo  Join Zoom Meeting https://us04web.zoom.us/j/7067009972?pwd=bzRtQzVhSWFtU2d2eG5mL29uSmYrZz09  Meeting ID: 706 700 9972 Passcode: 6Tjk0t


        public ActionResult SavePosts()
        {
            var UserID = User.Identity.GetUserId();
            var posts = db.SavePosts.Where(a => a.UserId == UserID);
            return View(posts.ToList()); 
        }

        public ActionResult AddLikedPosts(int id)
        {
            var UserID = User.Identity.GetUserId();
            Post post = db.Posts.Find(id);
            var l = db.Likes.Where(x => x.UserId == UserID && x.PostId == id ).FirstOrDefault();
            if (l==null)
            {
                var like = new Like();
                like.like = true;
                like.Dislike = false;
                like.PostId = id;
                like.UserId = UserID;
                db.Likes.Add(like);
                //update ranklike 
                post.RankLike = post.RankLike + 1;
                db.Entry(post).State = EntityState.Modified;
                db.SaveChanges();
            }
            else
            {
                if (l.like == true)
                {
                    l.like = false;
                    db.Entry(l).State = EntityState.Modified;
                    post.RankLike = post.RankLike -1;
                    db.Entry(post).State = EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    l.like = true;
                    l.Dislike = false;
                    db.Entry(l).State = EntityState.Modified;
                    post.RankLike = post.RankLike + 1;
                    db.Entry(post).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            
            return RedirectToAction("Index");
        }
        public ActionResult AddDislikedPosts(int id)
        {
            var UserID = User.Identity.GetUserId();
            Post post = db.Posts.Find(id);
            var l = db.Likes.Where(x => x.UserId == UserID && x.PostId == id).FirstOrDefault();
            if (l == null)
            {
                var like = new Like();
                like.like = false;
                like.Dislike = true;
                like.PostId = id;
                like.UserId = UserID;
                db.Likes.Add(like);
                //update ranklike 
                post.RankDislike = post.RankDislike + 1;
                db.Entry(post).State = EntityState.Modified;
                db.SaveChanges();
            }
            else
            {
                if (l.Dislike == true)
                {
                    l.Dislike = false;
                    db.Entry(l).State = EntityState.Modified;
                    post.RankDislike = post.RankDislike - 1;
                    db.Entry(post).State = EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    l.Dislike = true;
                    l.like = false;
                    db.Entry(l).State = EntityState.Modified;
                    post.RankDislike = post.RankDislike + 1;
                    db.Entry(post).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            
            return RedirectToAction("Index");
        }
        public ActionResult AddToSavePosts(int id)
        {
            var UserID = User.Identity.GetUserId();
            if (db.SavePosts.Where(x => x.UserId == UserID && x.PostId == id).FirstOrDefault() == null)
            {
                var saved = new SavePost();
                saved.PostId = id;
                saved.UserId = UserID;
                saved.SaveDate = System.DateTime.Now;
                db.SavePosts.Add(saved);
                db.SaveChanges();
            }
            
            return RedirectToAction("Index");
        }
        
        
        public ActionResult SendComment(string comment,int id)
        {
            var UserID = User.Identity.GetUserId();
            var com = new Comment();
            com.CommentDescription = comment;
            com.PostId = id;
            com.UserId = UserID;
            db.Comments.Add(com);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult replies(int ?id)
        {
            var UserID = User.Identity.GetUserId();
            var comments=db.Comments.Where(x => x.UserId == UserID && x.PostId==id).ToList();
            return View(comments);
        }
     
    }
}