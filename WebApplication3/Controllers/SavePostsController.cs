using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    public class SavePostsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: SavePosts
        public ActionResult Index()
        {
            var savePosts = db.SavePosts.Include(s => s.post);
            return View(savePosts.ToList());
        }

        // GET: SavePosts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SavePost savePost = db.SavePosts.Find(id);
            if (savePost == null)
            {
                return HttpNotFound();
            }
            return View(savePost);
        }

        // GET: SavePosts/Create
        public ActionResult Create()
        {
            ViewBag.PostId = new SelectList(db.Posts, "Id", "TripTitle");
            return View();
        }

        // POST: SavePosts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,SaveDate,ApplicationUserId,PostId")] SavePost savePost)
        {
            if (ModelState.IsValid)
            {
                db.SavePosts.Add(savePost);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PostId = new SelectList(db.Posts, "Id", "TripTitle", savePost.PostId);
            return View(savePost);
        }

        // GET: SavePosts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SavePost savePost = db.SavePosts.Find(id);
            if (savePost == null)
            {
                return HttpNotFound();
            }
            ViewBag.PostId = new SelectList(db.Posts, "Id", "TripTitle", savePost.PostId);
            return View(savePost);
        }

        // POST: SavePosts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,SaveDate,ApplicationUserId,PostId")] SavePost savePost)
        {
            if (ModelState.IsValid)
            {
                db.Entry(savePost).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PostId = new SelectList(db.Posts, "Id", "TripTitle", savePost.PostId);
            return View(savePost);
        }

        // GET: SavePosts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SavePost savePost = db.SavePosts.Find(id);
            if (savePost == null)
            {
                return HttpNotFound();
            }
            return View(savePost);
        }

        // POST: SavePosts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SavePost savePost = db.SavePosts.Find(id);
            db.SavePosts.Remove(savePost);
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
