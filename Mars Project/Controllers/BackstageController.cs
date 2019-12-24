using Mars_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mars_Project.Controllers
{
    public class BackstageController : Controller
    {
        public ActionResult Index()
        {
            var db = new MPEntities();
            ViewBag.AdminInfoCount = db.AdminInfoes.Count();
            ViewBag.NavMenuCount = db.NavMenus.Where(s => s.IsDele == 0).Count();
            ViewBag.SceneryInfoCount = db.SceneryInfoes.Where(s => s.IsDele == 0).Count();
            ViewBag.TrainInfoCount = db.TrainInfoes.Where(s => s.IsDele == 0).Count();
            ViewBag.MovieInfoCount = db.MovieInfoes.Where(s => s.IsDele == 0).Count();

            return View();
        }
    }
}