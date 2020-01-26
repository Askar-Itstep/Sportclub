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
        //private Model1 unitOfWork = new Model1();
        private UnitOfWork unitOfWork = new UnitOfWork();


        [Authorize(Roles ="admin, top_manager, manager, top_coache, head_coache, coache")]
        public ActionResult Index()
        {
            var coaches = unitOfWork.Coaches.Include(nameof(User)).ToList();
            return View(coaches);
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

        [HttpGet]
        [Authorize(Roles = "admin, top_manager, manager")]
        public ActionResult Create()    //в CreatePage - можно добавить специальность
        {
            using(Model1 unitOfWork = new Model1())
            {
                var specializations = unitOfWork.Specializations.ToList();
                specializations.Add(new Specialization { Title = "-- Добавить специализацию --" });
                ViewBag.Specializations = new SelectList(specializations, "Id", "Title");

                                                //1)в форму попадает только имя, 2)выбор тренера случаен                
                ViewBag.UserList = new SelectList(unitOfWork.Users.Where(u => u.Token.Contains("coache")).ToList(), "Id", "FullName");
                return View();
            }            
        }
        //[HttpPost]
        //public ActionResult CreateSpec()
        //{
        //    if (ModelState.IsValid)
        //    {
        //        using (Model1 unitOfWork = new Model1())
        //        {
        //            Specialization specialization = new Specialization { Title = form["Title"] };
        //            unitOfWork.Specializations.Add(specialization);
        //            unitOfWork.SaveChanges();
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
                using (Model1 unitOfWork = new Model1())
                {
                    unitOfWork.Specializations.Add(specialization);
                    unitOfWork.SaveChanges();
                    var specializations = unitOfWork.Specializations.ToList();
                    ViewBag.Specializations = new SelectList(specializations, "Id", "Title");
                    return new JsonResult  {
                        Data = specializations,
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                }
            }
            return RedirectToAction("Index", unitOfWork.Coaches.Include(nameof(User)).ToList());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind (Include ="Id, SpecializationId, Status, TimeWork=0, UserId")]
            Coaches coaches)
        {
            if (coaches == null)
                return HttpNotFound();
            if(coaches.UserId == 0) {
                return new HttpNotFoundResult ("User is null!");
            }
            {//не проходит проверку?
                //System.Diagnostics.Debug.WriteLine(ModelState.IsValidField("Id"));
                //System.Diagnostics.Debug.WriteLine(ModelState.IsValidField("SpecializationId"));
                //System.Diagnostics.Debug.WriteLine(ModelState.IsValidField("Status"));
                //System.Diagnostics.Debug.WriteLine(ModelState.IsValidField("TimeWork"));
                //System.Diagnostics.Debug.WriteLine(ModelState.IsValidField("UserId"));
            }
            if (ModelState.IsValid) 
            {
                unitOfWork.Coaches.Create(coaches);
                unitOfWork.Coaches.Save();
                return RedirectToAction("Index");
            }

            return RedirectToAction( "Index",
                unitOfWork.Coaches.Include(nameof(User)).Include(nameof(Specialization)).ToList());
        }
//------------------------------------ Edit ----------------------------------------------------------------
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.SpecList = new SelectList(unitOfWork.Specializations.GetAll().ToList(), "Id", "Title");
            Coaches coaches = unitOfWork.Coaches.Include(nameof(User)).ToList().Find(c => c.Id == id);
            if (coaches == null)
            {
                return HttpNotFound();
            }
            return View(coaches);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Coaches coaches)  //в форму добавл. скрыт. поля UserId, User.Id, User.Token
        {
            if (ModelState.IsValid)
            {
                unitOfWork.Coaches.Update(coaches); 
                unitOfWork.Users.Update(coaches.User);
                unitOfWork.Users.Save();
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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                unitOfWork.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
