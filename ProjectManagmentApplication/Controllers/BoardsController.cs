using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ProjectManagementApplication.Models;
using PagedList;

namespace ProjectManagementApplication.Controllers
{
    public class BoardsController : Controller
    {
        private Context db = new Context();

        public ActionResult Index(string sortType, string currentFilter, string searchString, int? page)
        {
            ViewBag.TitleParm = sortType == "Title" ? "Title_desc" : "Title";
            ViewBag.AdministratorParm = sortType == "Administrator" ? "Administrator_desc" : "Administrator";
            ViewBag.Type = sortType == "Type" ? "Type_desc" : "Type";
            string[] boardTypes = { "Private", "Team", "Public" };
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;
            List<Board> boards = GetVisibleBoards();

            if (!String.IsNullOrEmpty(searchString))
            {
                if (boardTypes.Contains(searchString))
                {
                    BoardType boardType = (BoardType)Enum.Parse(typeof(BoardType), searchString);
                    boards = boards.Where(b => b.BoardType == boardType).ToList();
                }
                else
                {
                    boards = boards.Where(b => b.Title.ToUpper().Contains(searchString.ToUpper()) ||
                    b.User.Name.ToUpper().Contains(searchString.ToUpper())).ToList();
                }
            }

            switch (sortType)
            {
                case "Title":
                    boards = boards.OrderBy(b => b.Title).ToList();
                    break;
                case "Title_desc":
                    boards = boards.OrderByDescending(b => b.Title).ToList();
                    break;
                case "Administrator":
                    boards = boards.OrderBy(b => b.User.Name).ToList();
                    break;
                case "Administrator_desc":
                    boards = boards.OrderByDescending(b => b.User.Name).ToList();
                    break;
                case "Type":
                    boards = boards.OrderBy(b => b.BoardType).ToList();
                    break;
                case "Type_desc":
                    boards = boards.OrderByDescending(b => b.BoardType).ToList();
                    break;
                default:
                    boards = boards.OrderBy(b => b.BoardId).ToList();
                    break;
            }

            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(boards.ToPagedList(pageNumber, pageSize));
        }

        private List<Board> GetVisibleBoards()
        {
            SessionContext sx = new SessionContext();
            int userId = sx.GetUserId();
            List<Board> boards;
            if (userId > 0)
            {
                List<int> teamMembersIds = db.TeamMembers.Where(u => u.UserId == userId).Select(b => b.BoardId).ToList();
                boards = db.Boards.Include(c => c.User).Where(b => (b.BoardType == BoardType.Private && b.UserId == userId)
                || (b.BoardType == BoardType.Team && (b.UserId == userId || teamMembersIds.Contains(b.BoardId)))
                || b.BoardType == BoardType.Public).ToList();
            }
            else
            {
                boards = db.Boards.Include(c => c.User).Where(b => b.BoardType == BoardType.Public).ToList();
            }

            return boards;
        }

        // GET: Boards/Details/5
        public ActionResult Details(int? id)
        {
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
            SessionContext sx = new SessionContext();

            ViewBag.Administrator = board.UserId == sx.GetUserId();


            return View(board);
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
            //Board board = db.Boards.Find(id);
            //board.Columns =
            //    db.Columns.Include(c => c.Tasks.Select(t => t.AssignedToUser)).Where(c => c.BoardId == id).ToList();

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
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Boards/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
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
                return RedirectToAction("Details",new {id = board.BoardId});
            }

            return View(board);
        }

        // GET: Boards/Edit/5
        [Authorize]
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
            if (!CheckAdministrator(board))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

            List<User> members =
                db.TeamMembers.Include(t => t.User).Where(t => t.BoardId == id).Select(t => t.User).ToList();

            ViewBag.UserId = new SelectList(db.Users, "UserId", "Email", board.UserId);
            ViewBag.TeamMembers = members;
            return View(board);
        }

        // POST: Boards/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit(Board board)
        {
            if (ModelState.IsValid)
            {
                db.Entry(board).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new { id = board.BoardId });
            }
            List<User> members =
               db.TeamMembers.Include(t => t.User).Where(t => t.BoardId == board.BoardId).Select(t => t.User).ToList();
            ViewBag.UserId = new SelectList(db.Users, "UserId", "Email", board.User);
            ViewBag.TeamMembers = members;
            return View(board);
        }

        // GET: Boards/Delete/5
        [Authorize]
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
            if (!CheckAdministrator(board))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            return View(board);
        }

        // POST: Boards/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
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
                    if (currentUser == null)
                        return false;

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
            return board.UserId == sx.GetUserId();
        }
    }
}
