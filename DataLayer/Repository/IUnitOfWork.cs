using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repository
{
    interface IUnitOfWork
    {
        BaseRepository<Administration> Administration { get; }
        BaseRepository<Clients> Clients { get; }
        BaseRepository<Coaches> Coaches { get; }
        //BaseRepository<Gender> Gender { get; }
        BaseRepository<GraphTraning> GraphTranings { get; }
        BaseRepository<Gyms> Gyms { get; }
        BaseRepository<Role> Roles { get; }
        BaseRepository<Specialization> Specialization { get; }
        BaseRepository<User> Users { get; }
    }
}
