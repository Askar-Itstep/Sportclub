using System;
using System.Web.Mvc;
using Unity;
using Unity.AspNet.Mvc;

namespace DataLayer
{
    public static class UnityConfig
    {
        #region Unity Container
        //private static Lazy<IUnityContainer> container =
        //  new Lazy<IUnityContainer>(() =>
        //  {
        //      var container = new UnityContainer();
        //      RegisterTypes(container);
        //      return container;
        //  });

       
        //public static IUnityContainer Container => container.Value;
        #endregion

        //public static void RegisterTypes(IUnityContainer container)
        //{
        //}
        public static void RegisterTypes()
        {
            var container = new UnityContainer();

            container.RegisterInstance<IUnityContainer>(container); //method UnityConfig
            AutoMapperConfig.RegisterWithUnity(container);  //method AutoMapperConfig

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}