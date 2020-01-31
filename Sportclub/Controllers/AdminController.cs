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
            //var managers = unitOfWork.Administration.Include(nameof(User)).ToList();
            var managers = DependencyResolver.Current.GetService<AdministrationBO>().LoadAllWithInclude(nameof(User));
            //managers.ToList().ForEach(m => System.Diagnostics.Debug.WriteLine(m.UserBO.FullName));

            var managersVM = managers.Select(m => mapper.Map<AdministrationVM>(m)).ToList();
            return View(managersVM);
        }
        
        public ActionResult Details(int? id)
        {
            if (id == null)
                return HttpNotFound();

            //var manager = unitOfWork.Administration.Include(nameof(User)).Where(m => m.Id == id);
            var manager = DependencyResolver.Current.GetService<AdministrationBO>()
                .LoadAllWithInclude(nameof(User)).Where(m=>m.Id==id).FirstOrDefault();
            var managerVM = mapper.Map<AdministrationVM>(manager);
            return View(managerVM);
        }
        
        public ActionResult Create()
        {
            return View();
        }

       
        [HttpPost]
        public ActionResult Create(AdministrationVM manager)
        {
            manager.UserVM.Token = "manager";
            manager.UserVM.RoleVMId = 3;
            manager.UserVM.RoleVM = new RoleVM { Id = 3, RoleName = "manager" };
            //var users = DependencyResolver.Current.GetService<UserBO>().LoadAll().ToList();
            //var lastUserId = users[users.Count() - 1].Id;
            //manager.UserVMId = lastUserId + 1;
            //manager.UserVM.Id = lastUserId + 1;
                       
            foreach (var prop in manager.GetType().GetProperties()) {
                  System.Diagnostics.Debug.WriteLine("{0} - {1}", prop.Name, ModelState.IsValidField(prop.Name));
            }

            if(ModelState.IsValid)
            {
                //unitOfWork.Administration.Create(manager);
                //unitOfWork.Administration.Save();
                //var managerBO = DependencyResolver.Current.GetService<AdministrationBO>();
                var managerBO = mapper.Map<AdministrationBO>(manager);
                managerBO.Save(managerBO);
                return RedirectToAction("Index");                           
            }
            
                return View(manager);
            
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
