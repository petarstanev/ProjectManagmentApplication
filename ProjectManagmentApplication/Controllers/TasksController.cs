using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProjectManagementApplication.Models;

namespace ProjectManagementApplication.Controllers
{
    public class TasksController : Controller
    {
        private Context db = new Context();

        // GET: Tasks
        public ActionResult Index()
        {
            var tasks = db.Tasks.Include(t => t.AssignedToUser).Include(t => t.Column).Include(t => t.CreatedByUser);
            
            return View(tasks.ToList());
        }

        // GET: Tasks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Task task = db.Tasks.Find(id);
            if (task == null)
            {
                return HttpNotFound();
            }
            return View(task);
        }

        // GET: Tasks/Create
        public ActionResult Create()
        {
            ViewBag.AssignedTo = new SelectList(db.Users, "UserId", "Email");
            ViewBag.ColumnId = new SelectList(db.Columns, "ColumnId", "Title");
            ViewBag.CreatedBy = new SelectList(db.Users, "UserId", "Email");
            return View();
        }

        // POST: Tasks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TaskId,Title,Description,Deadline,Private,CreatedBy,AssignedTo,ColumnId")] Task task)
        {
            if (ModelState.IsValid)
            {
                db.Tasks.Add(task);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AssignedTo = new SelectList(db.Users, "UserId", "Email", task.AssignedTo);
            ViewBag.ColumnId = new SelectList(db.Columns, "ColumnId", "Title", task.ColumnId);
            ViewBag.CreatedBy = new SelectList(db.Users, "UserId", "Email", task.CreatedBy);
            return View(task);
        }

        // GET: Tasks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Task task = db.Tasks.Find(id);
            if (task == null)
            {
                return HttpNotFound();
            }
            ViewBag.AssignedTo = new SelectList(db.Users, "UserId", "Email", task.AssignedTo);
            ViewBag.ColumnId = new SelectList(db.Columns, "ColumnId", "Title", task.ColumnId);
            ViewBag.CreatedBy = new SelectList(db.Users, "UserId", "Email", task.CreatedBy);
            return View(task);
        }

        // POST: Tasks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TaskId,Title,Description,Deadline,Private,CreatedBy,AssignedTo,ColumnId")] Task task)
        {
            if (ModelState.IsValid)
            {
                db.Entry(task).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AssignedTo = new SelectList(db.Users, "UserId", "Email", task.AssignedTo);
            ViewBag.ColumnId = new SelectList(db.Columns, "ColumnId", "Title", task.ColumnId);
            ViewBag.CreatedBy = new SelectList(db.Users, "UserId", "Email", task.CreatedBy);
            return View(task);
        }

        // GET: Tasks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Task task = db.Tasks.Find(id);
            if (task == null)
            {
                return HttpNotFound();
            }
            return View(task);
        }

        // POST: Tasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Task task = db.Tasks.Find(id);
            db.Tasks.Remove(task);
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
