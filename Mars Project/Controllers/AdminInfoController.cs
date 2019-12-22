using System;
using System.Linq;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web;

using Mars_Project.Models;

namespace Mars_Project.Controllers {
    public class AdminInfoController : Controller {
        public ActionResult Index() {
            return View();
        }

        public ActionResult GetPageData(int page, int limit) {
            var db = new MPEntities();
            int countData = db.AdminInfoes.Count();
            List<AdminInfo> dataList = db.AdminInfoes
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

        #region 新增

        public ActionResult AddStu() {
            return PartialView();
        }

        public ActionResult DoAddStu(AdminInfo stu) {
            var db = new MPEntities();
            db.AdminInfoes.Add(stu);
            int i = db.SaveChanges();
            var msg = new { code = i };
            return Json(msg);
        }

        #endregion

        #region 修改

        public ActionResult ModeifyStu(int id) {
            var db = new MPEntities();
            var obj = db.AdminInfoes.Where(s => s.Id == id).FirstOrDefault();

            if (obj == null) {
                return Content("修改数据不存在！");
            } else {
                return PartialView(obj);
            }
        }

        public ActionResult DoModifyStu(AdminInfo stu) {
            //创建数据对象
            var db = new MPEntities();
            //附加对象
            AdminInfo temp = db.AdminInfoes.Attach(stu);
            //修改对象在内存中的状态 
            db.Entry<AdminInfo>(temp).State = System.Data.Entity.EntityState.Modified;
            //完成保存操作
            int i = db.SaveChanges();
            var msg = new { code = i };
            return Json(msg);
        }

        #endregion

        #region 删除

        public ActionResult DoDele(int id) {
            //创建数据对象
            var db = new MPEntities();
            //根据传入id 查找 对象
            AdminInfo temp = db.AdminInfoes.Where(s => s.Id == id).FirstOrDefault();
            //移除对象
            db.AdminInfoes.Remove(temp);
            //保存到数据库中
            int i = db.SaveChanges();
            var msg = new { code = i };

            return Json(msg);
        }

        #endregion

    }
}
