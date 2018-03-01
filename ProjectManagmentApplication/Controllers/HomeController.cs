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

            List<Board> favouriteBoards = db.FavoriteBoards.Include(f => f.Board).Where(f => f.UserId == userId).Select(f => f.Board).ToList();
            
            HomePageBoards boards = new HomePageBoards();
            boards.Favourite = favouriteBoards;
            boards.PrivateBoards = db.Boards.Where(b => b.BoardType == BoardType.Private && b.UserId == userId).ToList();
            boards.TeamBoards = db.Boards.Where(b => b.BoardType == BoardType.Team && b.UserId == userId).ToList();
            boards.PublicBoards = db.Boards.Where(b => b.BoardType == BoardType.Public && b.UserId == userId).ToList();
            
            return View(boards);
        }        
    }
}