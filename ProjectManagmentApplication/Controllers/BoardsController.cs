using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ProjectManagementApplication.Models;
using ProjectManagementApplication.ViewModels;

namespace ProjectManagementApplication.Controllers
{
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
        public ActionResult DetailsPart(int? id, string type, string taskName, string time, string personName)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Board board = db.Boards.Include(e => e.Columns.Select(c => c.Tasks)).SingleOrDefault(e => e.BoardId == id);
            FilterBoardByTasksType(board, type);
            FilterBoardByTaskName(board, taskName);
            FilterBoardByTasksTime(board, time);
            FilterBoardByTasksUser(board, personName);
            return PartialView("PartialView/BoardDetail", board);
        }

        private void FilterBoardByTasksUser(Board board, string personName)
        {
            if (!String.IsNullOrEmpty(personName))
            {
                personName = personName.ToLower();
                List<Int32> foundUsersIds = db.Users.Where(u => u.Name.Contains(personName) || u.Email.Contains(personName)).Select(t => t.UserId).ToList();

                foreach (Column boardColumn in board.Columns)
                {
                    boardColumn.Tasks = boardColumn.Tasks.Where(t => (t.AssignedTo != null && foundUsersIds.Contains((Int32)t.AssignedTo)) 
                    || (t.CreatedBy != null && foundUsersIds.Contains((Int32)t.CreatedBy))).ToList();
                }
            }
        }
        
        private void FilterBoardByTasksTime(Board board, string time)
        {
            if (time != "all")
            {
                DateTime? endDate;
                switch (time)
                {
                    case "overdue":
                        endDate = DateTime.UtcNow;
                        break;
                    case "tomorrow":
                        endDate = DateTime.UtcNow.AddDays(1);
                        break;
                    case "due-next-week":
                        endDate = DateTime.UtcNow.AddDays(7);
                        break;
                    default:
                        endDate = null;
                        break;
                }

                foreach (Column boardColumn in board.Columns)
                {
                    if (endDate == null)
                        boardColumn.Tasks = boardColumn.Tasks.Where(t => t.Deadline == null).ToList();
                    else
                        boardColumn.Tasks = boardColumn.Tasks.Where(t => t.Deadline < endDate).ToList();
                }
            }
        }

        private void FilterBoardByTaskName(Board board, string taskName)
        {
            taskName = taskName.ToLower();
            foreach (Column boardColumn in board.Columns)
            {
                boardColumn.Tasks = boardColumn.Tasks.Where(t => t.Title.ToLower().Contains(taskName)).ToList();
            }
        }

        private void FilterBoardByTasksType(Board board, string type)
        {
            foreach (Column boardColumn in board.Columns)
            {
                boardColumn.Tasks = boardColumn.Tasks.Where(t => ShouldTaskBeIncluded(t, type)).ToList();
            }
        }

        private bool ShouldTaskBeIncluded(Task task, string type)
        {
            if (type == "all-tasks")
            {
                return true;
            }
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
            List<User> teamMembers =
                db.TeamMembers.Include(t => t.User).Where(t => t.BoardId == id).Select(t => t.User).ToList();

            ViewBag.TeamMembers = teamMembers;
            return View(board);
        }

        // POST: Boards/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Board board)
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
