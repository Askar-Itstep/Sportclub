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

namespace Sportclub.Controllers
{
    public class ClientsController : Controller
    {
        private Model1 db = new Model1();

    
        public ActionResult Index()
        {
            return View(db.Clients.Include(c=>c.User).ToList());
        }

       
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Clients clients = db.Clients.Include(c=>c.User).ToList().Find(c=>c.Id==id);
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
                    db.Clients.Add(clients);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            clients.User.Role = new Role { RoleName = "client" };//.. (т.к. нов. клиент ~ нов. юзер)
            db.Users.Add(clients.User);
            db.Clients.Add(clients);
            db.SaveChanges();
            //return View(clients);
            return RedirectToAction("Index");
        }

       
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Clients clients = db.Clients.Include(c=>c.User).ToList().Find(c=>c.Id==id);
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
                db.Entry(clients).State = EntityState.Modified;
                db.SaveChanges();
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
            Clients clients = db.Clients.Include(c=>c.User).ToList().Find(c=>c.Id==id);
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
            Clients clients = db.Clients.Include(c => c.User).ToList().Find(c => c.Id == id);
            db.Clients.Remove(clients);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
