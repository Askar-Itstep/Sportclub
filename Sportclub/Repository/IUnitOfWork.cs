﻿using Sportclub.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sportclub.Repository
{
    interface IUnitOfWork
    {
        BaseRepository<Administration> Administration { get; }
        BaseRepository<Clients> Clients { get; }
        BaseRepository<Coaches>Coaches { get; }
        //BaseRepository <Gender> Gender { get; }
        BaseRepository<GraphTraning> GraphTraning { get; }
        BaseRepository<Gyms> Gyms { get; }
        BaseRepository<Specialization> Specializations { get; }
        BaseRepository<User> Users { get; }
        //void Save();

    }
}
