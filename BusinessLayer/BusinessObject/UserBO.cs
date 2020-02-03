﻿using AutoMapper;
using DataLayer.Entities;
using DataLayer.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace BusinessLayer.BusinessObject
{
    public class UserBO : BaseBusinessObject
    {
        #region simple fields
        public int Id { get; set; }

        public string FullName { get; set; }

        [Required]
        public DateTime BirthDay { get; set; }


        [RegularExpression(@"(^\+\d{1,2})?((\(\d{3}\))|(\-?\d{3}\-)|(\d{3}))((\d{3}\-\d{4})|(\d{3}\-\d\d\  
                    -\d\d)|(\d{7})|(\d{3}\-\d\-\d{3})) ", ErrorMessage = "Некорректный номер")]
        public string Phone { get; set; }


        [RegularExpression(@"(/\A[^@]+@([^@\.]+\.)+[^@\.]+\z/)) ", ErrorMessage = "Некорректный e-mail")]
        public string Email { get; set; }
        public GenderBO GenderBO { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }


        public int RoleBOId { get; set; }
        public RoleBO RoleBO { get; set; }

        public string Token { get; set; }   //manager, coache
        #endregion
        //----------------------------------------------------------------------
        public UserBO() { }
        readonly IUnityContainer unityContainer;
        public UserBO(IMapper mapper, UnitOfWork unitOfWork, IUnityContainer container)
            : base(mapper, unitOfWork)
        {
            unityContainer = container;
        }
        public IEnumerable<UserBO> LoadAll()  //из DataObj в BusinessObj
        {
            var users = unitOfWork.Users .GetAll().ToList();
            if (mapper == null)
                System.Diagnostics.Debug.WriteLine("users[1]: " + users[1].FullName);
            var res = users.AsEnumerable().Select(a => mapper.Map<UserBO>(a)).ToList();
            return res;
        }
        public IEnumerable<UserBO> LoadAllWithInclude(params string[] properties)  //из DataObj в BusinessObj
        {
            var users = unitOfWork.Users.Include(properties);
            var res = users.AsEnumerable().Select(a => mapper.Map<UserBO>(a)).ToList();
            return res;
        }
        public void Load(int id)
        {
            var user = unitOfWork.Users.GetById(id);
            mapper.Map(user, this);
        }
        public void Save(UserBO userBO)
        {
            var user = mapper.Map<User>(userBO);
            if (userBO.Id == 0) {
                Add(user);
            }
            else {
                Update(user);
            }
            unitOfWork.Users.Save();
        }
        private void Add(User user)
        {
            unitOfWork.Users.Create(user);
        }
        private void Update(User user)
        {
            unitOfWork.Users.Update(user);
        }
        public void DeleteSave(UserBO userBO)
        {
            var user = mapper.Map<User>(userBO);
            unitOfWork.Users.Delete(user.Id);
            unitOfWork.Users.Save();
        }
    }
}

