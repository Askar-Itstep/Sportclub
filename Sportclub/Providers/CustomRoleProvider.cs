using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
namespace DataLayer.Providers
{
    public class CustomRoleProvider : RoleProvider
    {
        public override string ApplicationName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetRolesForUser(string username)
        {
            string[] roles = new string[] { };
            if (username.Contains('@'))
                username = username.Split('@')[0];
            using(Model1 db = new Model1())
            {                
                User user = db.Users.Include(nameof(Role)).FirstOrDefault(u => u.Login == username);
                if (user != null && user.Role != null)
                {
                    roles = new string[] { user.Role.RoleName };
                }
                return roles;
            }
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            using (Model1 db = new Model1())
            {
                // Получаем пользователя
                User user = db.Users.Include(nameof(Role)).FirstOrDefault(u => u.Email == username);

                if (user != null && user.Role != null && user.Role.RoleName == roleName)
                    return true;
                else
                    return false;
            }
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}