using Autofac;
using Microsoft.AspNetCore.Mvc;
using ServiceContracts;

namespace DIExample.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICitiesService _citiesService1;
        private readonly ICitiesService _citiesService2;
        private readonly ICitiesService _citiesService3;
        private readonly ILifetimeScope _lifetimeScope; // Autofac

        // ****************** DI Factory ***************
        /*private readonly IServiceScopeFactory _serviceScopeFactory;  // DI container
        public HomeController(ICitiesService citiesService1, ICitiesService citiesService2, ICitiesService citiesService3, IServiceScopeFactory serviceScopeFactory)
        {
            _citiesService1 = citiesService1;
            _citiesService2 = citiesService2;
            _citiesService3 = citiesService3;
            _serviceScopeFactory = serviceScopeFactory;
        }*/

        public HomeController(ICitiesService citiesService1, ICitiesService citiesService2, ICitiesService citiesService3, ILifetimeScope lifetimeScope)
        {
            _citiesService1 = citiesService1;
            _citiesService2 = citiesService2;
            _citiesService3 = citiesService3;
            _lifetimeScope = lifetimeScope;
        }


        [Route("/")]
        // public IActionResult Index([FromServices] ICitiesService _citiesService) // ----- Method Injection example - 
        public IActionResult Index()

        {
            List<string> cities = _citiesService1.GetCities();
            ViewBag.InstanceId_CitiesService_1 = _citiesService1.ServiceInstanceId;
            ViewBag.InstanceId_CitiesService_2 = _citiesService2.ServiceInstanceId;
            ViewBag.InstanceId_CitiesService_3 = _citiesService3.ServiceInstanceId;

            // ****************** DI Container way for child scope *********

            /* using(IServiceScope scope = _serviceScopeFactory.CreateScope())
             {
                 // Inject CitiesService
                ICitiesService citiesService = scope.ServiceProvider.GetRequiredService<ICitiesService>();
                // DB work

                 ViewBag.InstanceId_CitiesService_InScope = citiesService.ServiceInstanceId;
             } // end of scope ; it calls CitiesService.Dispose()*/


            // ************ Autofac way for child scope *************

            using (ILifetimeScope scope = _lifetimeScope.BeginLifetimeScope())
            {
                // Inject CitiesService
                ICitiesService citiesService = scope.Resolve<ICitiesService>();
                // DB work

                ViewBag.InstanceId_CitiesService_InScope = citiesService.ServiceInstanceId;
            } // end of scope ; it calls CitiesService.Dispose()

            return View(cities);
        }

        private interface IServiceProviderFactory
        {
        }
    }
}
