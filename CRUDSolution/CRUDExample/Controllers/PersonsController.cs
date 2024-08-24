using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace CRUDExample.Controllers
{
    [Route("persons")]
    public class PersonsController : Controller
    {
        private readonly IPersonsService _personsService;
        private readonly ICountriesService _countriesService;

        public PersonsController(IPersonsService personsService, ICountriesService countriesService)
        {
            _personsService = personsService;
            _countriesService = countriesService;
        }


        [Route("index")]
        [Route("/")]
        public IActionResult Index(string searchBy, string? searchString, string sortBy = nameof(PersonResponse.PersonName), SortOrderOptions sortOrder = SortOrderOptions.ASC)
        {
            // Search
            ViewBag.SearchFields = new Dictionary<string, string>() {
                { nameof(PersonResponse.PersonName), "Person Name" },
                { nameof(PersonResponse.Email), "Email" },
                { nameof(PersonResponse.DateOfBirth), "Date of Birth" },
                { nameof(PersonResponse.Gender), "Gender" },
                { nameof(PersonResponse.CountryID), "Country" },
                { nameof(PersonResponse.Address), "Address" },
            };
            List<PersonResponse> persons= _personsService.GetFilteredPersons(searchBy, searchString);
            ViewBag.CurrentSearchBy = searchBy;
            ViewBag.CurrentSearchString = searchString;

            // Sort
            List<PersonResponse> sortedPersons = _personsService.GetSortedPersons(persons, sortBy, sortOrder);
            ViewBag.CurrentSortBy = sortBy;
            ViewBag.CurrentSortOrder = sortOrder.ToString();

            return View(sortedPersons); // Views/Persons/Index.cshtml
        }

        [Route("[action]")]
        [HttpGet]
        public IActionResult Create() {
            List<CountryResponse> countries = _countriesService.GetAllCountries();
            ViewBag.Countries = countries.Select(c => new SelectListItem() { 
                Text = c.CountryName,
                Value = c.CountryId.ToString()
            });
            return View();
        }

        [HttpPost]
        [Route("create")]
        public IActionResult Create(PersonAddRequest personAddRequest)
        {
           if(!ModelState.IsValid)
            {
                List<CountryResponse> countries = _countriesService.GetAllCountries();
                ViewBag.Countries = countries;
                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return View();
            }

           // Call the service method
            _personsService.AddPerson(personAddRequest);

            // navigate to Index() action method (it makes another get request to "persons/index")
            return RedirectToAction("Index", "Persons");
        }

        [HttpGet]
        [Route("[action]/{personID}")] // Eg: /persons/edit/1
        public IActionResult Edit(Guid personID)
        {
            PersonResponse? personResponse = _personsService.GetPersonByPersonID(personID);
            if(personResponse == null)
            {
                return RedirectToAction("Index");
            }

            PersonUpdateRequest personUpdateRequest = personResponse.ToPersonUpdateRequest();
            List<CountryResponse> countries = _countriesService.GetAllCountries();
            ViewBag.Countries = countries.Select(c => new SelectListItem()
            {
                Text = c.CountryName,
                Value = c.CountryId.ToString()
            });

            return View(personUpdateRequest);
        }

        [HttpPost]
        [Route("[action]/{personID}")]
        public IActionResult Edit(PersonUpdateRequest personUpdateRequest) 
        { 
           PersonResponse personResponse = _personsService.GetPersonByPersonID(personUpdateRequest.PersonID);
            if(personResponse == null)
            {
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                _personsService.UpdatePerson(personUpdateRequest);
                return RedirectToAction("Index");
            } else
            {
                List<CountryResponse> countries = _countriesService.GetAllCountries();
                ViewBag.Countries = countries;
                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return View();
            }
        }
    }
}
