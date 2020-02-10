﻿using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Newtonsoft.Json;
using DataLayer.Repository;
using DataLayer.Entities;
using AutoMapper;
using BusinessLayer.BusinessObject;
using Sportclub.ViewModel;

namespace DataLayer.Controllers
{
    public class GraphTraningsController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();
        IMapper mapper;
        private int idEmptyCoache = 999;
        private int idEmptySpecializ = 888;
        public GraphTraningsController(IMapper mapper)
        {
            this.mapper = mapper;
        }
        public ActionResult Index()
        {
            var graphicsBO = DependencyResolver.Current.GetService<GraphTraningBO>().LoadAllWithInclude("Coache.User", nameof(Clients), nameof(Gyms));
            var graphicsVM = graphicsBO.Select(g => mapper.Map<GraphTraningVM>(g)); //пока нет списка клиентов - no many-to-many

            var clientsBO = DependencyResolver.Current.GetService<ClientsBO>().LoadAllWithInclude(nameof(User));
            foreach (var graphic in graphicsBO) {   // no good - need many-to-many
                foreach (var client in clientsBO) {
                    if (graphic.Id == client.GraphicId) {
                        graphic.Clients.Add(client);
                    }
                }
            }
            var clientsVM = mapper.Map<List<ClientsVM>>(clientsBO);
            //var gymsBO = DependencyResolver.Current.GetService<GymsBO>().LoadAll();   //больше не нужно
            //var gymsVM = gymsBO.Select(g => mapper.Map<GymsVM>(g));
            //ViewBag.GymList = gymsVM.ToList();
            return View(graphicsVM);
        }
        //=============================================================================================
        //------------------------------------------Create ------------------------------------------------------    
        [Authorize(Roles ="admin, top_coache, head_coache")]
        [HttpGet]
        public ActionResult Create()
        {
            var coachesBO = DependencyResolver.Current.GetService<CoachesBO>().LoadAllWithInclude(nameof(User), nameof(Specialization));
            var coachesVM = coachesBO.Select(c => mapper.Map<CoachesVM>(c)).ToList();
            coachesVM.Add(new CoachesVM
            {
                Id = idEmptyCoache,
                User = new UserVM { FullName = "<--Select Name-->" },
                SpecializationId = idEmptySpecializ,
                Specialization = new SpecializationVM { Id = idEmptySpecializ, Title = "<--Select Specialization-->" }
            });
            ViewBag.CoacheId = new SelectList(coachesVM, "Id", "User.FullName");
            ViewBag.SpecializationId = new SelectList(coachesVM, "SpecializationId", "Specialization.Title");

            var gymsBO = DependencyResolver.Current.GetService<GymsBO>().LoadAll();   
            var gymsVM = gymsBO.Select(g => mapper.Map<GymsVM>(g));
            ViewBag.GymList = new SelectList(gymsVM, "Id", "GymName");
            return View();
        }

