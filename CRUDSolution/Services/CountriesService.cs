using ServiceContracts;
using ServiceContracts.DTO;
using Entities;

namespace Services
{
    public class CountriesService : ICountriesService
    {
        private readonly List<Country> _countries;

        public CountriesService()
        {
            _countries = new List<Country>();
        }
        public CountryResponse AddCountry(CountryAddRequest? countryAddRequest)
        {
            //Validation 
            if(countryAddRequest == null)
            {
                throw new ArgumentNullException(nameof(countryAddRequest));
            }

            if(countryAddRequest.CountryName == null)
            {
                throw new ArgumentNullException(nameof(countryAddRequest.CountryName));
            }

            // Validation : CountryName can't be duplicated
            if (_countries.Where(country => country.CountryName == countryAddRequest.CountryName).Any())
            {
                throw new ArgumentNullException("Given country name already exists");
            }

            // Convert object from CountryAddRequest to Country type
           Country country = countryAddRequest.ToCountry();
            
            //generate CountriID
            country.CountryID = Guid.NewGuid();

            // Add country object into _countries
            _countries.Add(country);

            return country.ToCountryResponse();
        }

        public List<CountryResponse> GetAllCountries()
        {
            return _countries.Select(country => country.ToCountryResponse()).ToList();
        }

        public CountryResponse? GetCountryByCountryID(Guid? countryId)
        {
            if(countryId == null)
            {
                return null;
            }

            Country? country = _countries.FirstOrDefault((country) => country.CountryID == countryId);

            if (country == null) return null;

            return country.ToCountryResponse();
        }
    }
}