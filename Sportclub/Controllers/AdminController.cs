using AutoMapper;
using BusinessLayer.BusinessObject;
using DataLayer.Entities;
using DataLayer.Repository;
using Sportclub.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
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

        [Authorize(Roles = "admin, top_manager, manager")]          //из CustomRoleProvider!
        public ActionResult Index()
        {
            var managers = DependencyResolver.Current.GetService<AdministrationBO>().LoadAllWithInclude(nameof(User)).ToList();
            var users = DependencyResolver.Current.GetService<UserBO>().LoadAllWithInclude(nameof(Role)).ToList();
            var res = managers.Join(users, m => m.UserBOId, u => u.Id, (m, u) => new AdministrationBO {
                Id = m.Id, Status = m.Status, UserBOId = m.UserBOId, UserBO = u
            }).ToList();

            //var managersVM = managers.Select(m => mapper.Map<AdministrationVM>(m)).ToList();
            var managersVM = res.Select(m => mapper.Map<AdministrationVM>(m)).ToList();
            return View(managersVM);
        }
        //--------------------------------------------------------------------------------------------------------------
        [HttpGet]
        [Authorize(Roles ="admin, top_manager")]
        public ActionResult Create()
        {
            return View();
        }
       
        [HttpPost]
        public ActionResult Create(AdministrationVM manager)
        {
            //добор значений не вошедшю в форму
            manager.UserVM.Token = "manager";
            manager.UserVM.RoleVMId = 3;
            manager.UserVM.RoleVM = new RoleVM { Id = 3, RoleName = "manager" };
            int lenUsers = DependencyResolver.Current.GetService<UserBO>().LoadAll().Count();

            if(ModelState.IsValid)
            {
                var userBO = mapper.Map<UserBO>(manager.UserVM);
                userBO.Save(userBO);
                userBO = userBO.LoadAll().Where(u => u.Email == manager.UserVM.Email && u.Password == manager.UserVM.Password).FirstOrDefault();
              
                var managerBO = mapper.Map<AdministrationBO>(manager);
                managerBO.UserBO.Id = userBO.Id;
                managerBO.UserBOId = userBO.Id;
                managerBO.Save(managerBO);
                return RedirectToAction("Index");                           
            }
            return View(manager);            
        }
        //------------------------------------------------------------------------------------
      
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return HttpNotFound();
            
            var managerBO = DependencyResolver.Current.GetService<AdministrationBO>().LoadAllWithInclude(nameof(User)).Where(m=>m.Id == id).FirstOrDefault();   //здесь eще нет RoleBO (не явл. навиг. св.)
            var roleBO = DependencyResolver.Current.GetService<RoleBO>();
            roleBO = roleBO.LoadAll().Where(r => r.Id == managerBO.UserBO.RoleBOId).FirstOrDefault();

            managerBO.UserBO.RoleBO = roleBO;
            var managerVM = mapper.Map<AdministrationVM>(managerBO);
            return View(managerVM);
        }
        
        [HttpPost]
        public ActionResult Edit(AdministrationVM managerVM) 
        {
            if(ModelState.IsValid)
            {
                var userBO = mapper.Map<UserBO>(managerVM.UserVM);
                userBO.Save(userBO);
                userBO = userBO.LoadAll().Where(u => u.Email == managerVM.UserVM.Email && u.Password == managerVM.UserVM.Password).FirstOrDefault();

                var managerBO = mapper.Map<AdministrationBO>(managerVM);
                managerBO.UserBO = userBO;
                managerBO.UserBO.Id = userBO.Id;
                managerBO.UserBOId = userBO.Id;
                managerBO.Save(managerBO);
                return RedirectToAction("Index");
            }   
            return View(managerVM);
            
        }
        //---------------------------------------------------------------------------
        [HttpGet]
        [Authorize(Roles = "admin, top_manager")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return HttpNotFound();
            
            var managerBO = DependencyResolver.Current.GetService<AdministrationBO>();
            managerBO = managerBO.LoadAllWithInclude(nameof(User)).Where(m => m.Id == id).FirstOrDefault();
            var userBO = DependencyResolver.Current.GetService<UserBO>().LoadAllWithInclude(nameof(Role)).Where(u => u.Id == managerBO.UserBO.Id).FirstOrDefault();
            managerBO.UserBO = userBO;
            var managerVM = mapper.Map<AdministrationVM>(managerBO);
            return View(managerVM);

        }
        
        [HttpPost]
        public ActionResult Delete(AdministrationVM managerVM)
        {
            if(ModelState.IsValid)
            {
                var managerBO = mapper.Map<AdministrationBO>(managerVM);
                var userBO = managerBO.UserBO;
                userBO.DeleteSave(userBO);
                managerBO.DeleteSave(managerBO);

                return RedirectToAction("Index");                
            }
            
            return View(managerVM);
            
        }
        //------------------------------------------------------------------
        public ActionResult Details(int? id)
        {
            if (id == null)
                return HttpNotFound();

            var managerBO = DependencyResolver.Current.GetService<AdministrationBO>()
                                .LoadAllWithInclude(nameof(User)).Where(m => m.Id == id).FirstOrDefault();
            var managerVM = mapper.Map<AdministrationVM>(managerBO);
            return View(managerVM);
        }
    }
}