        public ActionResult GetTime(int? intDay)    //ajax-отправ. самое ранее возм. время по залу
        {
            if (intDay == null)
                return HttpNotFound();

            var graphics = DependencyResolver.Current.GetService<GraphTraningBO>().LoadAllWithInclude("Coache.User", nameof(Clients));
            //------------занятое время-------------------
            DateTime today = DateTime.Today;
            var times = graphics.Select(g => new { TimeBegin = g.TimeBegin, TimeEnd = g.TimeEnd }).ToList();

            times.Add(new
            {
                TimeBegin = new DateTime(today.Year, today.Month, today.Day, 21, 0, 0),  //ночью не работает!
                TimeEnd = new DateTime(today.Year, today.Month, today.Day, 0, 0, 0)
            });
            times.Add(new
            {
                TimeBegin = new DateTime(today.Year, today.Month, today.Day, 0, 0, 0),
                TimeEnd = new DateTime(today.Year, today.Month, today.Day, 8, 30, 0)
            });
            times.Add(new
            {
                TimeBegin = new DateTime(today.Year, today.Month, today.Day, 21, 0, 0),
                TimeEnd = new DateTime(today.Year, today.Month, today.Day, 0, 0, 0)
            });
            List<Tuple<DateTime, DateTime>> freeTimes = new List<Tuple<DateTime, DateTime>>();
            for (int i = 0; i < times.Count() - 1; i++) {
                if (times[i + 1].TimeBegin.Hour - times[i].TimeEnd.Hour >= 2
                                                && times[i].TimeEnd.Hour + 1 >= 9) {
                    freeTimes.Add(new Tuple<DateTime, DateTime>(
                        new DateTime(today.Year, today.Month, today.Day, times[i].TimeEnd.Hour + 1, 0, 0)
                        , new DateTime(today.Year, today.Month, today.Day, times[i].TimeEnd.Hour + 2, 30, 0)));
                }
            }
            var json = JsonConvert.SerializeObject(freeTimes);
            return new JsonResult { Data = json, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(GraphTraningVM   graphTraningVM)
        {
            var res = Request.Form;
            if (ModelState.IsValid) {
                var graphicBO = mapper.Map<GraphTraningBO>(graphTraningVM);
                graphicBO.Save(graphicBO);
                return new JsonResult { Data = "Данные добавлены!", JsonRequestBehavior = JsonRequestBehavior.DenyGet };
            }
            return new JsonResult { Data = "Модель не валидна!", JsonRequestBehavior = JsonRequestBehavior.DenyGet };
        }
        //---------------ajax-вызов из CreatePage #1------------------------
        [HttpGet]   //вернуть все специальн. выбранн. тренера
        public ActionResult GetSpecialists(int id)// ID-coache
        {
            if (id == idEmptyCoache) { //если выбран <Select coache> - вернуть все специальности
                var coachesAllBO = DependencyResolver.Current.GetService<CoachesBO>().LoadAllWithInclude(nameof(User), nameof(Specialization));
                var coachesAllVM = coachesAllBO.Select(c => mapper.Map<CoachesVM>(c)).ToList();
                return PartialView("Partial/_GetSpecializations", coachesAllVM);
            }
            
            var coachesBO = DependencyResolver.Current.GetService<CoachesBO>().LoadAllWithInclude(nameof(User), nameof(Specialization));
            var coacheBO = coachesBO.FirstOrDefault(c => c.Id == id);    //тренер-личность
            var userCoachesBO = coachesBO.Where(c => c.UserId == coacheBO.UserId).ToList(); //его должности 
            var userCoachesVM = userCoachesBO.Select(u => mapper.Map<CoachesVM>(u));
            return PartialView("Partial/_GetSpecializations", userCoachesVM);
        }
        //------------#2 - вернуть тренеров с выбр. специальностью-<Select special.>-----------------
        [HttpGet]  
        public ActionResult GetCoaches(int? id) //Id специализ.
        {
            if (id == null) {
                return HttpNotFound();
            }
            var coachesBO = DependencyResolver.Current.GetService<CoachesBO>().LoadAllWithInclude(nameof(User), nameof(Specialization));
            var coachesVM = coachesBO.Select(c => mapper.Map<CoachesVM>(c)).ToList();
            
            if (id == idEmptySpecializ) {       
                return PartialView("Partial/_GetCoaches", coachesVM);
            }
           
            var specialization = DependencyResolver.Current.GetService<SpecializationBO>().Load((int)id);
            coachesBO = coachesBO.Where(c => c.SpecializationId == specialization.Id);
            coachesVM = coachesBO.Select(c => mapper.Map<CoachesVM>(c)).ToList(); ;
            return PartialView("Partial/_GetCoaches", coachesVM);
        }
        //---------------#3 - вернуть залы подходящ. по специальн.----------------------
        [HttpGet]  
        public ActionResult GetGyms(string  listIdSpec)    //Json-ID специальностей
        {
            string[] json = JsonConvert.DeserializeObject<string[]>(listIdSpec);
            List<int> arrSpecializId = new List<int>();
            json.ForEach(j => arrSpecializId.Add(Int32.Parse(j)));
            var specializAllBO = DependencyResolver.Current.GetService<SpecializationBO>().LoadAll().ToList();
            var specializAllVM = mapper.Map<List<SpecializationVM>>(specializAllBO);
            var specializeSelectVM = specializAllVM.Join(arrSpecializId, s => s.Id, i => i, (s, i) => new SpecializationVM
            {
                Id = s.Id,
                Title = s.Title //если ч/з BusinessObj - то свойств. mapper = null
            });
            //revers to specBO
            var specializListSelectBO = mapper.Map<List<SpecializationBO>>(specializeSelectVM);

            var gymsAllBO = DependencyResolver.Current.GetService<GymsBO>().LoadAll();
            List<GymsBO> gymsBO = new List<GymsBO>();
            //-----------залы соответствуют специальностям, кроме индивидуал. (vip) и кикбокс. (box)
            if (specializListSelectBO.Any(s => s.Title.Contains("individ")))
                gymsBO.Add(gymsAllBO.FirstOrDefault(g => g.GymName == "vip"));
            if (specializListSelectBO.Any(s => s.Title.Contains("kick")))
                gymsBO.Add(gymsAllBO.FirstOrDefault(g => g.GymName == "boxing"));
            foreach (var gym in gymsAllBO) {
                foreach (var specialization in specializListSelectBO) {
                    if (gym.GymName.ToUpper().Contains(specialization.Title.ToUpper()) || specialization.Title.ToUpper().Contains(gym.GymName.ToUpper())) {
                        gymsBO.Add(gym);
                    }
                }
            }
            var gymsVM = gymsBO.Select(c => mapper.Map<GymsVM>(c)).ToList();
            return PartialView("Partial/_GetGyms", gymsVM);
        }
        //==========================================================================
        //---------------------------------Edit--------------------------------------
        [HttpGet]   //ajax-вызов из EditPage (установ. label в соотв. с выбранн. специаль.)
        public ActionResult GetSpecializ(int id)//выбрать  специальностЬ  по ID-coache (одно имя - разн. спец.)
        {
            var coacheBO = DependencyResolver.Current.GetService<CoachesBO>().LoadAllWithInclude(nameof(Specialization)).FirstOrDefault(c => c.Id == id);
            var coacheVM = mapper.Map<CoachesVM>(coacheBO);
            return new JsonResult { Data = coacheVM.Specialization.Title, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        //[Authorize(Roles = "top_coache, head_coache")]
        public ActionResult Edit(int? id)   //ID-traning
        {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var graphicBO = DependencyResolver.Current.GetService<GraphTraningBO>();
            graphicBO.Load((int)id);
            if (graphicBO == null) {
                return HttpNotFound();
            }
            var graphicVM = mapper.Map<GraphTraningVM>(graphicBO);
            var graphicWithCoache = DependencyResolver.Current.GetService<GraphTraningBO>().LoadAllWithInclude("Coache");
            var coachesWithName = DependencyResolver.Current.GetService<CoachesBO>().LoadAllWithInclude(nameof(User));
            var graphicWithCoacheWithName = graphicWithCoache.Join(coachesWithName, g => g.CoacheId, c => c.Id,
                (g, c) => new
                {
                    Id = g.Id,
                    CoacheId = c.Id,
                    UserCoache = c.User
                });
            ViewBag.Coaches = new SelectList(graphicWithCoacheWithName, "CoacheId", "UserCoache.FullName", graphicVM.CoacheId);
            ViewBag.TimeBegin = graphicVM.TimeBegin.GetDateTimeFormats('t')[0];
            ViewBag.TimeEnd = graphicVM.TimeEnd.GetDateTimeFormats('t')[0];
            return View(graphicVM);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(GraphTraningVM graphTraningVM)
        {
            if (ModelState.IsValid) {
                var graphicBO = mapper.Map<GraphTraningBO>(graphTraningVM);
                graphicBO.Save(graphicBO);
                return RedirectToAction("Index");
            }
            var coachesBO = DependencyResolver.Current.GetService<GraphTraningBO>().LoadAllWithInclude(nameof(User));
            var coachesVM = mapper.Map<List<CoachesBO>>(coachesBO);
            ViewBag.CoacheId = new SelectList(coachesVM, "UserId", "User.FullName", graphTraningVM.CoacheId);
            return View(graphTraningVM);
        }

        //--------------Записаться!-------------------
        public ActionResult SignUp(int? id) //ID тренировки!
        {
            if (id == null) {
                return HttpNotFound();
            }
            var graphicBO = DependencyResolver.Current.GetService<GraphTraningBO>().LoadAllWithInclude(nameof(Clients)).Where(g => g.Id == id).FirstOrDefault();
            var clientBO = DependencyResolver.Current.GetService<ClientsBO>().LoadAllWithInclude(nameof(User))
                                                                             .Where(c => c.User.Login.Equals(User.Identity.Name)).FirstOrDefault();
            graphicBO.Clients.Add(clientBO);
            clientBO.GraphicId = graphicBO.Id; //пока 1 юзер - 1 запись (no many-to-many)
            clientBO.Save(clientBO);
            return RedirectToAction("Index");
        }

        //-------------------------------Delete --------------------------------------
        public ActionResult Delete(int? id)
        {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GraphTraning graphTraning = unitOfWork.GraphTranings.GetById(id);
            if (graphTraning == null) {
                return HttpNotFound();
            }
            return View(graphTraning);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            GraphTraning graphTraning = unitOfWork.GraphTranings.GetById(id);
            unitOfWork.GraphTranings.Delete(graphTraning.Id);
            unitOfWork.GraphTranings.Save();
            return RedirectToAction("Index");
        }

        //-------------------------------------Other ------------------------------
        public ActionResult Details(int? id)
        {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GraphTraning graphTraning = unitOfWork.GraphTranings.Include("Coache.User").ToList().Find(g => g.Id == id);
            if (graphTraning == null) {
                return HttpNotFound();
            }
            return View(graphTraning);
        }

    }
}
