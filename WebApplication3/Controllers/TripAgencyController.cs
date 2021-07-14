using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication3.general;
using WebApplication3.Models;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.Owin;
using System.Web.Helpers;

namespace WebApplication3.Controllers
{
    [Authorize(Roles = "TripsAgencey")]
    public class TripAgencyController : Controller
    {

        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationDbContext db;
        public TripAgencyController()
        {
            db = new ApplicationDbContext();

        }
       
        // GET: TripAgency

        public ActionResult Index()
        {
            var UserId = User.Identity.GetUserId();
            var posts = db.Posts.Where(a=>a.UserId==UserId);
            return View(posts.ToList());
        }
      
        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ActionResult Profile()
        {
            var UserId = User.Identity.GetUserId();
            var user = db.Users.Find(UserId);

            ProfileUserEdite u = new ProfileUserEdite();
            u.FirstName = user.FirstName;
            u.LastName = user.LastName;
            u.Email = user.Email;
            u.PhoneNumber = user.PhoneNumber;
            u.Photo = user.Photo;

            return View(u);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Profile(ProfileUserEdite userprofile, HttpPostedFileBase UserPhoto)
        {
            if (ModelState.IsValid)
            {
                var UserId = User.Identity.GetUserId();
                var user = await UserManager.FindByIdAsync(UserId);
                if (user == null)
                {
                    // Don't reveal that the user does not exist
                    return RedirectToAction("ResetPasswordConfirmation", "Account");
                }
                user.Email = userprofile.Email;
                user.UserName = userprofile.Email;
                user.FirstName = userprofile.FirstName;
                user.LastName = userprofile.LastName;
                user.PhoneNumber = userprofile.PhoneNumber;
                if(userprofile.Password != null)
                {
                    user.PasswordHash = Crypto.HashPassword(userprofile.Password);
                }
                string path = Server.MapPath("~/upload/user_images/");
                string name = Upload.upload_image(path, UserPhoto);
                if (name != null)
                {
                    Upload.delete_Image(path, user.Photo);
                    user.Photo = name;
                }

                var result = await UserManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("profile", "TripAgency");
                }
                return View(userprofile);
            }

            return View(userprofile);

        }

        // GET: TripAgency/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // GET: TripAgency/Create
       
        public ActionResult CreatePost()
        {
            return View();
        }

        // POST: TripAgency/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreatePost(Post post, HttpPostedFileBase PostImage)
        {
            var UserId = User.Identity.GetUserId();
           
            if (ModelState.IsValid)
            {
                string path = Server.MapPath("~/upload/Post_images/");                
                Post p = new Post();
                p.UserId = UserId;
                p.TripTitle = post.TripTitle;
                p.TripDetails = post.TripDetails;
                p.TripDestenation = post.TripDestenation;
                p.TripDate = post.TripDate;
                string name = Upload.upload_image(path, PostImage);
                if (name != null)
                {
                    p.TripImage = name;
                }
                p.PostDate = DateTime.Now;
                p.Price = post.Price;
                p.RankLike = 0;
                p.RankDislike = 0;
                p.Approved = false;
                db.Posts.Add(p);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

          
            return View(post);
        }
        public ActionResult ReceivedQuestions()
        {
            var UserID = User.Identity.GetUserId();
            var comments = from comment in db.Comments
                           join post in db.Posts
                           on comment.PostId equals post.Id
                           where post.UserId == UserID select comment ;
            return View(comments.ToList());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Answer(string Answer,int CommentId)
        {

            if (ModelState.IsValid)
            {
               
                Comment comment = db.Comments.Find(CommentId);
                comment.Answer = Answer;
                db.Entry(comment).State = EntityState.Modified;
                db.SaveChanges();
            }
            return Redirect("/TripAgency/ReceivedQuestions");
        }

 

        // GET: TripAgency/Edit/5
            public ActionResult EditPost(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // POST: TripAgency/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(Post post, HttpPostedFileBase PostImage)
        {
            if (ModelState.IsValid)
            {
           
                string path = Server.MapPath("~/upload/Post_images/");
        
                string name = Upload.upload_image(path, PostImage);
                if (name != null)
                {
                    Upload.delete_Image(path, post.TripImage);
                    post.TripImage = name;
                }
               
                db.Entry(post).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
           
            return View(post);
        }



        public ActionResult DeletePost(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            var comments = db.Comments.Where(a => a.PostId == post.Id).ToList();
            if (comments.Count > 0)
            {
                foreach (var comment in comments)
                {
                    db.Comments.Remove(comment);
                    db.SaveChanges();
                }
            }
            var likes = db.Likes.Where(a => a.PostId == post.Id).ToList();
            if (likes.Count > 0)
            {
                foreach (var like in likes)
                {
                    db.Likes.Remove(like);
                    db.SaveChanges();
                }
            }
            var saveposts = db.SavePosts.Where(a => a.PostId == post.Id).ToList();
            if (saveposts.Count > 0)
            {
                foreach (var savepost in saveposts)
                {
                    db.SavePosts.Remove(savepost);
                    db.SaveChanges();
                }
            }
            string path = Server.MapPath("~/upload/Post_images/");
            Upload.delete_Image(path, post.TripImage);
                db.Posts.Remove(post);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
