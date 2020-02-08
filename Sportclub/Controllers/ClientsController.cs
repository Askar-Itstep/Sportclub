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
    //[Authorize(Roles ="admin, top_manager, manager")]
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
        public ActionResult Create() //Добав. возм. админу добавл. нов. клиента..
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ClientsVM clientVM)
        {
            if (ModelState.IsValid) {
                var userBO = mapper.Map<UserBO>(clientVM.User);
                var roleBO = DependencyResolver.Current.GetService<RoleBO>().LoadAll().FirstOrDefault(r => r.RoleName == "client");
                //userBO.Role = roleBO;     //добавляет еще одну роль "client???"
                userBO.RoleId = roleBO.Id;
                userBO.Login = clientVM.User.Email.Split('@')[0];
                userBO.Gender = GenderBO.MEN;   //default
                var clientBO = mapper.Map<ClientsBO>(clientVM);
                clientBO.User = userBO;
                clientBO.UserId = userBO.Id;
                clientBO.Save(clientBO);
                return RedirectToAction("Index");

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
        public ActionResult Edit(ClientsVM clientVM)
        {
            if (ModelState.IsValid) {
                var clientBO = mapper.Map<ClientsBO>(clientVM);
                var roleBO = DependencyResolver.Current.GetService<RoleBO>().Load(clientVM.User.RoleId);
                clientBO.User.Role = roleBO;   //роль не меняется! 
                UserBO userBO = clientBO.User;
                userBO.Save(userBO);
                clientBO.Save(clientBO);

                return RedirectToAction("Index");
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
            clientBO.DeleteSave(clientBO);
            userBO.DeleteSave(userBO);
            return RedirectToAction("Index");
        }


    }
}
