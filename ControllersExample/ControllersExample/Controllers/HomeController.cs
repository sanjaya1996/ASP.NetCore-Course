using Microsoft.AspNetCore.Mvc;
using ControllersExample.Models;

namespace ControllersExample.Controllers
{
    public class HomeController : Controller
    {
        [Route("home")]
        [Route("/")]
        public ContentResult Index()
        {
            //return new ContentResult()
            //{
            //    Content = "Hello from Home",
            //    ContentType = "text/plain",
            //    StatusCode = 200
            //};

            return Content("<h1>Welcome</h1> <h2>Hello from Home</h2>", "text/html");
        }

        [Route("person")]
        public JsonResult Person()
        {
            Person person = new Person()
            {
                Id = Guid.NewGuid(),
                FirsName = "Sanjay",
                LastName = "Dahal",
                Age = 27
            };

            //return new JsonResult(person);
            return Json(person);
        }

        [Route("file-download")]
        public VirtualFileResult DownloadFile()
        {
            // return new VirtualFileResult("/Nominations.pdf", "application/pdf");
             return File("/Nominations.pdf", "application/pdf");
        }

        [Route("file-download2")]
        public PhysicalFileResult DownloadFile2()
        {
            // return new PhysicalFileResult("S:\\software development\\practice\\aspnetcore\\Nominations.pdf", "application/pdf");
            return PhysicalFile("S:\\software development\\practice\\aspnetcore\\Nominations.pdf", "application/pdf");

        }

        [Route("file-download3")]
        public FileContentResult DownloadFile3()
        {
           byte[] bytes = System.IO.File.ReadAllBytes("S:\\software development\\practice\\aspnetcore\\Nominations.pdf");
            // return new FileContentResult(bytes, "application/pdf");
            return File(bytes, "application/pdf");
        }
    }
}
