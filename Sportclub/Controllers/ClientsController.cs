using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using BusinessLayer.BusinessObject;
using DataLayer;
using DataLayer.Entities;
using DataLayer.Repository;
using Sportclub.Controllers;
using Sportclub.ViewModel;
using Sportclub.ViewModels;

namespace DataLayer.Controllers
{
    public class ClientsController : Controller
    {
        IMapper mapper;
        public ClientsController() { } 
        public ClientsController(IMapper mapper)
        {
            this.mapper = mapper;
        }
        public ActionResult Index()
        {
            var clientsBO = DependencyResolver.Current.GetService<ClientsBO>().LoadAllWithInclude(nameof(User));
            List<ClientsVM> clientsVM = mapper.Map<List<ClientsVM>>(clientsBO);
            return View(clientsVM);
        }


        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var clientBO = DependencyResolver.Current.GetService<ClientsBO>().LoadAllWithInclude(nameof(User));
            var clientVM = mapper.Map<ClientsVM>(clientBO.FirstOrDefault(c => c.Id == id));

            if (clientVM == null)
                return HttpNotFound();

            return View(clientVM);
        }

        [Authorize(Roles = "admin, top_manager, manager")]
        public ActionResult Create() //админ&co может добав. нов. клиента.
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync(ClientsVM clientVM, HttpPostedFileBase upload)
        { 
            if (ModelState.IsValid) {
                ImageVM imageVM = DependencyResolver.Current.GetService<ImageVM>();
                ImageBO imageBase = DependencyResolver.Current.GetService<ImageBO>();

                System.Diagnostics.Debug.WriteLine("User: " + ModelState.IsValidField("User"));
                System.Diagnostics.Debug.WriteLine("UserId: " + ModelState.IsValidField("User.Id"));
                System.Diagnostics.Debug.WriteLine("User.FullName: " + ModelState.IsValidField("User.FullName"));
                var userBO = mapper.Map<UserBO>(clientVM.User);
                var roleBO = DependencyResolver.Current.GetService<RoleBO>().LoadAll().FirstOrDefault(r => r.RoleName == "client");
                userBO.RoleId = roleBO.Id;
                userBO.Login = clientVM.User.Email.Split('@')[0];
                userBO.Gender = GenderBO.MEN;   //default

                if (upload != null) {           //+ img, если такого нет - записать в БД и вернуть!
                    var imgBase = await BlobHelper.SetImageAsync(upload, imageVM, imageBase, userBO, mapper); 
                }
                else {
                    userBO.ImageId = 1; //default - men.png (Model1)
                }
                var clientBO = mapper.Map<ClientsBO>(clientVM);
                clientBO.User = userBO;         //user create too!
                clientBO.UserId = userBO.Id;
                clientBO.Save(clientBO);
                return new JsonResult { Data = "Данные записаны", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            else
                return  Json ( new { Data = "Ошибка создания модели!", good = false, JsonRequestBehavior = JsonRequestBehavior.AllowGet } );
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var clientBO = DependencyResolver.Current.GetService<ClientsBO>().LoadAllWithInclude(nameof(User)).FirstOrDefault(c => c.Id == id);

            if (clientBO == null) {
                return HttpNotFound();
            }
            var clientVM = mapper.Map<ClientsVM>(clientBO);
            return View(clientVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync(ClientsVM clientVM, HttpPostedFileBase upload)
        {
            ImageVM imageVM = DependencyResolver.Current.GetService<ImageVM>();
            ImageBO imageBase = DependencyResolver.Current.GetService<ImageBO>();
            if (ModelState.IsValid) {
                var clientBO = mapper.Map<ClientsBO>(clientVM);
                var roleBO = DependencyResolver.Current.GetService<RoleBO>().Load(clientVM.User.RoleId);
                clientBO.User.Role = roleBO;   //роль не меняется! 
                UserBO userBO = clientBO.User;
                if(upload != null) {
                    var imgBase = await BlobHelper.SetImageAsync(upload, imageVM, imageBase, userBO, mapper);//если такого нет - записать в БД и вернуть! 
                }
                else {
                    var currUserBO = DependencyResolver.Current.GetService<UserBO>().LoadAll().Where(u => u.Id == clientBO.UserId).FirstOrDefault();
                    userBO.ImageId = currUserBO.ImageId;
                }
                userBO.Save(userBO);
                clientBO.Save(clientBO);
                return new JsonResult { Data = "Данные записаны", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            return View(clientVM);
        }
        //-------------------------------------------------------------------------------
        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (id == null) 
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var clientBO = DependencyResolver.Current.GetService<ClientsBO>().LoadAllWithInclude(nameof(User)).FirstOrDefault(c => c.Id == id);

            if (clientBO == null) 
                return HttpNotFound();

            var clientVM = mapper.Map<ClientsVM>(clientBO);
            return View(clientVM);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var clientBO = DependencyResolver.Current.GetService<ClientsBO>().LoadAllWithInclude(nameof(User)).FirstOrDefault(c => c.Id == id);
            var userBO = clientBO.User;
            //-------поискать среди тренеров--------------
            var coacheBO = DependencyResolver.Current.GetService<CoachesBO>().LoadAllWithInclude(nameof(User)).Where(c => c.UserId == userBO.Id).FirstOrDefault();
            if (coacheBO != null)
                coacheBO.DeleteSave(coacheBO);
            //----..среди клиентов ---------------
            clientBO.DeleteSave(clientBO);
            userBO.DeleteSave(userBO);
            return RedirectToAction("Index");
        }


    }
}
