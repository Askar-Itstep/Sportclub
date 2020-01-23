using Sportclub.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sportclub.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Section()
        {
            ViewBag.Message = "Секции спортклуба";
            using(Model1 db = new Model1())
            {
                //var specializations = db.Coaches.Select(c=>c.Specialization).Distinct().ToList();
                //specializations.ForEach(s=>System.Diagnostics.Debug.WriteLine(s));
            }

            return View();
        }

        public ActionResult Politics()
        {
            ViewBag.Message = "Правила спортклуба";

            return View();
        }
    }
}