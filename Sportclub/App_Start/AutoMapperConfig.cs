using AutoMapper;
using Sportclub.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Unity;
//using DataLayer.Entities;
namespace Sportclub.App_Start
{
    public class AutoMapperConfig
    {
        public static void RegisterWithUnity(UnityContainer container)
        {
            IMapper mapper = CreateMapperConfiguration().CreateMapper();
            container.RegisterInstance<IMapper>(mapper);
        }

        public static MapperConfiguration CreateMapperConfiguration()
        {
            return new MapperConfiguration(mpr =>
            {
               // mpr.CreateMap<Administration, AuthorBO>()
               // .ConstructUsing(c => DependencyResolver.Current.GetService<AuthorBO>()).ForMember("AuthorName", opt => opt.MapFrom(
               //     c => c.FirstName + " " + c.LastName));

               // mpr.CreateMap<AuthorBO, Authors>()
               // .ConstructUsing(c => DependencyResolver.Current.GetService<Authors>()).ForMember("FirstName", opt => opt.MapFrom(
               //     c => c.AuthorName.Split(' ')[0])).ForMember("LastName", opt => opt.MapFrom(c => c.AuthorName.Split(' ')[1]));


               // mpr.CreateMap<IEnumerable<Authors>, List<AuthorBO>>()
               //.ConstructUsing(c => DependencyResolver.Current.GetService<List<AuthorBO>>());
               // //------------------------------------------------------------------------

               // mpr.CreateMap<AuthorBO, AuthorViewModel>()
               // //.ConstructUsing(c => DependencyResolver.Current.GetService<AuthorViewModel>());
               // .ConstructUsing(c => DependencyResolver.Current.GetService<AuthorViewModel>()).ForMember("FirstName", opt => opt.MapFrom(
               //     c => c.AuthorName.Split(' ')[0])).ForMember("LastName", opt => opt.MapFrom(c => c.AuthorName.Split(' ')[1]));


               // mpr.CreateMap<AuthorViewModel, AuthorBO>()
               // //.ConstructUsing(c => DependencyResolver.Current.GetService<AuthorBO>());
               // .ConstructUsing(c => DependencyResolver.Current.GetService<AuthorBO>()).ForMember("AuthorName", opt => opt.MapFrom(
               //     c => c.FirstName + " " + c.LastName));

            });
        }
    }
}