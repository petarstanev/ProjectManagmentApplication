﻿using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ProjectManagementApplication.Models;
using ProjectManagmentApplication.Hubs;

namespace ProjectManagementApplication.Controllers
{
    [Authorize]
    public class CommentsController : Controller
    {
        private Context db = new Context();

        // GET: Comments/Create
        public ActionResult Create(int? taskId)
        {
            if (taskId == null)
            {
                return HttpNotFound();
            }

            Comment comment = new Comment();
            comment.Task = db.Tasks.Find(taskId);
            return View(comment);
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
                comment.UserId = sessionContext.GetUserData().UserId;

                db.Comments.Add(comment);
                db.SaveChanges();
                TaskHub.TaskUpdated(comment.TaskId);

                return RedirectToAction("Details", "Tasks", new { id = comment.TaskId});
            }

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
            return View(comment);
        }

        // POST: Comments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( Comment comment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(comment).State = EntityState.Modified;
                db.SaveChanges();
                TaskHub.TaskUpdated(comment.TaskId);
                return RedirectToAction("Details", "Tasks", new { id = comment.TaskId });
            }
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
            TaskHub.TaskUpdated(comment.TaskId);
            return RedirectToAction("Details", "Tasks", new { id = comment.TaskId });
        }
    }
}
