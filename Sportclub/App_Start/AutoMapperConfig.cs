using AutoMapper;
using System.Collections.Generic;
using BusinessLayer.BusinessObject;
using DataLayer.Entities;
using System;
using System.Web.Mvc;
using Unity;
using Sportclub.ViewModel;

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
                .ConstructUsing(c => DependencyResolver.Current.GetService<AdministrationBO>())
                .ForMember("UserBOId", a => a.MapFrom(c => c.UserId)).ForMember("UserBO", a => a.MapFrom(c => c.User));

                mpr.CreateMap<AdministrationBO, Administration>()
                .ConstructUsing(c => DependencyResolver.Current.GetService<Administration>()).ForMember("UserId", a=>a.MapFrom(c=>c.UserBOId));

                mpr.CreateMap<AdministrationBO, AdministrationVM>()
               .ConstructUsing(c => DependencyResolver.Current.GetService<AdministrationVM>()).ForMember("UserVM", a => a.MapFrom(c =>
                new UserVM { FullName= c.UserBO.FullName, BirthDay = c.UserBO.BirthDay, Email = c.UserBO.Email, Phone = c.UserBO.Phone }
                ));
                mpr.CreateMap<AdministrationVM, AdministrationBO>()
               .ConstructUsing(c => DependencyResolver.Current.GetService<AdministrationBO>())
                            .ForMember("UserBO", a => a.MapFrom(c =>
                                new UserVM
                                {
                                    Id = c.Id,
                                    FullName = c.UserVM.FullName,
                                    BirthDay = c.UserVM.BirthDay,
                                    Phone = c.UserVM.Phone,
                                    Email = c.UserVM.Email,
                                    GenderVM = c.UserVM.GenderVM,
                                    Login = c.UserVM.Login,
                                    Password = c.UserVM.Password,
                                    RoleVMId = c.UserVM.RoleVMId,
                                    RoleVM = new RoleVM { Id = c.UserVM.RoleVM.Id, RoleName = c.UserVM.RoleVM.RoleName },
                                    Token = c.UserVM.Token
                                }))
                           .ForMember("Status", src => src.MapFrom(a => AdministrationVM.StatusManager.MANAGER))
                           //.ForMember("UserBOId", src => src.MapFrom(a => a.UserVMId))
                           .ForMember("UserBOId", src => src.Ignore())
                           //.ForMember("Id", src => src.MapFrom(a => a.Id));
                           .ForMember("Id", src => src.Ignore());

                // mpr.CreateMap<IEnumerable<Administration>, List<AdministrationBO>>()
                //.ConstructUsing(c => DependencyResolver.Current.GetService<List<AdministrationBO>>());
                //------------------------------------------------------------------------

                mpr.CreateMap<Clients, ClientsBO>()
                .ConstructUsing(c => DependencyResolver.Current.GetService<ClientsBO>());

                mpr.CreateMap<ClientsBO, Clients>()
                .ConstructUsing(c => DependencyResolver.Current.GetService<Clients>()).ForMember("UserId", c=>c.MapFrom(cl => cl.UserBOId))
                                  .ForMember("GraphicId", c => c.MapFrom(cl => cl.GraphicBOId));

               // mpr.CreateMap<IEnumerable<Clients>, List<ClientsBO>>()
               //.ConstructUsing(c => DependencyResolver.Current.GetService<List<ClientsBO>>());
                //---------------------------------------------------------------------------------------

                mpr.CreateMap<Coaches, CoachesBO>()
               .ConstructUsing(c => DependencyResolver.Current.GetService<CoachesBO>()).ForMember("SpecializationBOId", c => c.MapFrom(co => co.SpecializationId))
                                            .ForMember("UserBOId", c => c.MapFrom(cl => cl.UserId));

                mpr.CreateMap<CoachesBO, Coaches>()
                .ConstructUsing(c => DependencyResolver.Current.GetService<Coaches>()).ForMember("SpecializationId", c => c.MapFrom(co => co.SpecializationBOId))
                                            .ForMember("UserId", c => c.MapFrom(cl => cl.UserBOId));

               // mpr.CreateMap<IEnumerable<Coaches>, List<CoachesBO>>()
               //.ConstructUsing(c => DependencyResolver.Current.GetService<List<CoachesBO>>());
                //---------------------------------------------------------------------------------------

                mpr.CreateMap<Gender, GenderBO>()
               .ConstructUsing(c => DependencyResolver.Current.GetService<GenderBO>());

                mpr.CreateMap<GenderBO, Gender>()
                .ConstructUsing(c => DependencyResolver.Current.GetService<Gender>());

               // mpr.CreateMap<IEnumerable<Gender>, List<GenderBO>>()
               //.ConstructUsing(c => DependencyResolver.Current.GetService<List<GenderBO>>());
                //---------------------------------------------------------------------------------------

                mpr.CreateMap<GraphTraning, GraphTraningBO>()
               .ConstructUsing(c => DependencyResolver.Current.GetService<GraphTraningBO>()).ForMember("CoacheBOId", u => u.MapFrom(c => c.CoacheId));

                mpr.CreateMap<GraphTraningBO, GraphTraning>()
                .ConstructUsing(c => DependencyResolver.Current.GetService<GraphTraning>()).ForMember("CoacheId", u => u.MapFrom(c => c.CoacheBOId));

               // mpr.CreateMap<IEnumerable<GraphTraning>, List<GraphTraningBO>>()
               //.ConstructUsing(c => DependencyResolver.Current.GetService<List<GraphTraningBO>>());
                //---------------------------------------------------------------------------------------

                mpr.CreateMap<Gyms, GymsBO>()
               .ConstructUsing(c => DependencyResolver.Current.GetService<GymsBO>());

                mpr.CreateMap<GymsBO, Gyms>()
                .ConstructUsing(c => DependencyResolver.Current.GetService<Gyms>());

               // mpr.CreateMap<IEnumerable<Gyms>, List<GymsBO>>()
               //.ConstructUsing(c => DependencyResolver.Current.GetService<List<GymsBO>>());
                //---------------------------------------------------------------------------------------

                mpr.CreateMap<LoginModel, LoginModelBO>()
               .ConstructUsing(c => DependencyResolver.Current.GetService<LoginModelBO>());

                mpr.CreateMap<LoginModelBO, LoginModel>()
                .ConstructUsing(c => DependencyResolver.Current.GetService<LoginModel>());

               // mpr.CreateMap<IEnumerable<LoginModel>, List<LoginModelBO>>()
               //.ConstructUsing(c => DependencyResolver.Current.GetService<List<LoginModelBO>>());
                //---------------------------------------------------------------------------------------

                mpr.CreateMap<RegisterModel, RegisterModelBO>()
               .ConstructUsing(c => DependencyResolver.Current.GetService<RegisterModelBO>());

                mpr.CreateMap<RegisterModelBO, RegisterModel>()
                .ConstructUsing(c => DependencyResolver.Current.GetService<RegisterModel>());

               // mpr.CreateMap<IEnumerable<RegisterModel>, List<RegisterModelBO>>()
               //.ConstructUsing(c => DependencyResolver.Current.GetService<List<RegisterModelBO>>());
                //---------------------------------------------------------------------------------------

                mpr.CreateMap<Role, RoleBO>()
               .ConstructUsing(c => DependencyResolver.Current.GetService<RoleBO>());

                mpr.CreateMap<RoleBO, Role>()
                .ConstructUsing(c => DependencyResolver.Current.GetService<Role>());

               // mpr.CreateMap<IEnumerable<Role>, List<RoleBO>>()
               //.ConstructUsing(c => DependencyResolver.Current.GetService<List<RoleBO>>());
                //---------------------------------------------------------------------------------------

                mpr.CreateMap<Specialization, SpecializationBO>()
               .ConstructUsing(c => DependencyResolver.Current.GetService<SpecializationBO>());

                mpr.CreateMap<SpecializationBO, Specialization>()
                .ConstructUsing(c => DependencyResolver.Current.GetService<Specialization>());

               // mpr.CreateMap<IEnumerable<Specialization>, List<SpecializationBO>>()
               //.ConstructUsing(c => DependencyResolver.Current.GetService<List<SpecializationBO>>());
                //---------------------------------------------------------------------------------------

                mpr.CreateMap<User, UserBO>()
               .ConstructUsing(c => DependencyResolver.Current.GetService<UserBO>()).ForMember("RoleBOId", u => u.MapFrom(c => c.RoleId));

                mpr.CreateMap<UserBO, User>()
                .ConstructUsing(c => DependencyResolver.Current.GetService<User>()).ForMember("RoleId", u=>u.MapFrom(c=>c.RoleBOId));

                mpr.CreateMap<User, UserVM>()
              .ConstructUsing(c => DependencyResolver.Current.GetService<UserVM>())
              .ForMember("RoleVM", u => u.MapFrom(c => c.Role)).ForMember("GenderVM", u => u.MapFrom(c => c.Gender));

                // mpr.CreateMap<IEnumerable<User>, List<UserBO>>()
                //.ConstructUsing(c => DependencyResolver.Current.GetService<List<UserBO>>());
            });
        }
    
    }
}