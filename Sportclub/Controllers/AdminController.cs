using Sportclub.Entities;
using Sportclub.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sportclub.Controllers
{
    public class AdminController : Controller
    {
        private UnitOfWork unitOfWork;
        public AdminController()
        {
            unitOfWork = new UnitOfWork();
        }

        [Authorize(Roles = "admin, top_manager, manager")] //CustomRoleProvider!
        public ActionResult Index()
        {
            var admins = unitOfWork.Administration.GetAll().ToList();   //virtual!
            return View(admins);


        }

        public ActionResult Details(int? id)
        {
            if (id == null)
                return HttpNotFound();
            var admin = unitOfWork.Administration.GetById(id);
            return View(admin);
            //}
        }

        public ActionResult Create()
        {
            ViewBag.UserList = new SelectList(unitOfWork.Users.GetAll()
                .Where(u => u.Token == null).ToList(), "Id", "FullName");
            return View();

        }


        [HttpPost]
        public ActionResult Create(Administration manager)
        {
            try {
                unitOfWork.Administration.Create(manager);
                unitOfWork.Administration.Save();
                return RedirectToAction("Index");
            }
            catch {
                return View(manager);
            }
        }


        public ActionResult Edit(int? id)
        {
            if (id == null)
                return HttpNotFound();
            var admin = unitOfWork.Administration.Include("User").ToList().Find(a => a.Id == id);
            return View(admin);

        }

        [HttpPost]
        public ActionResult Edit(Administration manager)
        {
            try {
                unitOfWork.Administration.Update(manager);
                unitOfWork.Administration.Save();
                return RedirectToAction("Index");
                //}                
            }
            catch (Exception e) {
                System.Diagnostics.Debug.WriteLine("error: " + e.Message);
                return View(manager);
            }
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
                return HttpNotFound();
            var admin = unitOfWork.Administration.Include("User").ToList().Find(a => a.Id == id);
            return View(admin);

        }

        [HttpPost]
        public ActionResult Delete(Administration manager)
        {
            try {

                unitOfWork.Administration.Delete(manager.Id);
                unitOfWork.Administration.Save();
                return RedirectToAction("Index");
            }

            catch {
                return View(manager);
            }
        }
    }
}
