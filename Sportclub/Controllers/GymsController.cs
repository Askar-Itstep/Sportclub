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
    public class GymsController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();
        IMapper mapper;
       public GymsController(IMapper mapper)
        {
            this.mapper = mapper;
        }
        public ActionResult Index()
        {
            var gymsBO = DependencyResolver.Current.GetService<GymsBO>().LoadAll(); //unitOfWork.Gyms.GetAll().ToList();
           
            var gymsVM = gymsBO.Select(g=>mapper.Map<GymsVM>(g));
            return View(gymsVM);
        }

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
