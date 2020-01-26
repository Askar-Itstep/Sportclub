using Microsoft.Ajax.Utilities;
using Sportclub.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Newtonsoft.Json;
using Sportclub.Repository;

namespace Sportclub.Controllers
{
    public class GraphTraningController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        public ActionResult Index()
        {
            var GraphTraning = unitOfWork.GraphTraning.Include(g => g.Coache.User).Include(g => g.Clients);
            return View(GraphTraning.ToList());
        }

        //------------------------------------------Create ------------------------------------------------------      
        public ActionResult GetTime(int? intDay)    //отправ. в Create-page самое ранее возм. время
        {
            if (intDay == null) 
                return HttpNotFound();
            
            var graphics = unitOfWork.GraphTraning.GetAll().Where(g=>(int)g.DayOfWeek == intDay);
            //занятое время
            DateTime today = DateTime.Today;
            var times = graphics.Select(g => new { TimeBegin = g.TimeBegin, TimeEnd = g.TimeEnd }).ToList();
            times.Add(new { TimeBegin = new DateTime(today.Year, today.Month, today.Day, 21, 0, 0)
                , TimeEnd= new DateTime(today.Year, today.Month, today.Day, 8, 30, 0) });
            //times.ForEach(t => System.Diagnostics.Debug.WriteLine("timeBegin: " + t.TimeBegin.GetType() + "; "+ "timeEnd: "+t.TimeEnd.GetType()));

            List<Tuple<DateTime, DateTime>> freeTimes = new List<Tuple<DateTime, DateTime>>();
            for (int i = 0; i < times.Count()-1; i++) {
                if (times[i + 1].TimeBegin.Hour - times[i].TimeEnd.Hour >= 2) {
                    freeTimes.Add(new Tuple<DateTime, DateTime>(
                        new DateTime (today.Year, today.Month, today.Day,  times[i].TimeEnd.Hour+1, 0, 0)
                        , new DateTime (today.Year, today.Month, today.Day, times[i].TimeEnd.Hour+2, 30, 0)));
                }
            }
            
            var json = JsonConvert.SerializeObject(freeTimes);
            return new JsonResult { Data = json, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        [HttpGet]
        public ActionResult Create()    //надо добавить авто-коррекцию врмени в полях ввода в соотв. с граф.
        {
            var coachesId = new SelectList(unitOfWork.Coaches.Include(nameof(User)), "Id", "User.FullName");

            ViewBag.CoacheId = coachesId;   //-> DropDownList("CoacheId"..)
            ViewBag.SpecializationId = new SelectList(unitOfWork.Coaches.Include(nameof(Specialization)), "SpecializationId", "Specialization.Title");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(GraphTraning graphTraning)
        {
            if (ModelState.IsValid) {
                unitOfWork.GraphTraning.Create(graphTraning);
                unitOfWork.GraphTraning.Save();
                return new JsonResult { Data = "Данные добавлены!", JsonRequestBehavior = JsonRequestBehavior.DenyGet };
            }
            return new JsonResult { Data = "Модель не валидна!", JsonRequestBehavior = JsonRequestBehavior.DenyGet };
        }

        [HttpGet]   //вызов из CreatePage
        public ActionResult GetSpecialists(int id)//выбрать специальности  у выбранн. тренера по его UserId (сначала по ID-coache)
        {
            var coache = unitOfWork.Coaches.Include(nameof(Specialization)).FirstOrDefault(c => c.Id == id);    //тренер-личность
            var resCoaches = unitOfWork.Coaches.Include(nameof(Specialization)).Where(c => c.UserId == coache.UserId).ToList(); //его тренер-должности 
            return PartialView("Partial/_GetSpecializations", resCoaches);
        }
        //вызов из CreatePage
        public ActionResult GetCoaches(int? id) //выбрать тренеров по ID Тренера->Id специализ.
        {
            if (id == null) {
                return HttpNotFound();
            }

            var specialization = unitOfWork.Specializations.GetAll().FirstOrDefault(s => s.Id == id);
            List<Coaches> coaches = new List<Coaches>();
            var coachesQuery = unitOfWork.Coaches.Include(nameof(User))
                .Where(c => c.SpecializationId == specialization.Id).ToList();
            coaches.AddRange(coachesQuery);


            return PartialView("Partial/_GetCoaches", coaches);
        }

        //---------------------------------Edit--------------------------------------
        [HttpGet]   //вызов из EditPage (установ. label в соотв. с select)
        public ActionResult GetSpecializ(int id)//выбрать  специальностЬ  по ID-coache (одно имя - разн. спец.)
        {
            var coache = unitOfWork.Coaches.Include(nameof(Specialization)).FirstOrDefault(c => c.Id == id);    
            
            return new JsonResult { Data = coache.Specialization.Title, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public ActionResult Edit(int? id)   //ID-traning
        {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GraphTraning graphTraning = unitOfWork.GraphTraning.GetById(id);
            if (graphTraning == null) {
                return HttpNotFound();
            }            
            var graphicWithCoacheWithName = unitOfWork.GraphTraning.Include(g => g.Coache)
                                                 .Select(g => new { Id = g.Id, CoacheId = (int)g.CoacheId, UserCoache = g.Coache.User});
            ViewBag.Coaches = new SelectList(graphicWithCoacheWithName, "CoacheId", "UserCoache.FullName", graphTraning.CoacheId);
            ViewBag.TimeBegin = graphTraning.TimeBegin.GetDateTimeFormats('t')[0];
            ViewBag.TimeEnd = graphTraning.TimeEnd.GetDateTimeFormats('t')[0];
            return View(graphTraning);
        }
        
        //[HttpGet] //клиентов не надо передавать (автопоставка по навигац.)
        //public ActionResult PreEdit(int? id)
        //{
        //    if (id == null) {
        //        return HttpNotFound("Такoй график не найден!");
        //    }

        //    var clients = unitOfWork.Clients.Include(nameof(User)).Where(c => c.GraphicId == id).Select(c => c.Id).ToList();
        //    //System.Diagnostics.Debug.WriteLine("clients[0]: " + clients[0].User.FullName);
        //    return new JsonResult { Data = clients, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        //}
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(GraphTraning graphTraning) 
        {
            var form = Request.Form;
            if (ModelState.IsValid) {
                unitOfWork.GraphTraning.Update(graphTraning);
                unitOfWork.GraphTraning.Save();
                return RedirectToAction("Index");
            }
            ViewBag.CoacheId = new SelectList(unitOfWork.Coaches.Include(nameof(User)), "UserId", "User.FullName", graphTraning.CoacheId); 

            return View(graphTraning);
        }

        //--------------Записаться!-------------------
        public ActionResult SignUp(int? id) //ID тренировки!
        {
            if (id == null) {
                return HttpNotFound();
            }
            
                var graphic = unitOfWork.GraphTraning.GetAll().Where(g => g.Id == id).Include(g => g.Clients).FirstOrDefault();
                //----------исп-ся User(~Principal) авторизованн. юзера----------
                var client = unitOfWork.Clients.Include(c => c.User).Where(c => c.User.Login.Equals(User.Identity.Name)).FirstOrDefault();
                graphic.Clients.Add(client);
                unitOfWork.Clients.Save();
            
            return RedirectToAction("Index");
        }

        //-------------------------------Delete --------------------------------------
        public ActionResult Delete(int? id)
        {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GraphTraning graphTraning = unitOfWork.GraphTraning.GetById(id);
            if (graphTraning == null) {
                return HttpNotFound();
            }
            return View(graphTraning);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            GraphTraning graphTraning = unitOfWork.GraphTraning.GetById(id);
            unitOfWork.GraphTraning.Delete(graphTraning.Id);
            unitOfWork.GraphTraning.Save();
            return RedirectToAction("Index");
        }

        //-------------------------------------Other ------------------------------
        public ActionResult Details(int? id)
        {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GraphTraning graphTraning = unitOfWork.GraphTraning.Include(g => g.Coache.User).ToList().Find(g => g.Id == id);
            if (graphTraning == null) {
                return HttpNotFound();
            }
            return View(graphTraning);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing) {
                unitOfWork.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
