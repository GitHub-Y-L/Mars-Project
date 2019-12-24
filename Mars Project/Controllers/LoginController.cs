using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mars_Project.Models;

namespace Mars_Project.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DoLogin(string loginName, string pwd)
        {
            var db = new MPEntities();
            AdminInfo info = db.AdminInfoes.Where(a => a.UserName == loginName && a.Password == pwd).FirstOrDefault();
            if (info == null)
            {
                var res = new { code = -1, msg = "登录失败，用户名或密码错误！" };
                return Json(res);
            }
            else
            {
                //用户信息存入Session 
                Session["admin"] = info;
                var res = new { code = 1, msg = "登录成功", link = "/Backstage/Index" };
                return Json(res);
            }

        }

        public ActionResult SignOut() {
            Session["admin"] = null;
            return Redirect("/");
        }
    }
}