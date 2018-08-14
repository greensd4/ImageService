using Communication.Infrastructure;
using WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    public class ConfigController : Controller
    {
        #region Members
        private static ConfigModel model = ConfigModel.GetInstance;
        private static string deleteHandler;
        #endregion
        public ConfigController()
        {
            model.NotifyRefresh -= Refresh;
            model.NotifyRefresh += Refresh;
        }
        public void Refresh()
        {
            Config();
        }
        // GET: Config
        public ActionResult Config()
        {
            return View(model);
        }
        // GET: Config
        public ActionResult Confirm()
        {
            return View(model);
        }
        public ActionResult ClickedOnHandler(string clickedHandler)
        {
            deleteHandler = clickedHandler;
            model.DeleteHandler = deleteHandler;
            return RedirectToAction("Confirm");
        }
        public ActionResult ApprovedDelete()
        {
            model.SendCommandToService(new CommandRecievedEventArgs((int)CommandEnum.CloseCommand, new string[] { deleteHandler }, null));
            return RedirectToAction("Config");
        }
        public ActionResult CanceledDelete()
        {
            return RedirectToAction("Config");
        }
    }
}