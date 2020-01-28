using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataLayer.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private BaseRepository<Administration> administration;
        private BaseRepository<Clients> clients;
        private BaseRepository<Coaches> coaches;
        private BaseRepository<GraphTraning> graphTranings;
        private BaseRepository<Gyms> gyms;
        private BaseRepository<Role> roles;
        private BaseRepository<Specialization> specialization;
        private BaseRepository<User> users;
        private Model1 db;
        //public Model1 Db
        //{
        //    get { return db ?? (db = new Model1()); }
        //}
        public UnitOfWork()
        {
            db = new Model1();
        }


        public BaseRepository<Administration> Administration
        {
            get
            {
                if (administration == null)
                    administration = new BaseRepository<Administration>();
                return administration;
            }
        }
        public BaseRepository<Clients> Clients
        {
            get
            {
                if (clients == null)
                    clients = new BaseRepository<Clients>();
                return clients;
            }
        }

        public BaseRepository<Coaches> Coaches
        {
            get
            {
                if (coaches == null)
                    coaches = new BaseRepository<Coaches>();
                return coaches;
            }
        }

        public BaseRepository<GraphTraning> GraphTranings
        {
            get
            {
                if (graphTranings == null)
                    graphTranings = new BaseRepository<GraphTraning>();
                return graphTranings;
            }
        }

        public BaseRepository<Gyms> Gyms
        {
            get
            {
                if (gyms == null)
                    gyms = new BaseRepository<Gyms>();
                return gyms;
            }
        }

        public BaseRepository<Role> Roles
        {
            get
            {
                if (roles == null)
                    roles = new BaseRepository<Role>();
                return roles;
            }
        }

        public BaseRepository<Specialization> Specialization
        {
            get
            {
                if (specialization == null)
                    specialization = new BaseRepository<Specialization>();
                return specialization;
            }
        }

        public BaseRepository<User> Users
        {
            get
            {
                if (users == null)
                    users = new BaseRepository<User>();
                return users;
            }
        }
        //public void Save()
        //{
        //    Db.SaveChanges();
        //}
    }
}