using AutoMapper;
using BusinessLayer.BusinessObject;
using DataLayer.Entities;
using DataLayer.Repository;
using Sportclub.ViewModel;
using Sportclub.ViewModels;
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
        IMapper mapper;
        public AdminController()
        { }
        public AdminController(IMapper mapper)
        {
            this.mapper = mapper;
        }

        [Authorize(Roles = "admin, top_manager, manager, ")]          //из CustomRoleProvider!
        public ActionResult Index()
        {
            var managers = DependencyResolver.Current.GetService<AdministrationBO>().LoadAllWithInclude(nameof(User)).ToList();
            var managersVM = managers.Select(m => mapper.Map<AdministrationVM>(m)).ToList();
            return View(managersVM);
        }
        //--------------------------------------------------------------------------------------------------------------
        [HttpGet]
        [Authorize(Roles ="admin, top_manager")]
        public ActionResult Create()
        {                   //дать топам право созд. помощн. из возд.
            return View();
        }
       
        [HttpPost]
        public ActionResult Create(AdministrationVM manager)
        {
            if(ModelState.IsValid)
            {
                var userBO = mapper.Map<UserBO>(manager.User);
                var roleBO = DependencyResolver.Current.GetService<RoleBO>().LoadAll();
                userBO.Image = DependencyResolver.Current.GetService<ImageBO>().Load(1);
                userBO.ImageId = 1;

                var managerBO = mapper.Map<AdministrationBO>(manager);
                managerBO.User = userBO;
                if (managerBO.Status.ToString().ToUpper().Contains("TOP")) {
                    managerBO.User.RoleId = roleBO.FirstOrDefault(r => r.RoleName.Equals("top_manager")).Id;
                    managerBO.User.Token = "top_manager";
                }
                if (managerBO.Status.ToString().ToUpper().Contains("ADMIN")) {
                    managerBO.User.RoleId = roleBO.FirstOrDefault(r => r.RoleName.Contains("admin")).Id;
                    managerBO.User.Token = "admin";
                }
                if (managerBO.Status.ToString().ToUpper().Equals("MANAGER")) {
                    managerBO.User.RoleId = roleBO.FirstOrDefault(r => r.RoleName.Equals("manager")).Id;
                    managerBO.User.Token = "manager";
                }
                managerBO.Save(managerBO);  //сначала создется юзер!
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
            roleBO = roleBO.LoadAll().Where(r => r.Id == managerBO.User.RoleId).FirstOrDefault();
            managerBO.User.Role = roleBO;

            var managerVM = mapper.Map<AdministrationVM>(managerBO);
            return View(managerVM);
        }
        
        [HttpPost]
        public ActionResult Edit(AdministrationVM managerVM, HttpPostedFileBase upload)
        {
            ImageVM imageVM = DependencyResolver.Current.GetService<ImageVM>();
            ImageBO imageBase = DependencyResolver.Current.GetService<ImageBO>();

            if (ModelState.IsValid)
            {
                var userBO = mapper.Map<UserBO>(managerVM.User);
                                
                if (upload != null) {           //with img
                    imageBase = SetImage(upload, imageVM, imageBase);
                    userBO.ImageId = imageBase.Id;
                }
                else {
                    userBO.Image = new ImageBO { Filename = "", ImageData = new byte[1] { 0 } };
                }
                userBO.Save(userBO);
                var managerBO = mapper.Map<AdministrationBO>(managerVM);
                managerBO.User = userBO;
                managerBO.User.Id = userBO.Id;
                managerBO.UserId = userBO.Id;
                managerBO.Save(managerBO);
                return new JsonResult { Data = "Данные записаны", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            return View(managerVM);
            
        }

        private ImageBO SetImage(HttpPostedFileBase upload, ImageVM imageVM, ImageBO imageBase)
        {
            string filename = System.IO.Path.GetFileName(upload.FileName);
            imageVM.Filename = filename;
            byte[] myBytes = new byte[upload.ContentLength];
            upload.InputStream.Read(myBytes, 0, upload.ContentLength);
            imageVM.ImageData = myBytes;
            var imgListBO = DependencyResolver.Current.GetService<ImageBO>().LoadAll().Where(i => i.Filename == imageVM.Filename).ToList();
            if (imgListBO == null || imgListBO.Count() == 0)  //если такого в БД нет - сохранить
            {
                var imageBO = mapper.Map<ImageBO>(imageVM);
                imageBase.Save(imageBO);
            }
            List<ImageBO> imageBases = DependencyResolver.Current.GetService<ImageBO>().LoadAll().Where(i => i.Filename == imageVM.Filename).ToList();
            imageBase = imageBases[0];
            return imageBase;
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
            var userBO = DependencyResolver.Current.GetService<UserBO>().LoadAllWithInclude(nameof(Role)).Where(u => u.Id == managerBO.User.Id).FirstOrDefault();
            managerBO.User = userBO;
            var managerVM = mapper.Map<AdministrationVM>(managerBO);
            return View(managerVM);

        }
        
        [HttpPost]
        public ActionResult Delete(AdministrationVM managerVM)
        {
            if(ModelState.IsValid)
            {
                var managerBO = mapper.Map<AdministrationBO>(managerVM);
                var userBO = managerBO.User;
                managerBO.DeleteSave(managerBO);
                userBO.DeleteSave(userBO);

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
