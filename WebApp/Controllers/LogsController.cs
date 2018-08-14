using WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace WebApp.Controllers
{
    public class LogsController : Controller
    {
        private static LogsModel model = new LogsModel();

        public LogsController()
        {
            model.NotifyRefresh -= Refresh;
            model.NotifyRefresh += Refresh;
        }
        public void Refresh()
        {
            Logs();
        }
        // GET: Logs
        public ActionResult Logs()
        {
            ViewBag.Logs = model.Logs;
            return View(model);
        }
    }
}