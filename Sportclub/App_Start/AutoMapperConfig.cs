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
                .ConstructUsing(c => DependencyResolver.Current.GetService<AdministrationBO>());
                //.ForMember("UserBOId", a => a.MapFrom(c => c.UserId)).ForMember("UserBO", a => a.MapFrom(c => c.User));
               

                mpr.CreateMap<AdministrationBO, Administration>()
                .ConstructUsing(c => DependencyResolver.Current.GetService<Administration>());
                //.ForMember("UserId", a => a.MapFrom(c => c.UserBOId));

                mpr.CreateMap<AdministrationBO, AdministrationVM>()
               .ConstructUsing(c => DependencyResolver.Current.GetService<AdministrationVM>());
                //.ForMember("UserVM", a => a.MapFrom(c => new UserVM {
                //    Id = c.UserBOId, FullName = c.UserBO.FullName, BirthDay = c.UserBO.BirthDay, Email = c.UserBO.Email, Phone = c.UserBO.Phone,
                //    GenderVM = (GenderVM)c.UserBO.GenderBO, Login = c.UserBO.Login, Password = c.UserBO.Password, RoleVMId = c.UserBO.RoleBOId,
                //    Token = c.UserBO.Token,
                //    RoleVM = new RoleVM { Id = c.UserBO.RoleBO.Id, RoleName = c.UserBO.RoleBO.RoleName }
                //}))
                //.ForMember(a=>a.UserVMId, a=>a.MapFrom(c=>c.UserBOId));
                //.ForMember(a => a.User, c => c.MapFrom(u => u.User));

                mpr.CreateMap<AdministrationVM, AdministrationBO>()
               .ConstructUsing(c => DependencyResolver.Current.GetService<AdministrationBO>());
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
                
                //------------------------------------------------------------------------
                #region some code

                mpr.CreateMap<Clients, ClientsBO>()
                .ConstructUsing(c => DependencyResolver.Current.GetService<ClientsBO>());

                mpr.CreateMap<ClientsBO, Clients>()
                .ConstructUsing(c => DependencyResolver.Current.GetService<Clients>());
                //.ForMember("UserId", c => c.MapFrom(cl => cl.UserBOId)).ForMember("GraphicId", c => c.MapFrom(cl => cl.GraphicBOId));

                // mpr.CreateMap<IEnumerable<Clients>, List<ClientsBO>>()
                //.ConstructUsing(c => DependencyResolver.Current.GetService<List<ClientsBO>>());
                //---------------------------------------------------------------------------------------

                mpr.CreateMap<Coaches, CoachesBO>()
               .ConstructUsing(c => DependencyResolver.Current.GetService<CoachesBO>());
               //.ForMember("SpecializationBOId", c => c.MapFrom(co => co.SpecializationId)).ForMember("UserBOId", c => c.MapFrom(cl => cl.UserId));

                mpr.CreateMap<CoachesBO, Coaches>()
                .ConstructUsing(c => DependencyResolver.Current.GetService<Coaches>());
                //.ForMember("SpecializationId", c => c.MapFrom(co => co.SpecializationBOId)).ForMember("UserId", c => c.MapFrom(cl => cl.UserBOId));

                // mpr.CreateMap<IEnumerable<Coaches>, List<CoachesBO>>()
                //.ConstructUsing(c => DependencyResolver.Current.GetService<List<CoachesBO>>());
                //---------------------------------------------------------------------------------------

               // mpr.CreateMap<Gender, GenderBO>()
               //.ConstructUsing(c => DependencyResolver.Current.GetService<GenderBO>());
                //mpr.CreateMap<BusinessLayer.BusinessObject.GenderBO, Entities.Gender>()
                // .ConvertUsing(c => {
                //     switch (c) {
                //         case GenderBO.MEN: return Gender.MEN;
                //         case GenderBO.WOMEN: return Gender.WOMEN;
                //         //default: return Gender.MEN;
                //     }
                // });

              //  mpr.CreateMap<BusinessLayer.BusinessObject.GenderBO, Entities.Gender>()
              //  .ConstructUsing(c => DependencyResolver.Current.GetService<Entities.Gender>());

              //  mpr.CreateMap<GenderVM, GenderBO>()
              //.ConstructUsing(c => DependencyResolver.Current.GetService<GenderBO>());

              //  mpr.CreateMap<GenderBO, GenderVM>()
              //  .ConstructUsing(c => DependencyResolver.Current.GetService<GenderVM>());
                //---------------------------------------------------------------------------------------

                mpr.CreateMap<GraphTraning, GraphTraningBO>()
               .ConstructUsing(c => DependencyResolver.Current.GetService<GraphTraningBO>());
                //.ForMember("CoacheBOId", u => u.MapFrom(c => c.CoacheId));

                mpr.CreateMap<GraphTraningBO, GraphTraning>()
                .ConstructUsing(c => DependencyResolver.Current.GetService<GraphTraning>());
                //.ForMember("CoacheId", u => u.MapFrom(c => c.CoacheBOId));

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
               .ConstructUsing(c => DependencyResolver.Current.GetService<SpecializationBO>());

                mpr.CreateMap<SpecializationBO, Specialization>()
                .ConstructUsing(c => DependencyResolver.Current.GetService<Specialization>());

                mpr.CreateMap<IEnumerable<Specialization>, List<SpecializationBO>>()
               .ConstructUsing(c => DependencyResolver.Current.GetService<List<SpecializationBO>>());
                //---------------------------------------------------------------------------------------
                #endregion

                mpr.CreateMap<User, UserBO>()
               .ConstructUsing(c => DependencyResolver.Current.GetService<UserBO>())
               //.ForMember("RoleBOId", u => u.MapFrom(c => c.RoleId)).ForMember("RoleBO", u => u.MapFrom(c => c.Role));
                .ForMember(u => u.Gender, u => u.MapFrom(c => (int)c.Gender));

                mpr.CreateMap<UserBO, User>()
                .ConstructUsing(c => DependencyResolver.Current.GetService<User>());
                //.ForMember("RoleId", u => u.MapFrom(c => c.RoleBOId));

                mpr.CreateMap<UserVM, UserBO>()
              .ConstructUsing(c => DependencyResolver.Current.GetService<UserBO>());

                mpr.CreateMap<UserBO, UserVM>()
             .ConstructUsing(c => DependencyResolver.Current.GetService<UserVM>())
                //.ForMember("RoleBOId", u => u.MapFrom(c => c.RoleVMId))
                //.ForMember("RoleBO", u => u.MapFrom(c => new RoleBO { Id = c.RoleVM.Id, RoleName = c.RoleVM.RoleName }))
                .ForMember(u=>u.Gender, u => u.MapFrom(c => (int)c.Gender));

                // mpr.CreateMap<IEnumerable<User>, List<UserBO>>()
                //.ConstructUsing(c => DependencyResolver.Current.GetService<List<UserBO>>());
            });
        }

    }
}