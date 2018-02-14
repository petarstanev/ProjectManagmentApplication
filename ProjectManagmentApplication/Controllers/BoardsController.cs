using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProjectManagementApplication.Models;
using ProjectManagementApplication.ViewModels;

namespace ProjectManagementApplication.Controllers
{
    [Authorize]
    public class BoardsController : Controller
    {
        private Context db = new Context();

        // GET: Boards
        public ActionResult Index()
        {
            return View(db.Boards.Include(c => c.User).ToList());
        }

        // GET: Boards
        public ActionResult IndexJson()
        {
            try
            {
                return Json(db.Boards.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        // GET: Boards/Details/5
        public ActionResult Details(int? id)
        {
            BoardViewModel boardView = new BoardViewModel();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Board board = db.Boards.Include(e => e.Columns.Select(c => c.Tasks)).Include(b => b.User).SingleOrDefault(e => e.BoardId == id);
            if (board == null)
            {
                return HttpNotFound();
            }
            if (!ValidateBoardAccess(board))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

            boardView.SelectedBoard = board;
            boardView.AllBoards = db.Boards.ToList();

            return View(boardView);
        }

        // GET: Boards/Details/5
        [HttpGet]
        public ActionResult DetailsPart(int? id, string type)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Board board = db.Boards.Include(e => e.Columns.Select(c => c.Tasks)).SingleOrDefault(e => e.BoardId == id);

            if (type == "all-tasks")
            {
                return PartialView("PartialView/BoardDetail", board);
            }

            SessionContext sx = new SessionContext();

            Column filteredColumn;
            //TODO change 
            Board filterBoard = new Board();
            filterBoard.Title = board.Title;
            filterBoard.Columns = new List<Column>();
            foreach (Column column in board.Columns)
            {
                filteredColumn = new Column();
                filteredColumn.Title = column.Title;
                filteredColumn.Tasks = new List<Task>();
                foreach (Task task in column.Tasks)
                {
                    if (ShouldTaskBeIncluded(task, type))
                    {
                        filteredColumn.Tasks.Add(task);
                    }
                }
                filterBoard.Columns.Add(filteredColumn);
            }

            return PartialView("PartialView/BoardDetail", filterBoard);
        }

        private bool ShouldTaskBeIncluded(Task task, string type)
        {
            SessionContext sx = new SessionContext();

            if (type == "assigned-tasks" && task.AssignedTo != null && task.AssignedTo == sx.GetUserId())
            {
                return true;
            }
            if (type == "created-tasks" && task.CreatedBy != null && task.CreatedBy == sx.GetUserId())
            {
                return true;
            }
            return false;
        }


        // GET: Boards/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Boards/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BoardId,Title,BoardType")] Board board)
        {
            if (ModelState.IsValid)
            {
                SessionContext sx = new SessionContext();
                board.UserId = sx.GetUserId();

                db.Boards.Add(board);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(board);
        }

        // GET: Boards/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Board board = db.Boards.Find(id);
            if (board == null)
            {
                return HttpNotFound();
            }
            return View(board);
        }

        // POST: Boards/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BoardId,Title")] Board board)
        {
            if (ModelState.IsValid)
            {
                db.Entry(board).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(board);
        }

        // GET: Boards/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Board board = db.Boards.Find(id);
            if (board == null)
            {
                return HttpNotFound();
            }
            return View(board);
        }

        // POST: Boards/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Board board = db.Boards.Include(p => p.Columns).SingleOrDefault(p => p.BoardId == id);

            db.Columns.RemoveRange(board.Columns);
            db.Boards.Remove(board);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult AddTeamMember(int id)
        {
            Board board = db.Boards.Find(id);
            if (board == null)
            {
                return HttpNotFound();
            }
            TeamMember teamMember = new TeamMember {BoardId = board.BoardId, Board = board};
            ViewBag.UserId = new SelectList(db.Users, "UserId", "Email");
            return View(teamMember);
        }

        [HttpPost]
        public ActionResult AddTeamMember(TeamMember teamMember)
        {
            db.TeamMembers.Add(teamMember);
            db.SaveChanges();
            return RedirectToAction("Details", new {id = teamMember.BoardId});
        }




        private bool ValidateBoardAccess(Board board)
        {
            SessionContext sx = new SessionContext();
            User currentUser = sx.GetUserData();
            bool result;

            switch (board.BoardType)
            {
                case BoardType.Private:
                    result = CheckAdministrator(board);
                    break;
                case BoardType.Team:
                    TeamMember teamMember = db.TeamMembers.FirstOrDefault(u => u.UserId == currentUser.UserId && u.BoardId == board.BoardId);
                    
                    result = teamMember != null || CheckAdministrator(board);
                    break;
                default:
                    result = true;
                    break;
            }

            return result;
        }

        private bool CheckAdministrator(Board board)
        {
            SessionContext sx = new SessionContext();
            return board.User.UserId == sx.GetUserId();
        }
    }
}
