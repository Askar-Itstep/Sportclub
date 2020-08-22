﻿using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DataLayer.Controllers
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
            

            return View();
        }

        //public ActionResult Politics()
        //{
        //    ViewBag.Message = "Правила спортклуба";

        //    return View();
        //}

        public ActionResult Politics()
        {
            //ViewBag.Message = "Key vault value = " + ConfigurationManager.AppSettings["vaultName"];
            ViewBag.Message = "Key vault Uri = " + ConfigurationManager.AppSettings["DbConnect"];
            return View();
        }
    }
}