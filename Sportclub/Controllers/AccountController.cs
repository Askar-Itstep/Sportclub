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
        private UnitOfWork unitOfWork = new UnitOfWork();

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
                user = unitOfWork.Users.Include(nameof(Role))
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
                User user = null;                                 //есть ли такой юзер?
                user = unitOfWork.Users.GetAll()
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
                else {
                    ModelState.AddModelError("", "Пользователь с таким логином или именем уже существует");
                }
            }
            return View(model);
        }

        private User CreateUser(RegisterModel model)
        {
            User user;
            string login = model.Email.Split('@')[0];
            Role role = unitOfWork.Roles.GetAll().Where(r => r.RoleName.Contains("client")).FirstOrDefault();
            role = IsRole(role, "client");    //проверка, если надо-установ.
            user = new User
            {
                FullName = model.FullName,
                BirthDay = model.BirthDay,
                Phone = model.Phone,
                Email = model.Email,
                Login = login,
                Password = model.Password
            };  //Role - далее, Gender, Graphic - в Edit-активн.

            //1-client
            if (model.Token == null || model.Token.Equals(""))  //Токен выдается административно или не выдается
            {
                user.Role = role;
                unitOfWork.Users.Create(user);
                unitOfWork.Clients.Create(new Clients { User = user });
            }
            //2-managers
            else if (model.Token.Contains("manager"))           //может быть и 1234-ABCD-..
            {
                //int key = int.Parse(new Regex(@"\d").Match(model.Token).Value);
                if (!model.Token.Contains("top")) {
                    role = unitOfWork.Roles.GetAll().Where(r => r.RoleName.Equals("manager")).FirstOrDefault();
                    role = IsRole(role, "manager");
                    user.Role = role;
                    unitOfWork.Users.Create(user);
                    user.Token = "manager1";
                    unitOfWork.Administration.Create(new Administration { User = user, Status = Administration.StatusManager.MANAGER });
                }
                else {
                    role = unitOfWork.Roles.GetAll().Where(r => r.RoleName.Equals("top_manager")).FirstOrDefault();
                    role = IsRole(role, "top_manager");
                    user.Role = role;
                    unitOfWork.Users.Create(user);
                    user.Token = "top_manager";
                    unitOfWork.Administration.Create(new Administration { User = user, Status = Administration.StatusManager.TOP_MANAGER });
                }
                
            }
            //3-coaches
            else if (model.Token.Contains("coache")) {
                var specialization = unitOfWork.Specialization.GetAll().Where(s => s.Title.Contains("individ")).FirstOrDefault();   //сначала все индивидуал
                unitOfWork.Users.Create(user);
                int key = int.Parse(new Regex(@"\d").Match(model.Token).Value);
                if (key == 1) {
                    role = unitOfWork.Roles.GetAll().Where(r => r.RoleName.Equals("coache")).FirstOrDefault();
                    role = IsRole(role, "coache");
                    user.Role = role;
                    user.Token = "coache1";
                    unitOfWork.Coaches.Create(new Coaches { User = user, Status = Coaches.StatusCoach.COACHE, SpecializationId = specialization.Id });
                }
                else if (key == 2) {
                    role = unitOfWork.Roles.GetAll().Where(r => r.RoleName.Equals("head_coache")).FirstOrDefault();
                    role = IsRole(role, "head_coache");
                    user.Role = role;
                    user.Token = "coache2";
                    unitOfWork.Coaches.Create(new Coaches { User = user, Status = Coaches.StatusCoach.HEAD_COACHE_HALL, SpecializationId = specialization.Id });
                }
                else {
                    role = unitOfWork.Roles.GetAll().Where(r => r.RoleName.Equals("top_coache")).FirstOrDefault();
                    role = IsRole(role, "top_coache");
                    user.Role = role;
                    user.Token = "coache3";
                    unitOfWork.Coaches.Create(new Coaches { User = user, Status = Coaches.StatusCoach.TOP_COACHE, SpecializationId = specialization.Id });
                }
            }
            unitOfWork.Users.Save();
            unitOfWork.Clients.Save();
            unitOfWork.Coaches.Save();
            unitOfWork.Administration.Save();
            //юзаем юзера..
            user = unitOfWork.Users.GetAll().Where(u => u.FullName == model.FullName && u.Password == model.Password).FirstOrDefault();
            return user;
        }

        private Role IsRole(Role role, string param)
        {
            //using (Model1 db = new Model1())
            //{
            if (role == null)//если в БД нет роли client
            {
                unitOfWork.Roles.Create(new Role { RoleName = param }); //пока
                unitOfWork.Roles.Save();
                role = unitOfWork.Roles.GetAll().Where(r => r.RoleName.Equals(param)).FirstOrDefault();
            }

            return role;
            //}
        }

        public ActionResult Logoff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account");
        }
    }

}