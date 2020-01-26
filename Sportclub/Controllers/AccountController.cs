using Sportclub.Entities;
using Sportclub.Models;
using Sportclub.Providers;
using Sportclub.Repository;
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
        private UnitOfWork unityOfWork;
        public AccountController()
        {
            unityOfWork = new UnitOfWork();
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model)//вызыв-ся 1-ым после Global\Application_Start\Database
        {
            if (ModelState.IsValid) {
                User user = null;
                string nick = "";
                if (model.Login.Contains('@'))
                    nick = model.Login.Split('@')[0];
                user = unityOfWork.Users.Include(nameof(Role))
                    .FirstOrDefault(u => (u.Login.Equals(model.Login) || u.Login.Equals(nick)) && u.Password.Equals(model.Password));



                if (user != null) {
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
                User user = null;                       //есть ли такой юзер?
                user = unityOfWork.Users.GetAll()
                .FirstOrDefault(u => u.Email == model.Email || u.FullName.Equals(model.FullName));

                if (user == null) {
                    // создаем нового пользователя

                    user = CreateUser(model);

                    //..и если юзер добавлен в бд - загруз. в куки
                    if (user != null) {
                        FormsAuthentication.SetAuthCookie(model.FullName, true);
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                    ModelState.AddModelError("", "Пользователь с таким логином или именем уже существует");
            }
            return View(model);
        }

        private User CreateUser(RegisterModel model)
        {
            User user;
            string login = model.Email.Split('@')[0];
            Role role = unityOfWork.Roles.GetAll().Where(r => r.RoleName.Contains("client")).FirstOrDefault();
            role = IsRole(role, "client");    //проверка, если null-установ.
            user = new User
            {
                FullName = model.FullName,
                BirthDay = model.BirthDay,
                Phone = model.Phone,
                Email = model.Email,
                Login = login,
                Password = model.Password
            };  //Role - далее; Gender, Graphic - в Edit

            //1---------------------client------------------------------------------------
            if (model.Token == null || model.Token.Equals(""))  //Токен выдается административно или не выдается
            {
                user.RoleId = role.Id;
                unityOfWork.Users.Create(user);
                unityOfWork.Clients.Create(new Clients { User = user });
            }
            //2--------------managers--------------------------------
            else if (model.Token.Contains("manager"))           //может быть и 1234-ABCD-..
            {
                if (!model.Token.Contains("top")) {
                    role = unityOfWork.Roles.GetAll().Where(r => r.RoleName.Equals("manager")).FirstOrDefault();
                    role = IsRole(role, "manager");
                    user.RoleId = role.Id;
                    user.Token = "manager1";
                    unityOfWork.Administration.Create(new Administration { User = user, Status = Administration.StatusManager.MANAGER });
                }
                else {
                    role = unityOfWork.Roles.GetAll().Where(r => r.RoleName.Equals("top_manager")).FirstOrDefault();
                    role = IsRole(role, "top_manager");
                    user.RoleId = role.Id;
                    user.Token = "top_manager";
                    unityOfWork.Administration.Create(new Administration { User = user, Status = Administration.StatusManager.TOP_MANAGER });
                }
            }
            //3-------------coaches-------------------------------
            else if (model.Token.Contains("coache")) {
                var specialization = unityOfWork.Specializations.GetAll().Where(s => s.Title.Contains("individ")).FirstOrDefault();   //сначала все индивидуал
                int key = int.Parse(new Regex(@"\d").Match(model.Token).Value);
                if (key == 1) {
                    role = unityOfWork.Roles.GetAll().Where(r => r.RoleName.Equals("coache")).FirstOrDefault();
                    role = IsRole(role, "coache");
                    //user.Role = role;
                    user.RoleId = role.Id;
                    user.Token = "coache1";
                    unityOfWork.Coaches.Create(new Coaches { User = user, Status = Coaches.StatusCoach.COACHE, SpecializationId = specialization.Id });
                }
                else if (key == 2) {
                    role = unityOfWork.Roles.GetAll().Where(r => r.RoleName.Equals("head_coache")).FirstOrDefault();
                    role = IsRole(role, "head_coache");
                    user.RoleId = role.Id;
                    user.Token = "coache2";
                    unityOfWork.Coaches.Create(new Coaches { User = user, Status = Coaches.StatusCoach.HEAD_COACHE_HALL, SpecializationId = specialization.Id });
                }
                else {
                    role = unityOfWork.Roles.GetAll().Where(r => r.RoleName.Equals("top_coache")).FirstOrDefault();
                    role = IsRole(role, "top_coache");
                    user.RoleId = role.Id;
                    user.Token = "coache3";
                    unityOfWork.Coaches.Create(new Coaches { User = user, Status = Coaches.StatusCoach.TOP_COACHE, SpecializationId = specialization.Id });
                }
            }
            unityOfWork.Users.Save();
            unityOfWork.Clients.Save();
            unityOfWork.Administration.Save();
            unityOfWork.Coaches.Save();
            //юзаем юзера..
            user = unityOfWork.Users.GetAll().Where(u => u.FullName == model.FullName && u.Password == model.Password).FirstOrDefault();
            return user;
        }

        private Role IsRole(Role role, string param)
        {

            if (role == null)//если в БД нет роли client
            {
                unityOfWork.Roles.Create(new Role { RoleName = param }); //пока
                unityOfWork.Roles.Save();
                role = unityOfWork.Roles.GetAll().Where(r => r.RoleName.Equals(param)).FirstOrDefault();
            }

            return role;

        }

        public ActionResult Logoff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account");
        }
    }

}