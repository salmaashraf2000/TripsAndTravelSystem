
using Microsoft.AspNet.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApplication3.Models;
using Microsoft.AspNet.Identity.Owin;
using System.Web.Helpers;
using WebApplication3.general;
using System.Net;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Collections.Generic;
using System.Collections;

namespace WebApplication3.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationDbContext db;
        public AdminController()
        {
            db = new ApplicationDbContext();
           
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
    
        // GET:All users
        public ActionResult Index()
        {
            var users =  UserManager.Users.Where(a=>a.UserType != "Admin");

           // ViewBag.AllUsers = users;
            return View(users);

        }
        // GET:All Posts
        public ActionResult GetPosts()
        {
            /*var posts = (from Posts in db.Posts
                         join Users in db.Users on Posts.UserId equals Users.Id
                         where Posts.Approved == true
                         select new
                         {

                             TripTitle = Posts.TripTitle,
                             Tripdetails = Posts.TripDetails,
                             TripDate = Posts.TripDate,
                             TripDestination = Posts.TripDestenation,
                             TripPrice = Posts.Price,
                             TripImage = Posts.TripImage,
                             AgencyName1 = Users.FirstName,
                             AgencyName2 = Users.LastName
                         });
            */
            var posts = db.Posts.Where(a => a.Approved == true);
            //ViewBag.posts = posts;
            return View(posts.ToList());
        }

        // GET:All Posts requests
        public ActionResult GetAllRequests()
        {
/*
            var posts = (from Posts in db.Posts
                         join Users in db.Users on Posts.UserId equals Users.Id
                         where Posts.Approved == false
                         select new
                         {
                             Posts.TripTitle,
                             Posts.TripDetails,
                             Posts.TripDate,
                             Posts.TripDestenation,
                             Posts.Price,
                             Posts.TripImage,
                             Users.FirstName,
                             Users.LastName,
                             Posts.Id
                         });
            ViewBag.Requests = posts;
*/
            var posts = db.Posts.Where(a => a.Approved == false);
        
          
            return View(posts.ToList());
        }
        // Accept or refuse Posts requests
          public ActionResult accept(int? id)
        {
            Post p = db.Posts.Find(id);
            p.Approved = true;
            db.Entry(p).State = EntityState.Modified;
            db.SaveChanges();
            return Redirect("/Admin/GetAllRequests");
        }
        public ActionResult decline(int? id)
        {
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            string path = Server.MapPath("~/upload/Post_images/");
            Upload.delete_Image(path, post.TripImage);
            db.Posts.Remove(post);
            db.SaveChanges();
            return Redirect("/Admin/GetAllRequests");
        }
        public ActionResult AcceptRefusePosts(string submit, string postId)
        {
            if (submit == "accept")
            {
                Post p = db.Posts.Find(Convert.ToInt32(postId));
                p.Approved = true;
                db.Entry(p).State = EntityState.Modified;
                db.SaveChanges();

            }
            else if (submit == "decline")
            {
                Post post = db.Posts.Find(Convert.ToInt32(postId));
                if (post == null)
                {
                    return HttpNotFound();
                }
                string path = Server.MapPath("~/upload/Post_images/");
                Upload.delete_Image(path, post.TripImage);
                db.Posts.Remove(post);
                db.SaveChanges();
            }
            return RedirectToAction("RequestedPage");
        }
        public ActionResult AddUser()
        {
            ViewBag.Name = new SelectList(db.Roles.Where(u => !u.Name.Contains("Admin")).ToList(), "Name", "Name");
            return View();
        }
        // GET:add user
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddUser(RegisterViewModel model, HttpPostedFileBase UserPhoto)
        {
            if (ModelState.IsValid)
            {
                string path = Server.MapPath("~/upload/user_images/");

                //upload_image take path of image and HttpPostedFileBase and return the image name 

                string name = Upload.upload_image(path, UserPhoto);
                if (name != null)
                {
                    model.Photo = name;

                }


                var user = new ApplicationUser { UserName = model.Email, Email = model.Email, FirstName = model.FirstName, LastName = model.LastName, PhoneNumber = model.PhoneNumber, Photo = model.Photo, UserType = model.UserType };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await this.UserManager.AddToRoleAsync(user.Id, model.UserType);
                    return RedirectToAction("Index");
                }
               
            }
            return View(model);
        }
        public ActionResult AddPost()
        {
            return View();
        }
        // Add post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddPost(Post post, HttpPostedFileBase PostImage)
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
                p.Approved = true;
                db.Posts.Add(p);
                db.SaveChanges();
                return RedirectToAction("GetPosts");
            }


            return View(post);
        }

        // delete post

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
            return RedirectToAction("GetPosts");
        }

        public ActionResult DeleteUser(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            var posts = db.Posts.Where(a => a.UserId == id).ToList();
            if (posts.Count > 0)
            {
                foreach (var post in posts)
                {
                    var comments = db.Comments.Where(a => a.PostId == post.Id).ToList();
                        if (comments.Count>0)
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
                    string path1 = Server.MapPath("~/upload/Post_images/");
                    Upload.delete_Image(path1, post.TripImage);
                    db.Posts.Remove(post);
                    db.SaveChanges();
                }
            }
            string path2 = Server.MapPath("~/upload/user_images/");
            Upload.delete_Image(path2, user.Photo);
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
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


        // edit post
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
                return RedirectToAction("GetPosts");
            }

            return View(post);
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
                if (userprofile.Password != null)
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
                    return RedirectToAction("Profile", "Admin");
                }
                ViewBag.userprofile = userprofile;
                return View(userprofile);
            }

            return View(userprofile);

        }


    }
}
