using Mars_Project.Models;
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
                .Where(s=>s.IsDele == 0)
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
            trainInfo.CreateTime = DateTime.Now;
            db.TrainInfoes.Add(trainInfo);
            var codeData = db.SaveChanges();
            var msg = new {
                code = codeData,
            };
            return Json(msg);
        }


        [HttpPost]
        public ActionResult DoDeleInfo(int id) {
            var db = new MPEntities();
            TrainInfo trainInfo = db.TrainInfoes.Where(s => s.Id == id).FirstOrDefault();
            //移除对象
            // db.NavMenus.Remove(temp);
            trainInfo.IsDele = 1;
            
            int i = db.SaveChanges();
            var res = new { code = i };
            return Json(res);
        }


        public ActionResult ModifyInfo(int id) {
            var db = new MPEntities();
            var trainInfo = db.TrainInfoes.Where(s => s.Id == id).FirstOrDefault();
            return PartialView(trainInfo);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult DoModifyInfo(TrainInfo trainInfo) {
            var db = new MPEntities();
            TrainInfo temp = db.TrainInfoes.Attach(trainInfo);
            temp.ModifyTime = DateTime.Now;
            //修改对象在内存中的状态
            db.Entry<TrainInfo>(temp).State = System.Data.Entity.EntityState.Modified;
            int i = db.SaveChanges();
            var res = new { code = i };
            return Json(res);
        }

    }
}