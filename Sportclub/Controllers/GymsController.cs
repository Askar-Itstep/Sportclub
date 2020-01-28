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
    public class GymsController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

       
        public ActionResult Index()
        {
            return View(unitOfWork.Gyms.GetAll().ToList());
        }

        // GET: Gyms/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Gyms gyms = unitOfWork.Gyms.GetById(id);
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
                unitOfWork.Gyms.Create(gyms);
                unitOfWork.Gyms.Save();
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
            Gyms gyms = unitOfWork.Gyms.GetById(id);
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
                unitOfWork.Gyms.Update(gyms);
                unitOfWork.Gyms.Save();
                return RedirectToAction("Index");
            }
            return View(gyms);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Gyms gyms = unitOfWork.Gyms.GetById(id);
            if (gyms == null)
            {
                return HttpNotFound();
            }
            return View(gyms);
        }

      
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Gyms gyms = unitOfWork.Gyms.GetById(id);
            unitOfWork.Gyms.Delete(gyms.Id);
            unitOfWork.Gyms.Save();
            return RedirectToAction("Index");
        }

       
    }
}
