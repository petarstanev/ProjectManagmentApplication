using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProjectManagementApplication.Models;
using ProjectManagementApplication.ViewModels;

namespace ProjectManagementApplication.Controllers
{
    [Authorize]
    public class ImagesController : Controller
    {
        private Context db = new Context();

        public ActionResult Upload(int id)
        {
            ImageViewModel image = new ImageViewModel { TaskId = id };
            return View(image);
        }

        // GET: Images/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Image image = db.Images.Find(id);
            if (image == null)
            {
                return HttpNotFound();
            }
            return View(image);
        }

        // POST: Images/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Image image = db.Images.Find(id);
            db.Images.Remove(image);
            db.SaveChanges();

            string filePath = Server.MapPath(image.Url);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            return RedirectToAction("Details", "Tasks", new { id = image.TaskId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Upload(ImageViewModel model)
        {
            var validImageTypes = new string[]
            {
                "image/gif",
                "image/jpeg",
                "image/pjpeg",
                "image/png"
            };

            if (model.ImageUpload == null || model.ImageUpload.ContentLength == 0)
            {
                ModelState.AddModelError("ImageUpload", "This field is required");
            }
            else if (!validImageTypes.Contains(model.ImageUpload.ContentType))
            {
                ModelState.AddModelError("ImageUpload", "Please choose either a GIF, JPG or PNG image.");
            }
            else if (model.ImageUpload.ContentLength / 1024 / 1024 > 3)
            {
                ModelState.AddModelError("ImageUpload", "This field is bigger than 3 megabytes.");
            }

            if (ModelState.IsValid)
            {
                var image = new Image();


                if (model.ImageUpload != null && model.ImageUpload.ContentLength > 0)
                {
                    string uploadDir = "/Content/images/taskimages";
                    string fileName = model.TaskId + "_" + Guid.NewGuid().ToString().Substring(0, 4) + Path.GetExtension(model.ImageUpload.FileName);

                    var imagePath = Path.Combine(Server.MapPath(uploadDir), fileName);
                    var imageUrl = Path.Combine(uploadDir, fileName);
                    model.ImageUpload.SaveAs(imagePath);
                    image.Url = imageUrl;
                }

                image.TaskId = model.TaskId;
                db.Images.Add(image);
                db.SaveChanges();
                return RedirectToAction("Details","Tasks",new {id = image.TaskId});
            }

            return View(model);
        }
    }
}
