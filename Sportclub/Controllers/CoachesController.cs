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

namespace Sportclub.Controllers
{
    public class CoachesController : Controller
    {
        private Model1 db = new Model1();

        
        public ActionResult Index()
        {
            return View(db.Coaches.Include(nameof(User)).Include(nameof(Specialization)).ToList());
        }

        
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Coaches coaches = db.Coaches.Include(nameof(User)).ToList().Find(c => c.Id == id);
            if (coaches == null)
            {
                return HttpNotFound();
            }
            return View(coaches);
        }

       
        public ActionResult Create()
        {
            using(Model1 db = new Model1())
            {
                var specializations = db.Specializations.ToList();
                specializations.Add(new Specialization { Title = "-- Добавить специализацию --" });
                ViewBag.Specializations = new SelectList(specializations, "Id", "Title");
                ViewBag.UserList = new SelectList(db.Users.Where(u=>u.Token.Contains("coache")).ToList(), "Id", "FullName");
                return View();
            }            
        }
        //[HttpPost]
        //public ActionResult CreateSpec()
        //{
        //    if (ModelState.IsValid)
        //    {
        //        using (Model1 db = new Model1())
        //        {
        //            Specialization specialization = new Specialization { Title = form["Title"] };
        //            db.Specializations.Add(specialization);
        //            db.SaveChanges();
        //            var specializations = db.Specializations.ToList();
        //            ViewBag.Specializations = new SelectList(specializations, "Id", "Title");
        //            return new JsonResult {
        //                Data = specialization,
        //                JsonRequestBehavior = JsonRequestBehavior.AllowGet
        //            };
        //        }
        //    }
        //    return RedirectToAction("Index", db.Coaches.Include(nameof(User)).ToList());
        //}

        [HttpPost]
        public ActionResult CreateSpec(Specialization specialization)
        {
            if (specialization == null)
                return HttpNotFound();
            if (ModelState.IsValid)
            {
                using (Model1 db = new Model1())
                {
                    db.Specializations.Add(specialization);
                    db.SaveChanges();
                    var specializations = db.Specializations.ToList();
                    ViewBag.Specializations = new SelectList(specializations, "Id", "Title");
                    return new JsonResult  {
                        Data = specializations,
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                }
            }
            return RedirectToAction("Index", db.Coaches.Include(nameof(User)).ToList());
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
                db.Coaches.Add(coaches);
                db.SaveChanges();
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
            ViewBag.SpecList = new SelectList(db.Specializations.ToList(), "Id", "Title");
            Coaches coaches = db.Coaches.Include(nameof(User)).ToList().Find(c => c.Id == id);
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
                db.Entry(coaches).State = EntityState.Modified; //здесь coaches.UserId == coaches.User.Id
                db.SaveChanges();
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
            Coaches coaches = db.Coaches.Include(nameof(User)).ToList().Find(c => c.Id == id);
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
            Coaches coaches = db.Coaches.Include(nameof(User)).ToList().Find(c=>c.Id==id);
            db.Coaches.Remove(coaches);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
