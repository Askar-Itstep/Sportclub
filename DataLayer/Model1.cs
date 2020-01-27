namespace Sportclub
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using Sportclub.Entities;

    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=Model1")
        {
            //Database.SetInitializer(new MyContextInitializer());//Global.asax
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
    }
    public class MyContextInitializer : DropCreateDatabaseIfModelChanges<Model1>    //Always //
    {
        protected override void Seed(Model1 context)    //1-ое обращ. из AccauntC.\Login - ?
        {
            //1) Roles
            Role adminRole = new Role { RoleName = "admin" };
            Role clientRole = new Role { RoleName = "client" };
            context.Roles.Add(adminRole);
            context.Roles.Add(clientRole);

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
                new Specialization{Title = "box"},
                new Specialization{ Title = "dance" },
                new Specialization{Title = "boxing"}
            };
            context.Specializations.AddRange(specializations);

            context.SaveChanges();
            base.Seed(context);
        }
    }
}
