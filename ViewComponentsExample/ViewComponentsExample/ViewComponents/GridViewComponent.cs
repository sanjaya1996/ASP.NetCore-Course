using Microsoft.AspNetCore.Mvc;
using ViewComponentsExample.Models;

namespace ViewComponentsExample.ViewComponents
{
    public class GridViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(string? gridType)
        {
            {

                PersonGridModel personGridModel = new PersonGridModel()
                {
                    GridTitle = "Persons List",
                    Persons = new List<Person>
                    {
                        new Person(){ PersonName = "John", JobTitle = "Manager"},
                        new Person(){ PersonName = "Sanjay", JobTitle = "Manager"},
                        new Person(){ PersonName = "William", JobTitle = "Clerk"}

                    }
                };

                if(gridType == "friends")
                {
                    personGridModel = new PersonGridModel()
                    {
                        GridTitle = "Friend List",
                        Persons = new List<Person>
                    {
                        new Person(){ PersonName = "Bijay", JobTitle = "Software Developer"},
                        new Person(){ PersonName = "Sam", JobTitle = "Engineering Manager"},
                        new Person(){ PersonName = "John", JobTitle = "Front-end Developer"}

                    }
                    };
                }

                return View("Sample", personGridModel); // invoked a partial view Views/Shared/Components/Grid/{Default | Sample}.cshtml

            }
        }
    }
    }
