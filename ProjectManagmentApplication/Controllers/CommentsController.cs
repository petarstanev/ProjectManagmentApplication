using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ProjectManagmentApplication.Models;

namespace ProjectManagmentApplication.Controllers
{
    public class CommentsController : Controller
    {
        private Context db = new Context();

        // GET: Comments
        public ActionResult Index()
        {
            var comments = db.Comments.Include(c => c.Author).Include(c => c.Task);
            return View(comments.ToList());
        }

        // GET: Comments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // GET: Comments/Create
        public ActionResult Create(int? taskId)
        {
            ViewBag.UserId = new SelectList(db.Users, "UserId", "Email");

            if (taskId != null)
            {
                ViewBag.TaskId = new SelectList(db.Tasks, "TaskId", "Title", taskId);

            }
            else
            {
                ViewBag.TaskId = new SelectList(db.Tasks, "TaskId", "Title");
            }

            return View();
        }

        // POST: Comments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CommentId,Content,CreatedDate,TaskId,UserId")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                comment.CreatedDate = DateTime.UtcNow;
                SessionContext sessionContext = new SessionContext();
                comment.Author = sessionContext.GetUserData();

                db.Comments.Add(comment);
                db.SaveChanges();
                
                return RedirectToAction("Details", "Tasks", new { id = comment.TaskId});
            }

            ViewBag.UserId = new SelectList(db.Users, "UserId", "Email", comment.UserId);
            ViewBag.TaskId = new SelectList(db.Tasks, "TaskId", "Title", comment.TaskId);
            return View(comment);
        }

        // GET: Comments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(db.Users, "UserId", "Email", comment.UserId);
            ViewBag.TaskId = new SelectList(db.Tasks, "TaskId", "Title", comment.TaskId);
            return View(comment);
        }

        // POST: Comments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CommentId,Content,CreatedDate,TaskId,UserId")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(comment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(db.Users, "UserId", "Email", comment.UserId);
            ViewBag.TaskId = new SelectList(db.Tasks, "TaskId", "Title", comment.TaskId);
            return View(comment);
        }

        // GET: Comments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Comment comment = db.Comments.Find(id);
            db.Comments.Remove(comment);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
