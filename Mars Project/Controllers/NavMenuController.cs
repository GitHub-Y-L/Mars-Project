using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Mars_Project.Models;

namespace Mars_Project.Controllers
{

    public class NavMenuController : Controller
    {
        //
        // GET: /NavMenu/

        public ActionResult Index()
        {
            return View();
        }


        /// <summary>
        /// 用于获取数据分页
        /// </summary>
        /// <returns></returns>
        public ActionResult GetPageData(int page, int limit)
        {
            var db = new MPEntities();
            // 统计表中的记录数：select count(*) from [dbo].[NavMenus] where isdele=0
            int count = db.NavMenus.Where(s => s.IsDele == 0).Count();



            //lambda 表达式
            List<NavMenu> dataList = db.NavMenus
                .Where(s => s.IsDele == 0)
                   .OrderBy(s => s.Id)
                   .Skip((page - 1) * limit)
                   .Take(limit)
                   .ToList();//执行语句;

            //2.数据格式也不符合 layui 的要求

            var msg = new { code = 0, msg = "", count = count, data = dataList };
            return Json(msg);
        }

        public ActionResult GetPageDataOrderBy(int page, int limit)
        {
            var db = new MPEntities();
            // 统计表中的记录数：select count(*) from [dbo].[NavMenus] where isdele=0
            int count = db.NavMenus.Where(s => s.IsDele == 0).Count();



            //lambda 表达式
            List<NavMenu> dataList = db.NavMenus
                .Where(s => s.IsDele == 0)
                   .OrderBy(s => s.OrderBy)
                   .ThenBy(r => r.Id)
                   .Skip((page - 1) * limit)
                   .Take(limit)
                   .ToList();//执行语句;

            //2.数据格式也不符合 layui 的要求

            var msg = new { code = 0, msg = "", count = count, data = dataList };
            return Json(msg);
        }


        #region 新增


        /// <summary>
        /// 添加信息的界面
        /// </summary>
        /// <returns></returns>
        public ActionResult AddNavMenu()
        {
            return PartialView();
        }

        /// <summary>
        /// 保存 添加的信息
        /// </summary>
        /// <param name="stu"></param>
        /// <returns></returns>
        public ActionResult DoAddNavMenu(NavMenu menu, HttpPostedFileBase file)
        {
            var db = new MPEntities();

            if (file != null)
            {
                //用于同名图片处理，设置物理路径
                string fileName = "~/UploadFiles/" + DateTime.Now.ToString("yyyyMMddHHssmm") + Path.GetFileName(file.FileName);
                var physicsFileName = Server.MapPath(fileName);
                try
                {
                    //保存图片到对应目录
                    file.SaveAs(physicsFileName);
                    menu.TopBanner = fileName.Replace("~", "");
                }
                catch (Exception e)
                {
                    return Json(new { result = "false", code = 500, msg = "保存失败" }, "text/html");
                }
            }
            else
            {
                return Json(new { result = "false", code = 500, msg = "文件不存在" }, "text/html");
            }

            AdminInfo info = Session["admin"] as AdminInfo;
            menu.CreaterId = info.Id;
            menu.CreateTime = DateTime.Now;
            db.NavMenus.Add(menu);
            int i = db.SaveChanges();
            var msg = new { code = i };
            return Json(msg);
        }
        #endregion


        #region 修改
        /// <summary>
        /// 修改信息的界面
        /// </summary>
        /// <returns></returns>
        public ActionResult ModeifyNavMenu(int id)
        {
            var db = new MPEntities();
            var obj = db.NavMenus.Where(s => s.Id == id).FirstOrDefault();

            if (obj == null)
            {
                return Content("修改数据不存在！");
            }
            else
            {
                return PartialView(obj);
            }


        }

        /// <summary>
        /// 保存 的信息
        /// </summary>
        /// <param name="stu"></param>
        /// <returns></returns>
        public ActionResult DoModifyNavMenu(NavMenu menu, HttpPostedFileBase file)
        {
            //创建数据对象
            var db = new MPEntities();
            var tempOriginal = db.NavMenus.Where(s => s.Id == menu.Id).FirstOrDefault();

            if (file != null)
            {
                //用于同名图片处理，设置物理路径
                string fileName = "~/UploadFiles/" + DateTime.Now.ToString("yyyyMMddHHssmm") + Path.GetFileName(file.FileName);
                var physicsFileName = Server.MapPath(fileName);
                try
                {
                    //保存图片到对应目录
                    file.SaveAs(physicsFileName);
                    tempOriginal.TopBanner = fileName.Replace("~", "");
                }
                catch (Exception e)
                {
                    return Json(new { result = "false", code = 500, msg = "保存失败" }, "text/html");
                }
            }

            tempOriginal.MenuName = menu.MenuName;
            tempOriginal.LinkAddress = menu.LinkAddress;
            tempOriginal.OrderBy = menu.OrderBy;
            tempOriginal.IsDele = menu.IsDele;
            tempOriginal.CreaterId = menu.CreaterId;
            tempOriginal.CreateTime = menu.CreateTime;

            AdminInfo info = Session["admin"] as AdminInfo;
            tempOriginal.ModifyerId = info.Id;
            tempOriginal.ModifyTime = DateTime.Now;
            //修改对象在内存中的状态
            db.Entry<NavMenu>(tempOriginal).State = EntityState.Modified;
            int i = db.SaveChanges();
            var msg = new { code = i };
            return Json(msg);
        }

        #endregion

        #region 删除

        /// <summary>
        /// 删除信息
        /// </summary>
        /// <param name="id">主键Id</param>
        /// <returns></returns>
        public ActionResult DoDele(int id)
        {
            //创建数据对象
            var db = new MPEntities();
            //根据传入id 查找 对象
            NavMenu temp = db.NavMenus.Where(s => s.Id == id).FirstOrDefault();
            //移除对象
            // db.NavMenus.Remove(temp);
            //设置 是否删除 字段 为1，标识删除
            temp.IsDele = 1;

            //保存到数据库中
            int i = db.SaveChanges();
            var msg = new { code = i };

            return Json(msg);
        }
        #endregion
    }
}
