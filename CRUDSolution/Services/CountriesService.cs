using ServiceContracts;
using ServiceContracts.DTO;
using Entities;

namespace Services
{
    public class CountriesService : ICountriesService
    {
        private readonly PersonsDbContext _db;

        public CountriesService(PersonsDbContext personsDbContext)
        {
            _db = personsDbContext;
        }
        public CountryResponse AddCountry(CountryAddRequest? countryAddRequest)
        {
            //Validation 
            if (countryAddRequest == null)
            {
                throw new ArgumentNullException(nameof(countryAddRequest));
            }

            if (countryAddRequest.CountryName == null)
            {
                throw new ArgumentNullException(nameof(countryAddRequest.CountryName));
            }

            // Validation : CountryName can't be duplicated
            if (_db.Countries.Count(country => country.CountryName == countryAddRequest.CountryName) > 0)
            {
                throw new ArgumentNullException("Given country name already exists");
            }

            // Convert object from CountryAddRequest to Country type
            Country country = countryAddRequest.ToCountry();

            //generate CountriID
            country.CountryID = Guid.NewGuid();

            // Add country object into _countries
            _db.Countries.Add(country);
            _db.SaveChanges();

            return country.ToCountryResponse();
        }

        public List<CountryResponse> GetAllCountries()
        {
            return _db.Countries.Select(country => country.ToCountryResponse()).ToList();
        }

        public CountryResponse? GetCountryByCountryID(Guid? countryId)
        {
            if (countryId == null)
            {
                return null;
            }

            Country? country = _db.Countries.FirstOrDefault((country) => country.CountryID == countryId);

            if (country == null) return null;

            return country.ToCountryResponse();
        }
    }
}