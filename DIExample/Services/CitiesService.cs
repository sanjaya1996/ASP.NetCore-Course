using ServiceContracts;

namespace Services
{
    public class CitiesService : ICitiesService, IDisposable
    {
        private List<string> _cities;
        private Guid _serviceInstanceId;

        public Guid ServiceInstanceId { get { return _serviceInstanceId; } }
        public CitiesService() {
            _serviceInstanceId = Guid.NewGuid();
            _cities = new List<string>()
            {
                "London", "Paris", "Sydney", "Melbourne", "Kathmandu", "Tokyo", "Rome"
            };
            // To Do: Add logic to open the db connectiom
        }

        public List<string> GetCities()
        {
            return _cities;
        }

        public void Dispose()
        {
            // To do: add logic to close db connection
        }
    }
}