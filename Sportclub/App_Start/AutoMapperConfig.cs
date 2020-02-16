using AutoMapper;
using BusinessLayer.BusinessObject;
using DataLayer.Entities;
using Sportclub.ViewModel;
using Sportclub.ViewModels;
using System.Collections.Generic;
using System.Web.Mvc;
using Unity;

namespace DataLayer
{
    public class AutoMapperConfig
    {
        public static void RegisterWithUnity(UnityContainer container)
        {
            IMapper mapper = CreateMapperConfiguration().CreateMapper();
            container.RegisterInstance<IMapper>(mapper);
        }

        private static MapperConfiguration CreateMapperConfiguration()
        {
            return new MapperConfiguration(mpr =>
            {
                mpr.CreateMap<Administration, AdministrationBO>()
                .ConstructUsing(c => DependencyResolver.Current.GetService<AdministrationBO>());

                mpr.CreateMap<AdministrationBO, Administration>()
                .ConstructUsing(c => DependencyResolver.Current.GetService<Administration>());

                mpr.CreateMap<AdministrationBO, AdministrationVM>()
               .ConstructUsing(c => DependencyResolver.Current.GetService<AdministrationVM>());

                mpr.CreateMap<AdministrationVM, AdministrationBO>()
               .ConstructUsing(c => DependencyResolver.Current.GetService<AdministrationBO>());
                {
                    // .ForMember("UserBO", a => a.MapFrom(c =>
                    //     new UserBO
                    //     {
                    //         Id = c.Id,
                    //         FullName = c.UserVM.FullName,
                    //         BirthDay = c.UserVM.BirthDay,
                    //         Phone = c.UserVM.Phone,
                    //         Email = c.UserVM.Email,
                    //         GenderBO = (GenderBO)c.UserVM.GenderVM,
                    //         Login = c.UserVM.Login,
                    //         Password = c.UserVM.Password,
                    //         RoleBOId = c.UserVM.RoleVMId,
                    //         RoleBO = new RoleBO { Id = c.UserVM.RoleVM.Id, RoleName = c.UserVM.RoleVM.RoleName },
                    //         Token = c.UserVM.Token
                    //     }))
                    //.ForMember("Status", src => src.MapFrom(a => a.Status))
                    //.ForMember("UserBOId", src => src.MapFrom(a => a.UserVMId))
                    //.ForMember("Id", src => src.MapFrom(a => a.Id));
                }
                //------------------------------------------------------------------------

                mpr.CreateMap<Clients, ClientsBO>()
                .ConstructUsing(c => DependencyResolver.Current.GetService<ClientsBO>());

                mpr.CreateMap<ClientsBO, Clients>()
                .ConstructUsing(c => DependencyResolver.Current.GetService<Clients>());

                mpr.CreateMap<IEnumerable<Clients>, List<ClientsBO>>()
               .ConstructUsing(c => DependencyResolver.Current.GetService<List<ClientsBO>>());

                mpr.CreateMap<ClientsVM, ClientsBO>()
               .ConstructUsing(c => DependencyResolver.Current.GetService<ClientsBO>());

                mpr.CreateMap<ClientsBO, ClientsVM>()
                .ConstructUsing(c => DependencyResolver.Current.GetService<ClientsVM>());
                //---------------------------------------------------------------------------------------

                mpr.CreateMap<Coaches, CoachesBO>()
               .ConstructUsing(c => DependencyResolver.Current.GetService<CoachesBO>());

                mpr.CreateMap<CoachesBO, Coaches>()
                .ConstructUsing(c => DependencyResolver.Current.GetService<Coaches>());

                mpr.CreateMap<CoachesVM, CoachesBO>()
               .ConstructUsing(c => DependencyResolver.Current.GetService<CoachesBO>());

                mpr.CreateMap<CoachesBO, CoachesVM>()
                .ConstructUsing(c => DependencyResolver.Current.GetService<CoachesVM>());

                mpr.CreateMap<IEnumerable<Coaches>, List<CoachesBO>>()
               .ConstructUsing(c => DependencyResolver.Current.GetService<List<CoachesBO>>());
                //---------------------------------------------------------------------------------------
              
                mpr.CreateMap<GraphTraning, GraphTraningBO>()
               .ConstructUsing(c => DependencyResolver.Current.GetService<GraphTraningBO>());

                mpr.CreateMap<GraphTraningBO, GraphTraning>()
                .ConstructUsing(c => DependencyResolver.Current.GetService<GraphTraning>());

                mpr.CreateMap<GraphTraningVM, GraphTraningBO>()
              .ConstructUsing(c => DependencyResolver.Current.GetService<GraphTraningBO>());

                mpr.CreateMap<GraphTraningBO, GraphTraningVM>()
                .ConstructUsing(c => DependencyResolver.Current.GetService<GraphTraningVM>())
                .ForMember(g=>g.Clients, c=>c.MapFrom(u=>u.Clients));

                mpr.CreateMap<IEnumerable<GraphTraning>, List<GraphTraningBO>>()
               .ConstructUsing(c => DependencyResolver.Current.GetService<List<GraphTraningBO>>());
                //---------------------------------------------------------------------------------------

                mpr.CreateMap<Gyms, GymsBO>()
               .ConstructUsing(c => DependencyResolver.Current.GetService<GymsBO>());

                mpr.CreateMap<GymsBO, Gyms>()
                .ConstructUsing(c => DependencyResolver.Current.GetService<Gyms>());

                mpr.CreateMap<IEnumerable<Gyms>, List<GymsBO>>()
               .ConstructUsing(c => DependencyResolver.Current.GetService<List<GymsBO>>());

                mpr.CreateMap<GymsVM, GymsBO>()
              .ConstructUsing(c => DependencyResolver.Current.GetService<GymsBO>());

                mpr.CreateMap<GymsBO, GymsVM>()
                .ConstructUsing(c => DependencyResolver.Current.GetService<GymsVM>());

              //  mpr.CreateMap<IEnumerable<GymsBO>, List<GymsVM>>()
              //.ConstructUsing(c => DependencyResolver.Current.GetService<List<GymsVM>>());
                //---------------------------------------------------------------------------------------

                mpr.CreateMap<Role, RoleBO>()
               .ConstructUsing(c => DependencyResolver.Current.GetService<RoleBO>());

                mpr.CreateMap<RoleBO, Role>()
                .ConstructUsing(c => DependencyResolver.Current.GetService<Role>());

                mpr.CreateMap<RoleVM, RoleBO>()
              .ConstructUsing(c => DependencyResolver.Current.GetService<RoleBO>());

                mpr.CreateMap<RoleBO, RoleVM>()
                .ConstructUsing(c => DependencyResolver.Current.GetService<RoleVM>());
                //---------------------------------------------------------------------------------------

                mpr.CreateMap<Specialization, SpecializationBO>()
               .ConstructUsing(c => DependencyResolver.Current.GetService<SpecializationBO>()).ReverseMap();

                //mpr.CreateMap<SpecializationBO, Specialization>()
                //.ConstructUsing(c => DependencyResolver.Current.GetService<Specialization>());

                mpr.CreateMap<SpecializationVM, SpecializationBO>()
               .ConstructUsing(c => DependencyResolver.Current.GetService<SpecializationBO>()).ReverseMap();

                //mpr.CreateMap<SpecializationBO, SpecializationVM>()
                //.ConstructUsing(c => DependencyResolver.Current.GetService<SpecializationVM>());

                // mpr.CreateMap<IEnumerable<SpecializationBO>, List<SpecializationVM>>()
                //.ConstructUsing(c => DependencyResolver.Current.GetService<List<SpecializationVM>>());
                //---------------------------------------------------------------------------------------

                mpr.CreateMap<User, UserBO>()
               .ConstructUsing(c => DependencyResolver.Current.GetService<UserBO>())
                .ForMember(u => u.Gender, u => u.MapFrom(c => (int)c.Gender));
                //.ForMember(u => u.Token, opt => opt.MapFrom(s => s != null ? s.Token : ""))
                //.ReverseMap();

                mpr.CreateMap<UserBO, User>()
                .ConstructUsing(c => DependencyResolver.Current.GetService<User>())
                 .ForMember(u => u.Gender, u => u.MapFrom(c => (int)c.Gender));


                mpr.CreateMap<UserBO, UserVM>()
             .ConstructUsing(c => DependencyResolver.Current.GetService<UserVM>())
                .ForMember(u => u.Gender, u => u.MapFrom(c => (int)c.Gender));
                //.ForMember(u => u.Token, opt => opt.MapFrom(s => s != null ? s.Token : ""))
                //.ReverseMap();

                mpr.CreateMap<UserVM, UserBO>()
              .ConstructUsing(c => DependencyResolver.Current.GetService<UserBO>())
               .ForMember(u => u.Gender, u => u.MapFrom(c => (int)c.Gender));

                mpr.CreateMap<IEnumerable<UserBO>, List<UserVM>>()
               .ConstructUsing(c => DependencyResolver.Current.GetService<List<UserVM>>());

                //---------------------------------------------------------------------------------------

                mpr.CreateMap<Image, ImageBO>()
               .ConstructUsing(c => DependencyResolver.Current.GetService<ImageBO>());

                mpr.CreateMap<ImageBO, Image>()
                .ConstructUsing(c => DependencyResolver.Current.GetService<Image>());


                mpr.CreateMap<ImageBO, ImageVM>()
             .ConstructUsing(c => DependencyResolver.Current.GetService<ImageVM>());

                mpr.CreateMap<ImageVM, ImageBO>()
              .ConstructUsing(c => DependencyResolver.Current.GetService<ImageBO>());

                mpr.CreateMap<IEnumerable<ImageBO>, List<ImageVM>>()
               .ConstructUsing(c => DependencyResolver.Current.GetService<List<ImageVM>>());
            });
        }

    }
}