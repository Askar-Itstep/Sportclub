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
    public class GymsController : Controller
    {
        private Model1 db = new Model1();

        // GET: Gyms
        public ActionResult Index()
        {
            return View(db.Gyms.ToList());
        }

        // GET: Gyms/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Gyms gyms = db.Gyms.Find(id);
            if (gyms == null)
            {
                return HttpNotFound();
            }
            return View(gyms);
        }

        // GET: Gyms/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,GymName")] Gyms gyms)
        {
            if (ModelState.IsValid)
            {
                db.Gyms.Add(gyms);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(gyms);
        }

        // GET: Gyms/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Gyms gyms = db.Gyms.Find(id);
            if (gyms == null)
            {
                return HttpNotFound();
            }
            return View(gyms);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,GymName")] Gyms gyms)
        {
            if (ModelState.IsValid)
            {
                db.Entry(gyms).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(gyms);
        }

        // GET: Gyms/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Gyms gyms = db.Gyms.Find(id);
            if (gyms == null)
            {
                return HttpNotFound();
            }
            return View(gyms);
        }

        // POST: Gyms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Gyms gyms = db.Gyms.Find(id);
            db.Gyms.Remove(gyms);
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
