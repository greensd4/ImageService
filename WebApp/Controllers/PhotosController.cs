using Communication.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class PhotosController : Controller
    {
        private static PhotosModel model = new PhotosModel();
        public PhotosController()
        {
            model.NotifyRefresh -= Refresh;
            model.NotifyRefresh += Refresh;
            model.AddPhotosToList();
           
        }
        public void Refresh()
        {
            model.AddPhotosToList();
            Photos();
        }
        // GET: Photos
        public ActionResult Photos()
        {
            ViewBag.Photos = model.Photos;
            return View(model);
        }
        public ActionResult EnlargePhoto()
        {
            return View(model);
        }
        public ActionResult ConfirmDeletePhoto()
        {
            return View(model);
        }
        public ActionResult ViewPhotoPressed(string name, string path, string thumbPath)
        {
            model.ClickedPhotoName = name;
            model.ClickedPhotoPath = path;
            model.ClickedPhotoThumbPath = thumbPath;
            return RedirectToAction("EnlargePhoto");
        }
        public ActionResult DeletePhotoPressed(string name, string path, string thumbPath)
        {
            model.ClickedPhotoName = name;
            model.ClickedPhotoPath = path;
            model.ClickedPhotoThumbPath = thumbPath;
            return RedirectToAction("ConfirmDeletePhoto");
        }
        public ActionResult BackToPhotos()
        {
            return RedirectToAction("Photos");
        }
        public ActionResult ApprovedDelete()
        {
            model.SendCommandToService(new CommandRecievedEventArgs((int)CommandEnum.RemovePhoto,
                new string[] { model.ClickedPhotoPath , model.ClickedPhotoThumbPath }, null));
            return BackToPhotos();
        }
        public ActionResult CanceledDelete()
        {
            return BackToPhotos();
        }
       
    }
}