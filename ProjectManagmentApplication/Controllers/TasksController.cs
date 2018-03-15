using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Microsoft.AspNet.SignalR;
using ProjectManagementApplication.Models;
using ProjectManagmentApplication.Hubs;

namespace ProjectManagementApplication.Controllers
{
    public class TasksController : Controller
    {
        private Context db = new Context();

        // GET: Tasks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Task task = db.Tasks.Include(t => t.Images).SingleOrDefault(t => t.TaskId == id);

            if (task == null)
            {
                return HttpNotFound();
            }

             return View(task);
        }

        // GET: Tasks/Details/5
        [HttpGet]
        public ActionResult DetailsGet(int id)
        {
            Task task = db.Tasks.Include(t => t.Images)
                .Include(t => t.AssignedToUser)
                .Include(t => t.CreatedByUser)
                .SingleOrDefault(t => t.TaskId == id);

            if (task == null)
            {
                return HttpNotFound();
            }

            task.Comments = db.Comments.Include(c => c.Author).Where(c => c.TaskId == task.TaskId).ToList();
            task.Column = db.Columns.Include(c => c.Board).SingleOrDefault(c => c.ColumnId == task.ColumnId);
           
            return PartialView("PartialView/TaskDetails", task);
        }

        // GET: Tasks/Create
        [System.Web.Mvc.Authorize]
        public ActionResult Create(int boardId)
        {
            ViewBag.ColumnId = new SelectList(db.Columns.Where(c => c.BoardId == boardId), "ColumnId", "Title");
            
            ViewBag.AssignedTo = new SelectList(db.Users, "UserId", "Email");

            ViewBag.CreatedBy = new SelectList(db.Users, "UserId", "Email");
            return View();
        }

        // POST: Tasks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [System.Web.Mvc.Authorize]
        public ActionResult Create([Bind(Include = "TaskId,Title,Description,Deadline,Private,CreatedBy,AssignedTo,ColumnId")] Task task)
        {
            if (ModelState.IsValid)
            {
                db.Tasks.Add(task);
                db.SaveChanges();
                task.Column = db.Columns.Find(task.ColumnId);
                return RedirectToAction("Details", "Boards", new { id = task.Column.BoardId });
            }

            ViewBag.AssignedTo = new SelectList(db.Users, "UserId", "Email", task.AssignedTo);
            ViewBag.ColumnId = new SelectList(db.Columns, "ColumnId", "Title", task.ColumnId);
            ViewBag.CreatedBy = new SelectList(db.Users, "UserId", "Email", task.CreatedBy);
            return View(task);
        }

        // GET: Tasks/Edit/5
        [System.Web.Mvc.Authorize]
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
            ViewBag.CreatedBy = new SelectList(db.Users, "UserId", "Email", task.CreatedBy);
            task.Column = db.Columns.Find(task.ColumnId);
            return View(task);
        }

        // POST: Tasks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [System.Web.Mvc.Authorize]
        public ActionResult Edit([Bind(Include = "TaskId,Title,Description,Deadline,Private,CreatedBy,AssignedTo,ColumnId")] Task task)
        {
            if (ModelState.IsValid)
            {
                db.Entry(task).State = EntityState.Modified;
                db.SaveChanges();
                TaskHub.TaskUpdated(task.TaskId);

                return  RedirectToAction("Details", new { id = task.TaskId });
            }
            ViewBag.AssignedTo = new SelectList(db.Users, "UserId", "Email", task.AssignedTo);
            ViewBag.ColumnId = new SelectList(db.Columns, "ColumnId", "Title", task.ColumnId);
            ViewBag.CreatedBy = new SelectList(db.Users, "UserId", "Email", task.CreatedBy);
            return View(task);
        }

        // GET: Tasks/Delete/5
        [System.Web.Mvc.Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Task task = db.Tasks.Include(t => t.AssignedToUser)
                .Include(t => t.CreatedByUser)
                .SingleOrDefault(t => t.TaskId == id);

            if (task == null)
            {
                return HttpNotFound();
            }
            return View(task);
        }

        // POST: Tasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [System.Web.Mvc.Authorize]
        public ActionResult DeleteConfirmed(int id)
        {
            Task task = db.Tasks.Find(id);
            task.Column = db.Columns.Find(task.ColumnId);
            int boardId = task.Column.BoardId;
            db.Tasks.Remove(task);
            db.SaveChanges();
            return RedirectToAction("Details", "Boards", new { id = boardId });
        }

        [HttpPut]
        public JsonResult Edit(int? taskId, int? columnId)
        {
            Task task = db.Tasks.Find(taskId);
            if (task == null)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return Json((int)Response.StatusCode, JsonRequestBehavior.AllowGet);
            }

            //task.ColumnId = columnId;
            db.Entry(task).State = EntityState.Modified;
            db.SaveChanges();

            Response.StatusCode = (int)HttpStatusCode.OK;
            return Json((int)Response.StatusCode, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ChangeTaskColumn(int taskId, int columnId)
        {
            Task task = db.Tasks.Find(taskId);

            if (task == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            task.ColumnId = columnId;
            db.Entry(task).State = EntityState.Modified;
            db.SaveChanges();

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpGet]
        public ActionResult TestLoad()
        {
            ////Board b = db.Boards.Find(23);
            ////db.Boards.Remove(b);
            //for (int i = 31; i < 40; i++)
            //{
            //    for (int y = 0; y < 100; y++)
            //    {
            //        Task task = new Task();
            //        task.ColumnId = i;
            //        task.Title = y + " " + i;
            //        db.Tasks.Add(task);
            //        db.SaveChanges();
            //    }
            //}

            return null;
        } 
    }
}
