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
    [Authorize(Roles ="admin, top_manager, manager")]
    public class GymsController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();
        private IMapper mapper;
        public GymsController (IMapper mapper)
        {
            this.mapper = mapper;
        }
       
        public ActionResult Index()
        {
            var gymsBO = DependencyResolver.Current.GetService<GymsBO>().LoadAll().ToList();
            var gymsVM = gymsBO.Select(g => mapper.Map<GymsVM>(g));
            return View(gymsVM);
        }
        
        public ActionResult Details(int? id)
        {
            if (id == null)            
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            
            GymsBO gymsBO = DependencyResolver.Current.GetService<GymsBO>();
            gymsBO.Load((int)id);

            if (gymsBO == null)            
                return HttpNotFound();

            var gymsVM = mapper.Map<List<GymsVM>>(gymsBO);
            return View(gymsVM);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,GymName")] GymsVM gymsVM)
        {
            if (ModelState.IsValid)
            {
                var gymsBO = mapper.Map<GymsBO>(gymsVM);
                gymsBO.Save(gymsBO);
                return RedirectToAction("Index");
            }
            return View(gymsVM);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            
            GymsBO gymsBO = DependencyResolver.Current.GetService<GymsBO>().Load((int)id);

            if (gymsBO == null)
                return HttpNotFound();

            var gymsVM = mapper.Map<GymsVM>(gymsBO);
            return View(gymsVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,GymName")] GymsVM gymsVM)
        {
            if (ModelState.IsValid)
            {
                var gymsBO = mapper.Map<GymsBO>(gymsVM);
                gymsBO.Save(gymsBO);
                return RedirectToAction("Index");
            }
            return View(gymsVM);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            GymsBO gymsBO = DependencyResolver.Current.GetService<GymsBO>().Load((int)id);

            if (gymsBO == null)
                return HttpNotFound();
            var gymsVM = mapper.Map<GymsVM>(gymsBO);
            return View(gymsVM);
        }

      
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            GymsBO gymsBO = DependencyResolver.Current.GetService<GymsBO>().Load((int)id);
            gymsBO.DeleteSave(gymsBO);
            return RedirectToAction("Index");
        }

       
    }
}
