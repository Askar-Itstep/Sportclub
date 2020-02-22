namespace DataLayer
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using DataLayer.Entities;

    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=Model1")
        {
        }
       
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Administration> Administrations { get; set; }
        public virtual DbSet<Clients> Clients { get; set; }
        public virtual DbSet<Coaches> Coaches { get; set; }
        public virtual DbSet<GraphTraning> GraphTranings { get; set; }
        public virtual DbSet<Gyms> Gyms { get; set; }
        public virtual DbSet<Specialization> Specializations { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }

        //public System.Data.Entity.DbSet<Sportclub.ViewModel.GymsVM> GymsVMs { get; set; }
    }
    public class MyContextInitializer : DropCreateDatabaseIfModelChanges<Model1>    
    {
        protected override void Seed(Model1 context)    //1-ое обращ. из AccauntC.\Login 
        {
            //1) Roles
            Role adminRole = new Role { RoleName = "admin" };
            Role clientRole = new Role { RoleName = "client" };
            Role coacheRole = new Role { RoleName = "coache" };
            context.Roles.Add(adminRole);
            context.Roles.Add(clientRole);
            context.Roles.Add(coacheRole);

            //2) Users
            User userAdmin = new User {
                BirthDay = new DateTime(1900, 1, 1), Gender = Gender.MEN, Role = adminRole, Login = "admin", Password = "admin" };
            context.Users.Add(userAdmin);

            //3)Administrations
            Administration admin = new Administration
            {
                Status = Administration.StatusManager.ADMIN,
                User = userAdmin
            };
            context.Administrations.Add(admin);

            //4 Specializations
            Specialization[] specializations = new Specialization[]{
                new Specialization{ Title = "individual" },
                new Specialization{ Title = "dance" },
                new Specialization{Title = "boxing"}
            };
            context.Specializations.AddRange(specializations);

            //5
            Gyms[] gyms = new Gyms[]
            {
                new Gyms{ GymName="vip"},
                new Gyms{GymName="boxing"},
                new Gyms{GymName = "dance"}
            };
            context.Gyms.AddRange(gyms);
            //6
            //GraphTraning traning = new GraphTraning { GymsId = 1 };
            //context.GraphTranings.Add(traning);
            //--------------------
            context.SaveChanges();
            base.Seed(context);
        }
    }
}
