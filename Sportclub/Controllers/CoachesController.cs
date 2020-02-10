using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using BusinessLayer.BusinessObject;
using DataLayer;
using DataLayer.Entities;
using DataLayer.Repository;
using Sportclub.ViewModel;

namespace DataLayer.Controllers
{
    [Authorize(Roles ="admin, top_manager, manager, top_coache, head_coache")]
    public class CoachesController : Controller
    {
        
        IMapper mapper;
        public CoachesController() { }
        public CoachesController(IMapper mapper)
        {
            this.mapper = mapper;
        }

        public ActionResult Index()
        {
            var coachesBO = DependencyResolver.Current.GetService<CoachesBO>().LoadAllWithInclude(nameof(User), nameof(Specialization));
            var coachesVM = coachesBO.Select(co => mapper.Map<CoachesVM>(co));
            return View(coachesVM.ToList());
        }


        public ActionResult Details(int? id)
        {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var coacheBO = DependencyResolver.Current.GetService<CoachesBO>()
                                    .LoadAllWithInclude(nameof(User), nameof(Specialization)).FirstOrDefault(c => c.Id == id);

            if (coacheBO == null)
                return HttpNotFound();

            var coacheVM = mapper.Map<CoachesVM>(coacheBO);
            return View(coacheVM);
        }
        //----------------------------------------------------------------------------------------------------
        [HttpGet]
        public ActionResult Create()
        {
            var specializationsBO = DependencyResolver.Current.GetService<SpecializationBO>().LoadAll().ToList();
            specializationsBO.Add(new SpecializationBO { Title = "-- Добавить специализацию --" });
            var specializationsVM = specializationsBO.Select(s => mapper.Map<SpecializationVM>(s));
            ViewBag.Specializations = new SelectList(specializationsVM, "Id", "Title");
                                                                                 //произвести юзера  - в тренеры
            var usersBO = DependencyResolver.Current.GetService<UserBO>().LoadAll().Where(u => u.Token == null).ToList();
            var usersVM = usersBO.Select(u => mapper.Map<UserBO>(u)).ToList();
            ViewBag.UserList = new SelectList(usersVM, "Id", "FullName");
            return View();

        }
        [HttpPost]          //добавить специальн. тренеру
        public ActionResult CreateSpec(SpecializationVM specializationVM)   //обработ. ajax
        {
            if (specializationVM == null)
                return HttpNotFound();
            if (ModelState.IsValid) {
                var specBO = mapper.Map<SpecializationBO>(specializationVM);
                specBO.Save(specBO);
                var specVM = specBO.LoadAll().Select(s => mapper.Map<SpecializationVM>(s));
                ViewBag.Specializations = new SelectList(specVM, "Id", "Title");
                return new JsonResult
                {
                    Data = specVM,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            var coachesBO = DependencyResolver.Current.GetService<CoachesBO>()
                                    .LoadAllWithInclude(nameof(User), nameof(Specialization));
            var coachesVM = coachesBO.Select(co => mapper.Map<CoachesVM>(co));
            return RedirectToAction("Index", coachesVM.ToList());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CoachesVM coacheVM)    //нов. тренера только из существ. юзера!
        {                                               
            if (coacheVM == null)
                return HttpNotFound();
            if (coacheVM.UserId == 0) {
                return new HttpNotFoundResult("User is null!");
            }
            if (ModelState.IsValid) {
                var coacheBO = mapper.Map<CoachesBO>(coacheVM);
                coacheBO.User.Token = "coache1";
                coacheBO.Save(coacheBO);
                return RedirectToAction("Index");
            }
            return View(coacheVM);
        }
        //-------------------------------------------------------------------------------------------------
        public ActionResult Edit(int? id)
        {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var coacheBO = DependencyResolver.Current.GetService<CoachesBO>().LoadAllWithInclude(nameof(User)).FirstOrDefault(c => c.Id == id); 
            if (coacheBO == null) {
                return HttpNotFound();
            }
            var coacheVM = mapper.Map<CoachesVM>(coacheBO);
            ViewBag.SpecList = new SelectList(DependencyResolver.Current.GetService<SpecializationBO>().LoadAll(), "Id", "Title");
            return View(coacheVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CoachesVM coacheVM)  //в форму добавл. скрыт. поля model.UserId + model.User.Id
        {
            if (ModelState.IsValid) {
                var coacheBO = mapper.Map<CoachesBO>(coacheVM);
                var roleBO = DependencyResolver.Current.GetService<RoleBO>().LoadAll().Where(r => r.RoleName.Contains("coache")).FirstOrDefault();
                coacheBO.User.RoleId = roleBO.Id;
                coacheBO.User.Role = roleBO;
                coacheBO.User.Token = DependencyResolver.Current.GetService<UserBO>().LoadAll().Where(u => u.Id == coacheBO.UserId).FirstOrDefault().Token;
                var userBO = coacheBO.User;
                userBO.Save(userBO);
                coacheBO.Save(coacheBO);
                return RedirectToAction("Index");
            }
            return View(coacheVM);
        }
        //-----------------------------------------------------------------------------------------
        public ActionResult Delete(int? id)
        {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var coacheBO = DependencyResolver.Current.GetService<CoachesBO>().LoadAllWithInclude(nameof(User)).FirstOrDefault(c => c.Id == id);
            if (coacheBO == null) {
                return HttpNotFound();
            }
            var coacheVM = mapper.Map<CoachesVM>(coacheBO);
            return View(coacheVM);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var coacheBO = DependencyResolver.Current.GetService<CoachesBO>().LoadAllWithInclude(nameof(User)).FirstOrDefault(c => c.Id == id);
            coacheBO.DeleteSave(coacheBO);
            return RedirectToAction("Index");
        }


    }
}
