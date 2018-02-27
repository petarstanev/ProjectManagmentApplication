using System.Linq;
using System.Net;
using System.Web.Mvc;
using ProjectManagementApplication.Models;

namespace ProjectManagmentApplication.Controllers
{
    public class FavoritesController : Controller
    {
        Context db = new Context();
        SessionContext sx = new SessionContext();

        [HttpPost]
        public ActionResult ChangeFavorite(int boardId)
        {
            int userId = sx.GetUserId();
            FavoriteBoard favorite = db.FavoriteBoards.FirstOrDefault(f => f.UserId == userId && f.BoardId == boardId);
            if (favorite == null)
            {
                AddToFavorite(userId, boardId);
            }
            else
            {
                RemoveFromFavorite(favorite);
            }
            
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        private void AddToFavorite(int userId, int boardId)
        {
            FavoriteBoard favorite = new FavoriteBoard
            {
                BoardId = boardId,
                UserId = userId
            };

            db.FavoriteBoards.Add(favorite);
            db.SaveChanges();
        }

        private void RemoveFromFavorite(FavoriteBoard favorite)
        {
            db.FavoriteBoards.Remove(favorite);
            db.SaveChanges();
        }

        [HttpGet]
        public ActionResult IsFavorite(int? boardId)
        {
            int userId = sx.GetUserId();
            FavoriteBoard favorite = db.FavoriteBoards.FirstOrDefault(f => f.UserId == userId && f.BoardId == boardId);
            bool result = favorite != null;

            return Json(new { favourite = result },JsonRequestBehavior.AllowGet);
        }
    }
}