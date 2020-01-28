using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DataLayer;
using DataLayer.Entities;
using DataLayer.Repository;

namespace DataLayer.Controllers
{
    public class ClientsController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();
    
        public ActionResult Index()
        {
            var clients = unitOfWork.Clients.GetAll();
            return View(clients.ToList());
        }

       
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Clients clients = unitOfWork.Clients.Include("User").ToList().Find(c=>c.Id==id);
            if (clients == null)
            {
                return HttpNotFound();
            }
            return View(clients);
        }

       [Authorize(Roles ="admin, top_manager, manager")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Clients clients)
        {
            if (clients.UserId != 0) {              //Добав. возм. админу добавл. нов. клиента..
                if (ModelState.IsValid) {
                    unitOfWork.Clients.Create(clients);
                    unitOfWork.Clients.Save();
                    return RedirectToAction("Index");
                }
            }
            clients.User.Role = new Role { RoleName = "client" };//.. (т.к. нов. клиент ~ нов. юзер)
            unitOfWork.Users.Create(clients.User);
            unitOfWork.Clients.Create(clients);
            unitOfWork.Clients.Save();
            //return View(clients);
            return RedirectToAction("Index");
        }

       
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Clients clients = unitOfWork.Clients.Include("User").ToList().Find(c=>c.Id==id);
            if (clients == null)
            {
                return HttpNotFound();
            }
            return View(clients);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(
            //[Bind(Include = "Id,FullName,BirthDay,Phone,Email,Login,Password")]
        Clients clients)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.Clients.Update(clients);
                unitOfWork.Clients.Save();
                return RedirectToAction("Index");
            }
            return View(clients);
        }

        
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Clients clients = unitOfWork.Clients.Include("User").ToList().Find(c=>c.Id==id);
            if (clients == null)
            {
                return HttpNotFound();
            }
            return View(clients);
        }

       
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Clients clients = unitOfWork.Clients.Include("User").ToList().Find(c => c.Id == id);
            unitOfWork.Clients.Delete(clients.Id);
            unitOfWork.Clients.Save();
            return RedirectToAction("Index");
        }

        
    }
}
