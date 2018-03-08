using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProjectManagementApplication.Models;
using ProjectManagementApplication.ViewModels;
using System.Data.Entity;

namespace ProjectManagementApplication.Controllers
{
    public class HomeController : Controller
    {
        private Context db = new Context();

        [Authorize]
        public ActionResult Index()
        {
            SessionContext sx = new SessionContext();
            int userId = sx.GetUserId();
            HomePageBoards boards = new HomePageBoards();

            boards.Favourite = db.FavoriteBoards.Include(f => f.Board).Where(f => f.UserId == userId).Select(f => f.Board).ToList();
            boards.PrivateBoards = db.Boards.Where(b => b.BoardType == BoardType.Private && b.UserId == userId).ToList();
            List<int> teamMembers = db.TeamMembers.Where(u => u.UserId == userId).Select(b => b.BoardId).ToList();
            boards.TeamBoards = db.Boards.Where(b => b.BoardType == BoardType.Team && (teamMembers.Contains(b.BoardId) || b.UserId == userId)).ToList();
            boards.PublicBoards = db.Boards.Where(b => b.BoardType == BoardType.Public && b.UserId == userId).ToList();

            return View(boards);
        }
    }
}