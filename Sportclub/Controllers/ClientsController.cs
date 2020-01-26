using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Sportclub;
using Sportclub.Entities;
using Sportclub.Repository;

namespace Sportclub.Controllers
{
    public class ClientsController : Controller
    {
        //private Model1 db = new Model1();
        private UnitOfWork unityOfWork;
        public ClientsController()
        {
            unityOfWork = new UnitOfWork();
        }
    
        public ActionResult Index()
        {
            return View(unityOfWork.Clients.GetAll().ToList());
        }

       
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Clients client = unityOfWork.Clients.GetById(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
        }

       [Authorize(Roles ="admin, top_manager, manager")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Clients client)
        {
            if (client.UserId != 0) {              //Добав. возм. админу добавл. нов. клиента..
                if (ModelState.IsValid) {
                    unityOfWork.Clients.Create(client);
                    unityOfWork.Clients.Save();
                    return RedirectToAction("Index");
                }
            }
            client.User.Role = new Role { RoleName = "client" };//.. (т.к. нов. клиент ~ нов. юзер)
            unityOfWork.Users.Create(client.User);
            unityOfWork.Clients.Create(client);
            unityOfWork.Clients.Save();
            return RedirectToAction("Index");
        }

       
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Clients client = unityOfWork.Clients.GetById(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Clients client)
        {
            if (ModelState.IsValid)
            {
                unityOfWork.Clients.Update(client);
                unityOfWork.Users.Update(client.User);
                unityOfWork.Users.Save();
                unityOfWork.Clients.Save();
                return RedirectToAction("Index");
            }
            return View(client);
        }

        
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Clients clients = unityOfWork.Clients.GetById(id);
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
            Clients clients = unityOfWork.Clients.GetById(id);
            unityOfWork.Clients.Delete(clients.Id);
            unityOfWork.Clients.Save();
            return RedirectToAction("Index");
        }

    }
}
