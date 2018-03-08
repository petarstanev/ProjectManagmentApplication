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
    [Authorize]
    public class TeamMembersController : Controller
    {
        private Context db = new Context();
        [HttpGet]
        public ActionResult AddTeamMember(int id)
        {
            Board board = db.Boards.Find(id);
            if (board == null)
            {
                return HttpNotFound();
            }
            TeamMember teamMember = new TeamMember { BoardId = board.BoardId, Board = board };

            List<int> notAvailableUsersId = db.TeamMembers.Where(t => t.BoardId == id).Select(t => t.UserId).ToList();
            SessionContext sx = new SessionContext();
            int userId = sx.GetUserId();
            List<User> availableUsers = db.Users.Where(b => !(notAvailableUsersId.Contains(b.UserId) || b.UserId == userId)).ToList();

            ViewBag.UserId = new SelectList(availableUsers, "UserId", "Email");
            return View(teamMember);
        }

        [HttpPost]
        public ActionResult AddTeamMember(TeamMember teamMember)
        {
            db.TeamMembers.Add(teamMember);
            db.SaveChanges();
            return RedirectToAction("Edit", "Boards", new { id = teamMember.BoardId });
        }

        [HttpGet]
        public ActionResult RemoveTeamMember(int? memberId, int? boardId)
        {
            if (memberId == null || boardId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            TeamMember teamMember =
                db.TeamMembers.Include(t => t.Board).Include(t => t.User)
                .SingleOrDefault(t => t.UserId == memberId && t.BoardId == boardId);

            if (teamMember == null)
            {
                return HttpNotFound();
            }

            return View(teamMember);
        }

        [HttpPost]
        public ActionResult RemoveTeamMember(TeamMember teamMember)
        {
            teamMember = db.TeamMembers.Find(teamMember.TeamMemberId);

            db.TeamMembers.Remove(teamMember);
            db.SaveChanges();
            return RedirectToAction("Edit", "Boards", new { id = teamMember.BoardId });
        }
    }
}
