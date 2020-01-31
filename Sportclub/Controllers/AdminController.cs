using AutoMapper;
using BusinessLayer.BusinessObject;
using DataLayer.Entities;
using DataLayer.Repository;
using Sportclub.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DataLayer.Controllers
{
    public class AdminController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();
        IMapper mapper;
        public AdminController()
        { }
        public AdminController(IMapper mapper)
        {
            this.mapper = mapper;
        }

        [Authorize(Roles = "admin, top_manager, manager")] //из CustomRoleProvider!
        public ActionResult Index()
        {
            //var managers = unitOfWork.Administration.Include(nameof(User));
            var managers = DependencyResolver.Current.GetService<AdministrationBO>().LoadAllWithInclude(nameof(User));
            //managers.ToList().ForEach(m => System.Diagnostics.Debug.WriteLine(m.UserBO.FullName));

            var managersVM = managers.Select(m => mapper.Map<AdministrationVM>(m)).ToList();
            return View(managersVM);
        }
        
        public ActionResult Details(int? id)
        {
            if (id == null)
                return HttpNotFound();

            var manager = unitOfWork.Administration.Include(nameof(User)).Where(m => m.Id == id);
            return View(manager);
        }
        
        public ActionResult Create()
        {
            return View();
        }

       
        [HttpPost]
        public ActionResult Create(Administration manager)
        {
            try
            {
                unitOfWork.Administration.Create(manager);
                unitOfWork.Administration.Save();
                return RedirectToAction("Index");                           
            }
            catch
            {
                return View(manager);
            }
        }

      
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return HttpNotFound();
            
            var manager = unitOfWork.Administration.Include(nameof(User)).Where(m => m.Id == id);
            return View(manager);
        }
        
        [HttpPost]
        public ActionResult Edit(Administration manager) //int id, FormCollection collection
        {
            try
            {
                unitOfWork.Administration.Update(manager);
                unitOfWork.Administration.Save();
                return RedirectToAction("Index");
            }                
            
            catch
            {
                return View(manager);
            }
        }
        
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return HttpNotFound();

            var manager = unitOfWork.Administration.Include(nameof(User)).Where(m => m.Id == id);
            return View(manager);

        }
        
        [HttpPost]
        public ActionResult Delete(Administration manager)//int id, FormCollection collection
        {
            try
            {
                unitOfWork.Administration.Delete(manager.Id);
                unitOfWork.Administration.Save();
                return RedirectToAction("Index");                
            }
            catch
            {
                return View(manager);
            }
        }
    }
}
