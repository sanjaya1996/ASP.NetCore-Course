using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ConfigurationExample.Controllers
{
    public class HomeController : Controller
    {

       /* // IConfiguation only 
        private readonly IConfiguration _configuration;
        public HomeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }*/


        private readonly WEatherApiOptions _options;

        public HomeController(IOptions<WEatherApiOptions> weatherApiOptions)
        {
            _options = weatherApiOptions.Value;
        }

        [Route("/")]
        public IActionResult Index()
        {

            // ************* With Just IConfiguration ***********************
            /*ViewBag.ClientID = _configuration["WeatherAPI:ClientId"];
            ViewBag.ClientSecret = _configuration.GetValue("WeatherAPI:ClientSecret", "The default client secret");
            return View();*/

            // WEatherApiOptions options = _configuration.GetSection("WeatherAPI").Get<WEatherApiOptions>();

            // Bind: Loads configuration values into existing Options object
             
            /*WEatherApiOptions options = new WEatherApiOptions();
            _configuration.GetSection("WeatherAPI").Bind(options);*/

            ViewBag.ClientID = _options.ClientID;
            ViewBag.ClientSecret = _options.ClientSecret;
            return View();
        }
    }
}
