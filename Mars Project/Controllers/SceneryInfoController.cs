using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mars_Project.Models;

namespace Mars_Project.Controllers
{
    public class SceneryInfoController : Controller
    {
        // GET: SceneryInfo
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Select(int page, int limit) {
            var db = new MPEntities();

            int count = db.SceneryInfoes.Where(s => s.IsDele == 0).Count();

            List<SceneryInfo> list = db.SceneryInfoes.Where(s => s.IsDele == 0)
                                             .OrderBy(s=>s.Id)
                                             .Skip((page - 1) * limit)//跳过 的行数
                                             .Take(limit)//提取 数据的条数
                                             .ToList();//执行语句

            var res = new { code = 0, msg = "", count = count, data = list };
            return Json(res);
        }
        public ActionResult Add() {

            return PartialView();
        }

        public ActionResult DoAdd() {
            return View();
        }
    }
}