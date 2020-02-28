using DataLayer.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DataLayer.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(string userUri = null,  string jsonArr = null)
        {
            ViewBag.UserUri = userUri;
            ViewBag.imgBackGround = jsonArr;
            return View();
        }

        public ActionResult Section()
        {
            ViewBag.Message = "Секции спортклуба";
            

            return View();
        }

        public ActionResult Politics()
        {
            ViewBag.Message = "Правила спортклуба";

            return View();
        }
    }
}