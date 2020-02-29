namespace DataLayer
{
    using DataLayer.Entities;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Blob;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data.Entity;
    using System.IO;
    using System.Threading.Tasks;

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
        public virtual DbSet<Image> Images { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }

    }
    public class MyContextInitializer : CreateDatabaseIfNotExists<Model1>    //DropCreateDatabaseIfModelChanges
    {
        private string[] connectName = { "blobContainer", "blobBoxBackground" };
        private string[] uripath =  {"https://storageblobitstep.blob.core.windows.net/containerblob",
                                     "https://imagesbackground.blob.core.windows.net/backgrounds" };
        private string[] blobContainerName = {"containerblob", "backgrounds" };    //именя контейнеров в Azure
        public static bool UploadFile(FileStream fileStream, string connectName, string boxName)
        {
            try {
                string filename = fileStream.Name;
                string storagekey = ConfigurationManager.ConnectionStrings[connectName].ConnectionString;
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(storagekey);

                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = blobClient.GetContainerReference(boxName);

                container.SetPermissions(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });
                CloudBlockBlob blockBlob = container.GetBlockBlobReference(filename);

                blockBlob.UploadFromStreamAsync(fileStream);

                return true;
            }
            catch (Exception ex) {
                System.Diagnostics.Debug.WriteLine("Upload Error: " + ex.Message);
                return false;
            }
        }
        protected override void Seed(Model1 context)    //1-ое обращ. из AccauntC.\Login 
        {
            //1) Roles
            List<Role> roles = new List<Role>()   {
                new Role { RoleName = "admin" }, new Role { RoleName = "manager" }, new Role { RoleName = "top_manager" }, new Role { RoleName = "client" },
                new Role { RoleName = "coache" }, new Role { RoleName = "head_coache" }, new Role { RoleName = "top_coache" }
            };

            context.Roles.AddRange(roles);
            //2) Images            
            //----------нужно сначала загрузить 1-ый файл из папки File--а также для фона---------------
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Files"); //../Sportclub/Sportclub/Files
            DirectoryInfo dir = new DirectoryInfo(path);
            string filename = "";
            foreach (var item in dir.GetFiles()) {
                if (item.Name == "men.png" || item.Name == "default.png" || item.Name == "men.jpg" || item.Name == "default.jpg") {
                    filename = item.Name;
                    using (var filestream = File.Open(item.FullName, FileMode.Open)) {
                        UploadFile(filestream, connectName[0], blobContainerName[0]);
                    }
                }
                if (item.Name.Contains("sport")) {
                    filename = item.Name;
                    using (var filestream = File.Open(item.FullName, FileMode.Open)) {
                        UploadFile(filestream, connectName[1], blobContainerName[1]);
                    }
                }
            }
            //сохр. в БД путей для изобр. юзеров
            string uriStr = Path.Combine(uripath[0], filename);    
            Image image = new Image { Filename = "", URI = uriStr };

            context.Images.Add(image);

            //2b)Users
            User userAdmin = new User
            {
                BirthDay = new DateTime(1900, 1, 1),
                Gender = Gender.MEN,
                Role = roles.Find(r=>r.RoleName=="admin"),
                Login = "admin",
                Password = "admin",
                ImageId = 1
            };
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
            //--------------------
            context.SaveChanges();
            base.Seed(context);
        }
    }

}
