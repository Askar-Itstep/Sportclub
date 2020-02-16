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
using Sportclub.ViewModels;

namespace DataLayer.Controllers
{
    [Authorize(Roles = "admin, top_manager, manager, top_coache, head_coache, coache")]
    public class ClientsController : Controller
    {
        IMapper mapper;
        public ClientsController() { } 
        public ClientsController(IMapper mapper)
        {
            this.mapper = mapper;
        }
        public ActionResult Index()
        {
            var clientsBO = DependencyResolver.Current.GetService<ClientsBO>().LoadAllWithInclude(nameof(User));
            List<ClientsVM> clientsVM = mapper.Map<List<ClientsVM>>(clientsBO);
            return View(clientsVM);
        }


        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var clientBO = DependencyResolver.Current.GetService<ClientsBO>().LoadAllWithInclude(nameof(User));
            var clientVM = mapper.Map<ClientsVM>(clientBO.FirstOrDefault(c => c.Id == id));

            if (clientVM == null)
                return HttpNotFound();

            return View(clientVM);
        }

        [Authorize(Roles = "admin, top_manager, manager")]
        public ActionResult Create() //админ может добав. нов. клиента.
        {
            return View();
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ClientsVM clientVM, HttpPostedFileBase upload)
        {
            ImageVM imageVM = DependencyResolver.Current.GetService<ImageVM>();
            ImageBO imageBase = DependencyResolver.Current.GetService<ImageBO>();

            if (ModelState.IsValid) {
                var userBO = mapper.Map<UserBO>(clientVM.User);
                var roleBO = DependencyResolver.Current.GetService<RoleBO>().LoadAll().FirstOrDefault(r => r.RoleName == "client");
                userBO.RoleId = roleBO.Id;
                userBO.Login = clientVM.User.Email.Split('@')[0];
                userBO.Gender = GenderBO.MEN;   //default

                if (upload != null) {           //with img
                    imageBase = SetImage(upload, imageVM, imageBase);
                    userBO.ImageId = imageBase.Id;
                }
                else {
                    userBO.Image = new ImageBO { Filename = "", ImageData = new byte[1] { 0 } };
                }
                var clientBO = mapper.Map<ClientsBO>(clientVM);
                clientBO.User = userBO;         //user create too!
                clientBO.UserId = userBO.Id;
                clientBO.Save(clientBO);
                return new JsonResult { Data = "Данные записаны", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            return View(clientVM);
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var clientBO = DependencyResolver.Current.GetService<ClientsBO>().LoadAllWithInclude(nameof(User)).FirstOrDefault(c => c.Id == id);

            if (clientBO == null) {
                return HttpNotFound();
            }
            var clientVM = mapper.Map<ClientsVM>(clientBO);
            return View(clientVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ClientsVM clientVM, HttpPostedFileBase upload)
        {
            ImageVM imageVM = DependencyResolver.Current.GetService<ImageVM>();
            ImageBO imageBase = DependencyResolver.Current.GetService<ImageBO>();
            if (ModelState.IsValid) {
                var clientBO = mapper.Map<ClientsBO>(clientVM);
                var roleBO = DependencyResolver.Current.GetService<RoleBO>().Load(clientVM.User.RoleId);
                clientBO.User.Role = roleBO;   //роль не меняется! 
                UserBO userBO = clientBO.User;
                if(upload != null) {
                    imageBase = SetImage(upload, imageVM, imageBase);
                    userBO.ImageId = imageBase.Id;
                }
                userBO.Save(userBO);
                clientBO.Save(clientBO);
                //return RedirectToAction("Index");
                return new JsonResult { Data = "Данные записаны", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            return View(clientVM);
        }
        //-------------------------------------------------------------------------------
        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (id == null) 
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var clientBO = DependencyResolver.Current.GetService<ClientsBO>().LoadAllWithInclude(nameof(User)).FirstOrDefault(c => c.Id == id);

            if (clientBO == null) 
                return HttpNotFound();

            var clientVM = mapper.Map<ClientsVM>(clientBO);
            return View(clientVM);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var clientBO = DependencyResolver.Current.GetService<ClientsBO>().LoadAllWithInclude(nameof(User)).FirstOrDefault(c => c.Id == id);
            var userBO = clientBO.User;
            //-------поискать среди тренеров--------------
            var coacheBO = DependencyResolver.Current.GetService<CoachesBO>().LoadAllWithInclude(nameof(User)).Where(c => c.UserId == userBO.Id).FirstOrDefault();
            if (coacheBO != null)
                coacheBO.DeleteSave(coacheBO);
            //----..среди клиентов ---------------
            clientBO.DeleteSave(clientBO);
            userBO.DeleteSave(userBO);
            return RedirectToAction("Index");
        }


    }
}
