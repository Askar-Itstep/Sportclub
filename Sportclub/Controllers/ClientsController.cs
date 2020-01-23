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

       
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FullName,BirthDay,Phone,Email,GraphicId,Password")] Clients clients)
        {
            if (ModelState.IsValid)
            {
                db.Clients.Add(clients);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(clients);
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
