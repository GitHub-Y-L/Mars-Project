using Mars_Project.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mars_Project.Controllers
{
    public class MovieInfoController : Controller
    {
        // GET: MovieInfo
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetPageInfo(int page, int limit) {
            var db = new MPEntities();
            int countData = db.MovieInfoes.Count();
            List<MovieInfo> dataList = db.MovieInfoes
                .Where(s => s.IsDele == 0)
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

        public ActionResult AddInfo() {
            return PartialView();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult DoAddInfo(MovieInfo movieInfo, HttpPostedFileBase file) {
            var db = new MPEntities();

            if (file != null) {
                //用于同名图片处理，设置物理路径
                string fileName = "~/UploadFiles/" + DateTime.Now.ToString("yyyyMMddHHssmm") + Path.GetFileName(file.FileName);
                var physicsFileName = Server.MapPath(fileName);
                try {
                    //保存图片到对应目录
                    file.SaveAs(physicsFileName);
                    movieInfo.Image = fileName.Replace("~", "");
                } catch (Exception e) {
                    return Json(new { result = "false", code = 500, msg = "保存失败" }, "text/html");
                }
            } else {
                return Json(new { result = "false", code = 500, msg = "文件不存在" }, "text/html");
            }

            movieInfo.CreateTime = DateTime.Now;
            db.MovieInfoes.Add(movieInfo);
            var codeData = db.SaveChanges();
            var msg = new {
                code = codeData,
            };
            return Json(msg);
        }

        #endregion

        #region 删除

        [HttpPost]
        public ActionResult DoDeleInfo(int id) {
            var db = new MPEntities();
            MovieInfo movieInfo = db.MovieInfoes.Where(s => s.Id == id).FirstOrDefault();
            //移除对象
            // db.NavMenus.Remove(temp);
            movieInfo.IsDele = 1;

            int i = db.SaveChanges();
            var res = new { code = i };
            return Json(res);
        }

        #endregion

        #region 修改

        public ActionResult ModifyInfo(int id) {
            var db = new MPEntities();
            MovieInfo movieInfo = db.MovieInfoes.Where(s => s.Id == id).FirstOrDefault();
            return PartialView(movieInfo);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult DoModifyInfo(MovieInfo movieInfo, HttpPostedFileBase file) {
            var db = new MPEntities();
            MovieInfo tempOriginal = db.MovieInfoes.Where(s => s.Id == movieInfo.Id).FirstOrDefault();

            if (file != null) {
                //用于同名图片处理，设置物理路径
                string fileName = "~/UploadFiles/" + DateTime.Now.ToString("yyyyMMddHHssmm") + Path.GetFileName(file.FileName);
                var physicsFileName = Server.MapPath(fileName);
                try {
                    //保存图片到对应目录
                    file.SaveAs(physicsFileName);
                    tempOriginal.Image = fileName.Replace("~", "");
                } catch (Exception e) {
                    return Json(new { result = "false", code = 500, msg = "保存失败" }, "text/html");
                }
            }

            tempOriginal.Name = movieInfo.Name;
            tempOriginal.NameEn = movieInfo.NameEn;
            tempOriginal.Rating = movieInfo.Rating;
            tempOriginal.Cast = movieInfo.Cast;
            tempOriginal.Direction = movieInfo.Direction;
            tempOriginal.Genre = movieInfo.Genre;
            tempOriginal.Duration = movieInfo.Duration;
            tempOriginal.ReLink = movieInfo.ReLink;
            tempOriginal.IsDele = movieInfo.IsDele;

            tempOriginal.ModifyTime = DateTime.Now;
            //修改对象在内存中的状态
            db.Entry<MovieInfo>(tempOriginal).State = EntityState.Modified;
            int i = db.SaveChanges();
            var res = new { code = i };
            return Json(res);
        }

        #endregion


    }
}