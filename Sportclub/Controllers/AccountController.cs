using AutoMapper;
using BusinessLayer.BusinessObject;
using DataLayer.Entities;
using DataLayer.Providers;
using DataLayer.Repository;
using Sportclub.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Sportclub.Controllers
{
    public class AccountController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();
        IMapper mapper;
        public AccountController()
        {}
        public AccountController(IMapper mapper)
        {
            this.mapper = mapper;
        }

        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModelVM model)
        {
            if (ModelState.IsValid) {
                string nick = "";
                if (model.Login.Contains('@'))
                    nick = model.Login.Split('@')[0];
                else nick = model.Login;
                var userBO = DependencyResolver.Current.GetService<UserBO>();
                var userBOList = userBO.LoadAll();
                //var userBOList = userBO.LoadAllWithInclude(nameof(Role));
                userBO = userBOList.FirstOrDefault(u=>(u.Login.Equals(model.Login) || u.Login.Equals(nick)) && u.Password.Equals(model.Password));
                
                if (userBO != null && userBO.Login.Equals(model.Login) && userBO.Password.Equals(model.Password)) {
                    FormsAuthentication.SetAuthCookie(model.Login, true); //куки-набор (.AUTHPATH)
                    return RedirectToAction("Index", "Home");
                }
                else {
                    ModelState.AddModelError("", "Пользователя с таким логином и паролем нет"); //error validat.
                }
            }
            return View(model);
        }

        public ActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registration(RegisterModel model)
        {
            if (ModelState.IsValid) {
                var userBO = DependencyResolver.Current.GetService<UserBO>();
                userBO = userBO.LoadAll().Where(u=>u.Email != null && u.FullName != null).FirstOrDefault(u => u.Email == model.Email || u.FullName.Equals(model.FullName));
                if (userBO == null) {
                    userBO = CreateUser(model);                   
                    if (userBO != null) {   //..и если юзер добавлен в бд - загруз. в куки
                        FormsAuthentication.SetAuthCookie(model.FullName, true);
                        return RedirectToAction("Index", "Home");
                    }
                }
                else {
                    ModelState.AddModelError("", "Пользователь с таким логином или именем уже существует");
                }
            }
            return View(model);
        }

        private UserBO CreateUser(RegisterModel model)
        {
            UserBO userBO = DependencyResolver.Current.GetService<UserBO>();
            string login = model.Email.Split('@')[0];
            var roleBO = DependencyResolver.Current.GetService<RoleBO>();
            roleBO = roleBO.LoadAll().FirstOrDefault(r => r.RoleName.Contains("client"));
            roleBO = IsRole(roleBO, "client");    //проверка, если надо-установ.
            userBO.FullName = model.FullName;
            userBO.BirthDay = model.BirthDay;
            userBO.Phone = model.Phone;
            userBO.Email = model.Email;
            userBO.Login = login;
            userBO.Password = model.Password;

            //1-client
            if (model.Token == null || model.Token.Equals(""))  //Токен выдается административно или не выдается
            {
                userBO.Role = roleBO;
                userBO.RoleId = roleBO.Id;
                userBO.Save(userBO);
                userBO = userBO.LoadAll().Where(u => u.Email == model.Email && u.FullName == model.FullName).FirstOrDefault();

                var clientBO = DependencyResolver.Current.GetService<ClientsBO>();
                clientBO.User = userBO;
                clientBO.UserId = userBO.Id;
                clientBO.Save(clientBO);
            }
            //2-managers
            else if (model.Token.Contains("manager"))           //может быть и 1234-ABCD-..
            {
                var admin = DependencyResolver.Current.GetService<AdministrationBO>();
                //int key = int.Parse(new Regex(@"\d").Match(model.Token).Value);
                if (!model.Token.Contains("top") && !model.Token.Contains("2")) {
                    roleBO = roleBO.LoadAll().Where(r => r.RoleName.Equals("manager")).FirstOrDefault();
                    roleBO = IsRole(roleBO, "manager"); //может быть и NULL
                    userBO.Role = roleBO;
                    userBO.RoleId = roleBO.Id;
                    userBO.Token = "manager1";
                    userBO.Save(userBO);    //если надо - проапдейт-ся
                    admin.User = userBO;
                    admin.Status = AdministrationBO.StatusManager.MANAGER;
                }
                else {
                    roleBO = roleBO.LoadAll().Where(r => r.RoleName.Equals("top_manager")).FirstOrDefault();
                    roleBO = IsRole(roleBO, "top_manager");
                    userBO.Role = roleBO;
                    userBO.RoleId = roleBO.Id;
                    userBO.Token = "top_manager";
                    userBO.Save(userBO);
                    userBO = userBO.LoadAll().Where(u=>u.Email == model.Email && u.Password == model.Password).FirstOrDefault();
                    admin.User = userBO;
                    admin.UserId = userBO.Id;
                    admin.Status = AdministrationBO.StatusManager.TOP_MANAGER;
                }
                admin.Save(admin);
            }

            //3-coaches
            else if (model.Token.Contains("coache")) {
                var coache = DependencyResolver.Current.GetService<CoachesBO>();
                var specializationBO = DependencyResolver.Current.GetService<SpecializationBO>();
                specializationBO = specializationBO.LoadAll().Where(s => s.Title == "individual").FirstOrDefault();
                int key = int.Parse(new Regex(@"\d").Match(model.Token).Value);
                if (key == 1) {
                    roleBO = roleBO.LoadAll().Where(r => r.RoleName.Equals("coache")).FirstOrDefault();
                    roleBO = IsRole(roleBO, "coache");
                    userBO.Role = roleBO;
                    userBO.RoleId = roleBO.Id;                 
                    userBO.Token = "coache1";
                    userBO.Save(userBO); 
                    userBO = userBO.LoadAll().Where(u => u.Email == model.Email && u.Password == model.Password).FirstOrDefault();
                    coache.User = userBO;
                    coache.UserId = userBO.Id;
                    coache.Status = CoachesBO.StatusCoach.COACHE;
                    coache.SpecializationId = specializationBO.Id;    //все сначала individual
                }
               
                else if (key == 2) {
                    roleBO = roleBO.LoadAll().Where(r => r.RoleName.Equals("coache")).FirstOrDefault();
                    roleBO = IsRole(roleBO, "head_coache");
                    userBO.Role = roleBO;
                    userBO.RoleId = roleBO.Id;
                    userBO.Token = "coache2";
                    userBO.Save(userBO);  
                    userBO = userBO.LoadAll().Where(u => u.Email == model.Email && u.Password == model.Password).FirstOrDefault();
                    coache.User = userBO;
                    coache.UserId = userBO.Id;
                    coache.Status = CoachesBO.StatusCoach.HEAD_COACHE_HALL;
                    coache.SpecializationId = specializationBO.Id;
                }
                else if (key == 3) {
                    roleBO = roleBO.LoadAll().Where(r => r.RoleName.Equals("top_coache")).FirstOrDefault();
                    roleBO = IsRole(roleBO, "top_coache");
                    userBO.Role = roleBO;
                    userBO.RoleId = roleBO.Id;
                    userBO.Token = "coache3";
                    userBO.Save(userBO);
                    userBO = userBO.LoadAll().Where(u => u.Email == model.Email && u.Password == model.Password).FirstOrDefault();
                    coache.User = userBO;
                    coache.UserId = userBO.Id;
                    coache.Status = CoachesBO.StatusCoach.TOP_COACHE;
                    coache.SpecializationId = specializationBO.Id;
                }
                coache.Save(coache);
            }
            //юзаем юзера..
           userBO = userBO.LoadAll().Where(u => u.FullName == model.FullName && u.Password == model.Password).FirstOrDefault();
            return userBO;
        }

        private  RoleBO IsRole(RoleBO roleBO, string param)
        {
            if (roleBO == null || roleBO.Id == 0)//если в БД нет роли client
            {
                roleBO = DependencyResolver.Current.GetService<RoleBO>();
                roleBO.RoleName = param;
                roleBO.Save(roleBO);
                roleBO=roleBO.LoadAll().Where(r => r.RoleName.Equals(param)).FirstOrDefault();  //получить уже с ID
            }
            return roleBO;
           
        }

        public ActionResult Logoff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account");
        }
    }

}