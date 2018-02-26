using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProjectManagementApplication.Models;

namespace ProjectManagmentApplication.Controllers
{
    public class FavoritesController : Controller
    {
        Context db = new Context();
        SessionContext sx = new SessionContext();

        [HttpPost]
        public ActionResult AddToFavorite(int? boardId)
        {
            if (boardId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Board board = db.Boards.Find(boardId);
            if (board == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FavoriteBoard favorite = new FavoriteBoard();

            favorite.BoardId = (int) boardId;
            favorite.UserId = sx.GetUserId();

            db.FavoriteBoards.Add(favorite);
            db.SaveChanges();

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpPost]
        public ActionResult RemoveFromFavorite(int boardId)
        {
            int userId = sx.GetUserId();
            FavoriteBoard favorite = db.FavoriteBoards.FirstOrDefault(f => f.UserId == userId && f.BoardId == boardId);

            if (favorite == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            db.FavoriteBoards.Remove(favorite);
            db.SaveChanges();

            return new HttpStatusCodeResult(HttpStatusCode.OK);
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