using Microsoft.AspNetCore.Mvc;
using ModelValidationsExample.CustomModelBinder;
using ModelValidationsExample.Models;

namespace ModelValidationsExample.Controllers
{
    public class HomeController : Controller
    {
        [Route("register")]
        // Below is : Model Binding.........
        //public IActionResult Index([Bind(nameof(Person.Email), nameof(Person.Password), nameof(Person.ConfirmPassword))] Person person)
        // Below is Custom Model Binding: 
        //public IActionResult Index([FromBody] [ModelBinder(BinderType = typeof(PersonModelBinder))] Person person)
        // Below uses PersonModelBinder provider 
        public IActionResult Index(Person person, [FromHeader(Name = "User-Agent")] string userAgent)
        {
            if (!ModelState.IsValid )
            {

                string errors = string.Join("\n", ModelState.Values.SelectMany(value => value.Errors).Select(err => err.ErrorMessage));
                return BadRequest(errors);
            }

            return Content($"{person}, {userAgent}");
        }
    }
}
