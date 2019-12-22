using System;
using System.Collections.Generic;
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
        public ActionResult DoAddNavMenu(NavMenu menu)
        {
            var db = new MPEntities();
            //AdminInfo info = Session["admin"] as AdminInfo;
            //menu.CreaterId = info.Id;// 从 登录信息中 获取 登录人的 Id【从登录Session 中获取】
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
        public ActionResult DoModifyNavMenu(NavMenu menu)
        {
            //创建数据对象
            var db = new MPEntities();
            //附加对象
            NavMenu temp = db.NavMenus.Attach(menu);
            AdminInfo info = Session["admin"] as AdminInfo;
            temp.ModifyerId = info.Id;// 从 登录信息中 获取 登录人的 Id【从登录Session 中获取】
            temp.ModifyTime = DateTime.Now;
            //修改对象在内存中的状态
            db.Entry<NavMenu>(temp).State = System.Data.Entity.EntityState.Modified;
            //完成保存操作
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
