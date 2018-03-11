using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProjectManagementApplication.Models;
using ProjectManagementApplication.ViewModels;
using ProjectManagmentApplication.Hubs;
using Image = ProjectManagementApplication.Models.Image;

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
            TaskHub.TaskUpdated(image.TaskId);
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

                using (var img = System.Drawing.Image.FromStream(model.ImageUpload.InputStream))
                {
                    string uploadDir = "/Content/images/taskimages";
                    string fileName = model.TaskId + "_" + Guid.NewGuid().ToString().Substring(0, 4) + Path.GetExtension(model.ImageUpload.FileName);

                    //var imagePath = Path.Combine(Server.MapPath(uploadDir), fileName);
                    var imageUrl = Path.Combine(uploadDir, fileName);
                   
                    SaveToFolder(img, new Size(1920, 1080), imageUrl);
                    image.Url = imageUrl;
                }

                image.TaskId = model.TaskId;
                db.Images.Add(image);
                db.SaveChanges();
                TaskHub.TaskUpdated(image.TaskId);


                return RedirectToAction("Details","Tasks",new {id = image.TaskId});
            }

            return View(model);
        }

        private void SaveToFolder(System.Drawing.Image img, Size newSize, string pathToSave)
        {
            Size imgSize = NewImageSize(img.Size, newSize);
            using (System.Drawing.Image newImg = new Bitmap(img, imgSize.Width, imgSize.Height))
            {
                newImg.Save(Server.MapPath(pathToSave), img.RawFormat);
            }
        }

        private Size NewImageSize(Size imageSize, Size newSize)
        {
            Size finalSize;
            double tempval;
            if (imageSize.Height > newSize.Height || imageSize.Width > newSize.Width)
            {
                if (imageSize.Height > imageSize.Width)
                    tempval = newSize.Height / (imageSize.Height * 1.0);
                else
                    tempval = newSize.Width / (imageSize.Width * 1.0);

                finalSize = new Size((int)(tempval * imageSize.Width), (int)(tempval * imageSize.Height));
            }
            else
                finalSize = imageSize; 

            return finalSize;
        }

    }
}
