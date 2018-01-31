using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
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
            var tasks = db.Tasks.Include(t => t.Column);
            return View(tasks.ToList());
        }

        // GET: Tasks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Task task = db.Tasks.Include(c => c.Comments.Select(o => o.Author))
                .SingleOrDefault(t => t.TaskId == id);
            Column column = db.Columns.Find(task.ColumnId);
           
            task.Column = column;
            
            return View(task);
        }

        // GET: Tasks/Create
        public ActionResult Create(int? columnId)
        {
            if (columnId == null)
            {
                ViewBag.ColumnId = new SelectList(db.Columns, "ColumnId", "Title");
            }
            else
            {
                ViewBag.ColumnId = new SelectList(db.Columns, "ColumnId", "Title", columnId);
            }
            return View();
        }

        // POST: Tasks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TaskId,Title,Description,Deadline,Private,ColumnId")] Task task)
        {
            if (ModelState.IsValid)
            {
                db.Tasks.Add(task);
                db.SaveChanges();

                Column column = db.Columns.Find(task.ColumnId);
                return RedirectToAction("Details", "Boards", new { id = column.BoardId});
            }

            ViewBag.ColumnId = new SelectList(db.Columns, "ColumnId", "Title", task.ColumnId);
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
            ViewBag.ColumnId = new SelectList(db.Columns, "ColumnId", "Title", task.ColumnId);
            return View(task);
        }

        // POST: Tasks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TaskId,Title,Description,Deadline,Private,ColumnId")] Task task)
        {
            if (ModelState.IsValid)
            {
                db.Entry(task).State = EntityState.Modified;
                db.SaveChanges();
                Column column = db.Columns.Find(task.ColumnId);
                return RedirectToAction("Details", "Boards", new { id = column.BoardId });
            }
            ViewBag.ColumnId = new SelectList(db.Columns, "ColumnId", "Title", task.ColumnId);
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
            Column column = db.Columns.Find(task.ColumnId);
            return RedirectToAction("Details", "Boards", new { id = column.BoardId });
        }

        [HttpPost]
        public ActionResult UploadImage(HttpPostedFileBase file)
        {
            if (file != null)
            {
                string pic = System.IO.Path.GetFileName(file.FileName);
                string path = System.IO.Path.Combine(
                                       Server.MapPath("~/images/taskimages"), pic);
                // file is uploaded
                file.SaveAs(path);

                // save the image path path to the database or you can send image 
                // directly to database
                // in-case if you want to store byte[] ie. for DB
                using (MemoryStream ms = new MemoryStream())
                {
                    file.InputStream.CopyTo(ms);
                    byte[] array = ms.GetBuffer();
                }

            }
            // after successfully uploading redirect the user
            return RedirectToAction("Details");
        }
    }
}
