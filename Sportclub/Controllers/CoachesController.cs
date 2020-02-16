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
using Sportclub.ViewModels;

namespace DataLayer.Controllers
{
    [Authorize(Roles ="admin, top_manager, manager, top_coache, head_coache")]
    public class CoachesController : Controller
    {
        
        IMapper mapper;
        public CoachesController() { }
        public CoachesController(IMapper mapper)
        {
            this.mapper = mapper;
        }

        public ActionResult Index()
        {
            var coachesBO = DependencyResolver.Current.GetService<CoachesBO>().LoadAllWithInclude(nameof(User), nameof(Specialization));
            var coachesVM = coachesBO.Select(co => mapper.Map<CoachesVM>(co));
            return View(coachesVM.ToList());
        }


        public ActionResult Details(int? id)
        {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var coacheBO = DependencyResolver.Current.GetService<CoachesBO>()
                                    .LoadAllWithInclude(nameof(User), nameof(Specialization)).FirstOrDefault(c => c.Id == id);

            if (coacheBO == null)
                return HttpNotFound();

            var coacheVM = mapper.Map<CoachesVM>(coacheBO);
            return View(coacheVM);
        }
        //----------------------------------------------------------------------------------------------------
        [HttpGet]
        public ActionResult Create()
        {
            var specializationsBO = DependencyResolver.Current.GetService<SpecializationBO>().LoadAll().ToList();
            specializationsBO.Add(new SpecializationBO { Title = "-- Добавить специализацию --" });
            var specializationsVM = specializationsBO.Select(s => mapper.Map<SpecializationVM>(s));
            ViewBag.Specializations = new SelectList(specializationsVM, "Id", "Title");
                                                                                 //произвести юзера  - в тренеры
            var usersBO = DependencyResolver.Current.GetService<UserBO>().LoadAll().Where(u => u.Token == null).ToList();
            var usersVM = usersBO.Select(u => mapper.Map<UserBO>(u)).ToList();
            ViewBag.UserList = new SelectList(usersVM, "Id", "FullName");
            return View();

        }
        [HttpPost]          //добавить специальн. тренеру
        public ActionResult CreateSpec(SpecializationVM specializationVM)   //обработ. ajax
        {
            if (specializationVM == null)
                return HttpNotFound();
            if (ModelState.IsValid) {
                var specBO = mapper.Map<SpecializationBO>(specializationVM);
                specBO.Save(specBO);
                var specVM = specBO.LoadAll().Select(s => mapper.Map<SpecializationVM>(s));
                ViewBag.Specializations = new SelectList(specVM, "Id", "Title");
                return new JsonResult
                {
                    Data = specVM,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            var coachesBO = DependencyResolver.Current.GetService<CoachesBO>()
                                    .LoadAllWithInclude(nameof(User), nameof(Specialization));
            var coachesVM = coachesBO.Select(co => mapper.Map<CoachesVM>(co));
            return RedirectToAction("Index", coachesVM.ToList());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CoachesVM coacheVM)    //нов. тренера только из существ. юзера!
        {                                               
            if (coacheVM == null)
                return HttpNotFound();
            if (coacheVM.UserId == 0) {
                return new HttpNotFoundResult("User is null!");
            }
            if (ModelState.IsValid) {
                var coacheBO = mapper.Map<CoachesBO>(coacheVM);
                                      //userBO = DependencyResolver.Current.GetService<UserBO>().LoadAll().FirstOrDefault(u => u.Login == userBO.Login && u.Password == userBO.Password);

                //coacheBO.User = userBO;   //нельзя - дублирование!
                coacheBO.Save(coacheBO);

                var userBO = DependencyResolver.Current.GetService<UserBO>().Load(coacheVM.UserId);
                var roleBO = DependencyResolver.Current.GetService<RoleBO>().LoadAll().FirstOrDefault(r => r.RoleName.Contains("coache"));
                if (roleBO != null)
                    userBO.RoleId = roleBO.Id;
                userBO.Token = "coache1";   //сразу в князи не получится!
                userBO.Save(userBO);  //error-attache?

                return RedirectToAction("Index");;
            }
            return View(coacheVM);
        }

        private ImageBO SetImage(HttpPostedFileBase upload, ImageVM imageVM, ImageBO imageBase)
        {
            string filename = System.IO.Path.GetFileName(upload.FileName);
            imageVM.Filename = filename;
            byte[] myBytes = new byte[upload.ContentLength];
            upload.InputStream.Read(myBytes, 0, upload.ContentLength);
            imageVM.ImageData = myBytes;
            var imgListBO = DependencyResolver.Current.GetService<ImageBO>().LoadAll().Where(i => i.Filename == imageVM.Filename).ToList();
            if (imgListBO == null || imgListBO.Count() == 0)  //если такого в БД нет - сохранить
            {
                var imageBO = mapper.Map<ImageBO>(imageVM);
                imageBase.Save(imageBO);
            }
            List<ImageBO> imageBases = DependencyResolver.Current.GetService<ImageBO>().LoadAll().Where(i => i.Filename == imageVM.Filename).ToList();
            imageBase = imageBases[0];
            return imageBase;
        }

        //-------------------------------------------------------------------------------------------------
        public ActionResult Edit(int? id)
        {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var coacheBO = DependencyResolver.Current.GetService<CoachesBO>().LoadAllWithInclude(nameof(User)).FirstOrDefault(c => c.Id == id); 
            if (coacheBO == null) {
                return HttpNotFound();
            }
            var coacheVM = mapper.Map<CoachesVM>(coacheBO);
            ViewBag.SpecList = new SelectList(DependencyResolver.Current.GetService<SpecializationBO>().LoadAll(), "Id", "Title");
            return View(coacheVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CoachesVM coacheVM, HttpPostedFileBase upload)  //в форму добавл. скрыт. поля model.UserId + model.User.Id
        {
            ImageVM imageVM = DependencyResolver.Current.GetService<ImageVM>();
            ImageBO imageBase = DependencyResolver.Current.GetService<ImageBO>();

            if (ModelState.IsValid) {
                var coacheBO = mapper.Map<CoachesBO>(coacheVM);
                var roleBO = DependencyResolver.Current.GetService<RoleBO>().LoadAll();
                if (coacheBO.Status.ToString().ToUpper().Contains("TOP")) {
                    coacheBO.User.RoleId = roleBO.FirstOrDefault(r => r.RoleName.Contains("top")).Id;
                    coacheBO.User.Token = "coache3";
                }
                if (coacheBO.Status.ToString().ToUpper().Contains("HEAD")) {
                    coacheBO.User.RoleId = roleBO.FirstOrDefault(r => r.RoleName.Contains("head")).Id;
                    coacheBO.User.Token = "coache2";
                }
                if (coacheBO.Status.ToString().ToUpper().Equals("COACHE")) {
                    coacheBO.User.RoleId = roleBO.FirstOrDefault(r => r.RoleName.Equals("coache")).Id;
                    coacheBO.User.Token = "coache";
                }
                var userBO = coacheBO.User;
                if (upload != null) { //with img
                    imageBase = SetImage(upload, imageVM, imageBase);
                    userBO.ImageId = imageBase.Id;
                }
                else {
                    userBO.Image = new ImageBO { Filename = "", ImageData = new byte[1] { 0 } };
                }
                userBO.Save(userBO);        //нужно!
                coacheBO.Save(coacheBO);
                //return RedirectToAction("Index");
                return new JsonResult { Data = "Данные перезаписаны", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            return View(coacheVM);
        }
        //-----------------------------------------------------------------------------------------
        public ActionResult Delete(int? id)
        {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var coacheBO = DependencyResolver.Current.GetService<CoachesBO>().LoadAllWithInclude(nameof(User)).FirstOrDefault(c => c.Id == id);
            if (coacheBO == null) {
                return HttpNotFound();
            }
            var coacheVM = mapper.Map<CoachesVM>(coacheBO);
            return View(coacheVM);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var coacheBO = DependencyResolver.Current.GetService<CoachesBO>().LoadAllWithInclude(nameof(User)).FirstOrDefault(c => c.Id == id);
            //var clientBO = DependencyResolver.Current.GetService<ClientsBO>().LoadAllWithInclude(nameof(User)).FirstOrDefault(c => c.UserId == coacheBO.UserId);
            //if (clientBO.Id != 0)
            //    clientBO.DeleteSave(clientBO);    //можно оставить
            var graphicBO = DependencyResolver.Current.GetService<GraphTraningBO>().LoadAll().FirstOrDefault(g => g.CoacheId == coacheBO.Id);
            if(graphicBO != null)
                graphicBO.DeleteSave(graphicBO);
            coacheBO.DeleteSave(coacheBO);
            return RedirectToAction("Index");
        }


    }
}
