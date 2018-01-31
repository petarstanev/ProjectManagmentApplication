﻿using System;
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
    public class ColumnsController : Controller
    {
        private Context db = new Context();

        // GET: Columns
        public ActionResult Index()
        {
            var columns = db.Columns.Include(c => c.Board);
            return View(columns.ToList());
        }

        // GET: Columns/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Column column = db.Columns.Find(id);
            if (column == null)
            {
                return HttpNotFound();
            }
            return View(column);
        }

    
        // GET: Columns/Create
        public ActionResult Create(int? boardId)
        {

            if (boardId != null)
            {
                ViewBag.BoardId = new SelectList(db.Boards, "BoardId", "Title", boardId);

            }
            else
            {
                ViewBag.BoardId  = new SelectList(db.Boards, "BoardId", "Title");
            }
            return View();
        }

        // POST: Columns/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ColumnId,Title,BoardId")] Column column)
        {
            if (ModelState.IsValid)
            {
                db.Columns.Add(column);
                db.SaveChanges();
                return RedirectToAction("Details","Boards",new {id = column.BoardId});
            }

            ViewBag.BoardId = new SelectList(db.Boards, "BoardId", "Title", column.BoardId);
            return View(column);
        }
        
        // GET: Columns/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Column column = db.Columns.Find(id);
            if (column == null)
            {
                return HttpNotFound();
            }
            ViewBag.BoardId = new SelectList(db.Boards, "BoardId", "Title", column.BoardId);
            return View(column);
        }

        // POST: Columns/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ColumnId,Title,BoardId")] Column column)
        {
            if (ModelState.IsValid)
            {
                db.Entry(column).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "Boards", new { id = column.BoardId });
            }
            ViewBag.BoardId = new SelectList(db.Boards, "BoardId", "Title", column.BoardId);
            return View(column);
        }

        // GET: Columns/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Column column = db.Columns.Find(id);
            if (column == null)
            {
                return HttpNotFound();
            }
            return View(column);
        }

        // POST: Columns/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Column column = db.Columns.Find(id);
            db.Columns.Remove(column);
            db.SaveChanges();
            return RedirectToAction("Details", "Boards", new { id = column.BoardId });
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
