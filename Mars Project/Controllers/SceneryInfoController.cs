using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mars_Project.Models;
using System.Data;
using System.IO;

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

        [ValidateInput(false)]
        public ActionResult DoAdd(SceneryInfo scenery, HttpPostedFileBase file) {
            if (file == null)
            {
                return Json(new { result = "false", code = 400, msg = "文件不存在" }, "text/html");
            }

            //用于同名图片处理，设置物理路径
            string fileName = "~/UploadFiles/" + DateTime.Now.ToString("yyyyMMddHHssmm") + Path.GetFileName(file.FileName);
            var physicsFileName = Server.MapPath(fileName);
            try
            {
                //保存图片到对应目录
                file.SaveAs(physicsFileName);

                using (var db = new MPEntities())
                {
                    scenery.TopBanner = fileName.Replace("~", "");
                    scenery.CreateTime = DateTime.Now;
                    db.SceneryInfoes.Add(scenery);
                    int i = db.SaveChanges();
                    var res = new { code = i };
                }
                return Json(new { result = "true", msg = "上传成功", imgUrl = fileName });
            }
            catch (Exception ex)
            {
                return Json(new { result = "false", code = 500, msg = "保存失败" }, "text/html");
            }
        }


        public ActionResult Update(int id) {
            var db = new MPEntities();
            //修改对象的信息
            var m = db.SceneryInfoes.Where(s => s.Id == id).FirstOrDefault();
            return PartialView(m);
        }


        [ValidateInput(false)]
        public ActionResult DoUpdate(SceneryInfo scenery, HttpPostedFileBase file) {
            var db = new MPEntities();
            var Scenery= db.SceneryInfoes.Where(s => s.Id == scenery.Id).FirstOrDefault();

            //用于同名图片处理，设置物理路径
            if (file!=null) { 
                string fileName = "~/UploadFiles/" + DateTime.Now.ToString("yyyyMMddHHssmm") + Path.GetFileName(file.FileName);
                var physicsFileName = Server.MapPath(fileName);
                try
             {
                    //保存图片到对应目录
                 file.SaveAs(physicsFileName);
                 Scenery.TopBanner = fileName.Replace("~", "");
                }catch (Exception ex)
               {
                    return Json(new { result = "false", code = 500, msg = "修改失败" }, "text/html");
               }
             }
                Scenery.Title = scenery.Title;
                Scenery.Value = scenery.Value;
                Scenery.UEValue = scenery.UEValue;
                Scenery.ModifyTime = DateTime.Now;
                db.Entry<SceneryInfo>(Scenery).State = System.Data.Entity.EntityState.Modified;
                int i = db.SaveChanges();
                var res = new { code = i };
                return Json(new { result = "true", msg = "修改成功" });
           
            
        }

        public ActionResult Delete(int id) {
            var db = new MPEntities();
            //根据传入id 查找 对象
            SceneryInfo scenery = db.SceneryInfoes.Where(s => s.Id == id).FirstOrDefault();
            //设置 是否删除 字段 为1，标识删除
            scenery.IsDele = 1;

            //保存到数据库中
            int i = db.SaveChanges();
            var res = new { code = i };
            return Json(res);
        }
    }
}