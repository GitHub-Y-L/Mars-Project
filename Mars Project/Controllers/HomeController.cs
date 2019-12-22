using Mars_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mars_Project.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About() {
            return View();
        }

        public ActionResult TrainInfo() {
            return View();
        }

        public ActionResult TrainInfoOne(int id) {
            var db = new MPEntities();
            TrainInfo info = db.TrainInfoes
                .Where(s => s.IsDele == 0 && s.Id == id)
                .FirstOrDefault();

            ViewBag.TrainInfoOne = info;
            return View();
        }
    }
}