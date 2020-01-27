using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Sportclub;
using Sportclub.Entities;
using Sportclub.Repository;

namespace Sportclub.Controllers
{
    public class CoachesController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        
        public ActionResult Index()
        {
            var coaches = unitOfWork.Coaches.Include(nameof(User)).Include(nameof(Specialization));
            return View(coaches.ToList());
        }

        
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Coaches coaches = unitOfWork.Coaches.Include(nameof(User)).ToList().Find(c => c.Id == id);
            if (coaches == null)
            {
                return HttpNotFound();
            }
            return View(coaches);
        }

       
        public ActionResult Create()
        {
                var specializations = unitOfWork.Specialization.GetAll().ToList();
                specializations.Add(new Specialization { Title = "-- Добавить специализацию --" });
                ViewBag.Specializations = new SelectList(specializations, "Id", "Title");
                ViewBag.UserList = new SelectList(unitOfWork.Users.GetAll().Where(u=>u.Token.Contains("coache")).ToList(), "Id", "FullName");
                return View();
                      
        }
        //[HttpPost]
        //public ActionResult CreateSpec()
        //{
        //    if (ModelState.IsValid)
        //    {
        //        using (Model1 unitOfWork = new Model1())
        //        {
        //            Specialization specialization = new Specialization { Title = form["Title"] };
        //            unitOfWork.Specializations.Create(specialization);
        //            unitOfWork.Save();
        //            var specializations = unitOfWork.Specializations.ToList();
        //            ViewBag.Specializations = new SelectList(specializations, "Id", "Title");
        //            return new JsonResult {
        //                Data = specialization,
        //                JsonRequestBehavior = JsonRequestBehavior.AllowGet
        //            };
        //        }
        //    }
        //    return RedirectToAction("Index", unitOfWork.Coaches.Include(nameof(User)).ToList());
        //}

        [HttpPost]
        public ActionResult CreateSpec(Specialization specialization)
        {
            if (specialization == null)
                return HttpNotFound();
            if (ModelState.IsValid)
            {
               
                    unitOfWork.Specialization.Create(specialization);
                    unitOfWork.Specialization.Save();
                    var specializations = unitOfWork.Specialization.GetAll().ToList();
                    ViewBag.Specializations = new SelectList(specializations, "Id", "Title");
                    return new JsonResult  {
                        Data = specializations,
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                
            }
            return RedirectToAction("Index", unitOfWork.Coaches.Include(nameof(User)).ToList());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Coaches coaches)
        {
            if (coaches == null)
                return HttpNotFound();
            if(coaches.UserId == 0) {
                return new HttpNotFoundResult ("User is null!");
            }
            if (ModelState.IsValid)
            {
                unitOfWork.Coaches.Create(coaches);
                unitOfWork.Coaches.Save();
                return RedirectToAction("Index");
            }

            return View(coaches);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.SpecList = new SelectList(unitOfWork.Specialization.GetAll().ToList(), "Id", "Title");
            Coaches coaches = unitOfWork.Coaches.Include(nameof(User)).ToList().Find(c => c.Id == id);
            if (coaches == null)
            {
                return HttpNotFound();
            }
            return View(coaches);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Coaches coaches)  //в форму добавл. скрыт. поля model.UserId + model.User.Id
        {
            if (ModelState.IsValid)
            {
                unitOfWork.Coaches.Update(coaches); //здесь coaches.UserId == coaches.User.Id
                unitOfWork.Coaches.Save();
                return RedirectToAction("Index");
            }
            return View(coaches);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Coaches coaches = unitOfWork.Coaches.Include(nameof(User)).ToList().Find(c => c.Id == id);
            if (coaches == null)
            {
                return HttpNotFound();
            }
            return View(coaches);
        }

        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Coaches coaches = unitOfWork.Coaches.Include(nameof(User)).ToList().Find(c=>c.Id==id);
            unitOfWork.Coaches.Delete(coaches.Id);
            unitOfWork.Coaches.Save();
            return RedirectToAction("Index");
        }

       
    }
}
