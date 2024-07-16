using Microsoft.AspNetCore.Mvc;
using ViewsExample.Models;

namespace ViewsExample.Controllers
{
    public class HomeController : Controller
    {
        [Route("/home")]
        [Route("/")]
        public IActionResult Index()
        {
            ViewData["appTitle"] = "Asp.Net Core Demo App";
            List<Person> people = new List<Person>()
            {
                new Person(){Name = "John", DateOfBirth = DateTime.Parse("2000-05-06"), PersonGender = Gender.Male },
                new Person(){Name = "Linda", DateOfBirth = DateTime.Parse("2002-05-06"), PersonGender = Gender.Female },
                new Person(){Name = "Susan", DateOfBirth = DateTime.Parse("2000-05-06"), PersonGender = Gender.Other },
                new Person(){Name = "Sanjay", DateOfBirth = DateTime.Parse("1996-05-06"), PersonGender = Gender.Male },

            };
            //ViewData["people"] = people;
            //ViewBag.people = people;
            return View("Index", people); // Views/Home/Index.cshtml
            // return View("abc"); // abc.html
            // return new ViewResult() { ViewName = "abc" };
        }
        [Route("person-details/{name}")]
        public IActionResult Details(string? name)
        {
            if(name == null)
            {
                return Content("Person name can't be null");
            }

            List<Person> people = new List<Person>()
            {
                new Person(){Name = "John", DateOfBirth = DateTime.Parse("2000-05-06"), PersonGender = Gender.Male },
                new Person(){Name = "Linda", DateOfBirth = DateTime.Parse("2002-05-06"), PersonGender = Gender.Female },
                new Person(){Name = "Susan", DateOfBirth = DateTime.Parse("2000-05-06"), PersonGender = Gender.Other },
                new Person(){Name = "Sanjay", DateOfBirth = DateTime.Parse("1996-05-06"), PersonGender = Gender.Male },

            };

            Person? matchingPerson = people.Where(person => person.Name == name).FirstOrDefault();
            return View(matchingPerson); // Views/Home/Details.cshtml
        }

        [Route("person-with-product")]
        public IActionResult PersonWithProduct()
        {
            Person person = new Person() { Name = "John", DateOfBirth = DateTime.Parse("2000-05-06"), PersonGender = Gender.Male };
            Product product = new Product() { ProductId = 1, ProductName = "Air Conditioner" };
            PersonAndProductWrapperModel personAndProductWrapperModel = new PersonAndProductWrapperModel() { personData = person, productData = product };
            return View(personAndProductWrapperModel);
        }

        [Route("home/all-products")]
        public IActionResult All()
        {
            return View();
            // Views/Home/All
            // Views/Shared/All
        }
    }
}
