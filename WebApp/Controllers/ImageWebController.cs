using WebApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    public class ImageWebController : Controller
    {
        private static ImageWebModel model = new ImageWebModel();
        public ImageWebController()
        {
            ConfigModel con = ConfigModel.GetInstance;
            model.NotifyRefresh -= Refresh;
            model.NotifyRefresh += Refresh;
        }
        public void Refresh()
        {
            ImageWeb();
        }
        // GET: ImageWeb
        public ActionResult ImageWeb()
        {
            return View(model);
        }
    }
}