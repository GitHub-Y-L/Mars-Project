﻿using Mars_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mars_Project.Controllers
{
    public class TrainInfoController : Controller
    {
        // GET: TrainInfo
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetPageInfo(int page, int limit) {
            var db = new MPEntities();
            int countData = db.TrainInfoes.Count();
            List<TrainInfo> dataList = db.TrainInfoes
                .OrderBy(s => s.Id)
                .Skip((page - 1) * limit)
                .Take(limit)
                .ToList();

            var msg = new {
                code = 0,
                msg = "",
                count = countData,
                data = dataList
            };
            return Json(msg);
        }

        public ActionResult AddInfo() {
            return PartialView();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult DoAddInfo(TrainInfo trainInfo) {
            var db = new MPEntities();
            //db.TrainInfo.Add(trainInfo);
            //var codeData = db.SaveChanges();
            //var msg = new {
            //    code = codeData,
            //};
            return Json(0);
        }

    }
}