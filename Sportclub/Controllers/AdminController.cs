using Sportclub.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sportclub.Controllers
{
    public class AdminController : Controller
    {
       
        [Authorize(Roles = "admin, top_manager, manager")] //CustomRoleProvider!
        public ActionResult Index()
        {
            using(Model1 db = new Model1())
            {
                return View(db.Administrations.Include(a=>a.User).ToList());
            }
            
        }
        
        public ActionResult Details(int? id)
        {
            if (id == null)
                return HttpNotFound();
            using (Model1 db = new Model1())
            {
                return View(db.Administrations.Include(a => a.User).ToList().Find(a=>a.Id==id));
            }
        }
        
        public ActionResult Create()
        {
            using (Model1 db = new Model1())
            {
                return View();
            }
        }

       
        [HttpPost]
        public ActionResult Create(Administration manager)//FormCollection collection
        {
            try
            {
                using(Model1 db = new Model1())
                {
                    db.Administrations.Add(manager);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }                
            }
            catch
            {
                return View(manager);
            }
        }

      
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return HttpNotFound();
            using (Model1 db = new Model1())
            {
                return View(db.Administrations.Include(nameof(User)).ToList().Find(a=>a.Id==id));
            }
        }
        
        [HttpPost]
        public ActionResult Edit(Administration manager) //int id, FormCollection collection
        {
            try
            {
                using (Model1 db = new Model1())
                {
                    db.Entry(manager).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }                
            }
            catch
            {
                return View(manager);
            }
        }
        
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return HttpNotFound();
            using (Model1 db = new Model1())
            {
                return View(db.Administrations.Include(a => a.User).ToList().Find(a => a.Id == id));
            }            
        }
        
        [HttpPost]
        public ActionResult Delete(Administration manager)//int id, FormCollection collection
        {
            try
            {
                using (Model1 db = new Model1())
                {
                    db.Administrations.Remove(manager);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch
            {
                return View(manager);
            }
        }
    }
}
