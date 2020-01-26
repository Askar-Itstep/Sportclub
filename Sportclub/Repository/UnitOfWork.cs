using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sportclub.Entities;

namespace Sportclub.Repository
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private BaseRepository<Administration> administration;
        private BaseRepository<Clients> clients;
        private BaseRepository<Coaches> coaches;
        private BaseRepository<GraphTraning> graphTraning;
        private BaseRepository<Gyms> gyms;
        private BaseRepository<Specialization> specializations;
        private BaseRepository<User> users;
        private BaseRepository<Role> roles;

        private  Model1 db = new Model1();
        
        public BaseRepository<Administration> Administration
        {
            get
            {
                if (administration == null) {
                    administration = new BaseRepository<Administration>();
                }
                return administration;
            }
        }

        public BaseRepository<Clients> Clients
        {
            get
            {
                if (clients == null) {
                    clients = new BaseRepository<Clients>();
                }
                return clients;
            }
        }

        public BaseRepository<Coaches> Coaches
        {
            get
            {
                if (coaches == null) {
                    coaches = new BaseRepository<Coaches>();
                }
                return coaches;
            }
        }

        public BaseRepository<GraphTraning> GraphTraning
        {
            get
            {
                if (graphTraning == null) {
                    graphTraning = new BaseRepository<GraphTraning>();
                }
                return graphTraning;
            }
        }

        public BaseRepository<Gyms> Gyms
        {
            get
            {
                if (gyms == null) {
                    gyms = new BaseRepository<Gyms>();
                }
                return gyms;
            }
        }

        public BaseRepository<Specialization> Specializations
        {
            get
            {
                if (specializations == null) {
                    specializations = new BaseRepository<Specialization>();
                }
                return specializations;
            }
        }

        public BaseRepository<User> Users
        {
            get
            {
                if (users == null) {
                    users = new BaseRepository<User>();
                }
                return users;
            }
        }
        public BaseRepository<Role> Roles
        {
            get
            {
                if (roles == null) {
                    roles = new BaseRepository<Role>();
                }
                return roles;
            }
        }
        //public void Save()
        //{
        //    var transaction = db.Database.BeginTransaction();
        //    try {

        //    int var = db.SaveChanges();
        //    System.Diagnostics.Debug.WriteLine("dbUnit: " + var);

        //        transaction.Commit();
        //    }
        //    catch (Exception ex) {
        //        transaction.Rollback();
        //    }
        //}
        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed) {
                if (disposing) {
                    db.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}