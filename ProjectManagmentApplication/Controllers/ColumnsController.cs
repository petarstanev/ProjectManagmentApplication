using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ProjectManagmentApplication.Models;

namespace ProjectManagmentApplication.Controllers
{
    public class ColumnsController : Controller
    {
        private Context db = new Context();

        // GET: Columns
        public ActionResult Index()
        {
            return View(db.Columns.ToList());
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
        public ActionResult Create(int boardId)
        {
            if (boardId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Board board = db.Boards.Include(e => e.Columns).SingleOrDefault(e => e.BoardId == id);
            Board board = db.Boards.SingleOrDefault(e => e.BoardId == boardId);

            Column column = new Column();

            column.Board = board;

            return View(column);
        }


        // POST: Columns/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Column column)
        {
            if (ModelState.IsValid)
            {
                Board board = db.Boards.Include(p => p.Columns).SingleOrDefault(p => p.BoardId == column.Board.BoardId);
                column.Board = board;
                db.Columns.Add(column);
                db.SaveChanges();
                return RedirectToAction("Index","Boards");
            }

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
            return View(column);
        }

        // POST: Columns/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ColumnId,Title")] Column column)
        {
            if (ModelState.IsValid)
            {
                db.Entry(column).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
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
