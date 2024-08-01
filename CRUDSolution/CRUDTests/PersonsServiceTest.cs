using System;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Entities;
using Services;
using Xunit.Sdk;
using Xunit.Abstractions;

namespace CRUDTests
{
    public class PersonsServiceTest
    {
        private readonly IPersonsService _personsService;
        private readonly ICountriesService _countriesService;
        private readonly ITestOutputHelper _testOutputHelper;

        public PersonsServiceTest(ITestOutputHelper testOutputHelper)
        {
            _personsService = new PersonsService(false);
            _countriesService = new CountriesService(false);
            _testOutputHelper = testOutputHelper;
        }

        #region AddPerson
        // When we supply null value as PersonAddRequest it should throw ArgumentNullException
        [Fact]
        public void AddPerson_NullPerson()
        {
            // Arrange
            PersonAddRequest? personAddRequest = null;

            // Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                // Act
                _personsService.AddPerson(personAddRequest);
            });
        }

        // When we supply null value as PersonName, it should throw ArgumentException
        [Fact]
        public void AddPerson_PersonNameIsNull()
        {
            // Arrange
            PersonAddRequest? personAddRequest = new PersonAddRequest() { PersonName = null };

            // Assert
            Assert.Throws<ArgumentException>(() =>
            {
                // Act
                _personsService.AddPerson(personAddRequest);
            });
        }

        // When we supply proper person details, it should insert the person into the perons list; 
        // and it should return an object of PersonResponse, which includes with the newly generated personID
        [Fact]
        public void AddPerson_ProperPersonDetails()
        {
            // Arrange
            PersonAddRequest? personAddRequest = new PersonAddRequest()
            {
                PersonName = "Person name",
                Email = "person@example.com",
                Address = "Adress 1, example road",
                CountryID = Guid.NewGuid(),
                Gender = GenderOptions.Male,
                DateOfBirth = DateTime.Parse("2000-01-01"),
                ReceiveNewsLetters = true
            };

            // Act
            PersonResponse person_response_from_add = _personsService.AddPerson(personAddRequest);
            List<PersonResponse> persons_list = _personsService.GetAllPersons();

            // Assert
            Assert.True(person_response_from_add.PersonID != Guid.Empty);

            Assert.Contains(person_response_from_add, persons_list);
        }
        #endregion
        
        #region GetPersonByPersonID
        // If we supply null as PersonID, it should return null as PersonResponse
        [Fact]
        public void GetPersonByPersonID_NullPersonID()
        {
            // Arrange 
            Guid? personID = null;

            // Act
            PersonResponse? person_response_from_get = _personsService.GetPersonByPersonID(personID);

            // Assert
            Assert.Null(person_response_from_get);
        }

        // If we supply a valid person id, it should return the valid person details as PersonResponse object
        [Fact]
        public void GetPersonByPersonID_WithPersonID()
        {
            // Arrange 
            CountryAddRequest country_request = new CountryAddRequest()
            {
                CountryName = "Canada"
            };

            CountryResponse country_response = _countriesService.AddCountry(country_request);

            PersonAddRequest person_request = new PersonAddRequest()
            {
                PersonName = "Person name ...",
                Email = "email@example.com",
                Address = "address",
                CountryID = country_response.CountryId,
                DateOfBirth = DateTime.Parse("2000-01-01"),
                Gender = GenderOptions.Male,
                ReceiveNewsLetters = true
            };

            // Act

            PersonResponse person_response_from_add = _personsService.AddPerson(person_request);
            PersonResponse? person_response_from_get = _personsService.GetPersonByPersonID(person_response_from_add.PersonID);

            // Assert
            Assert.Equal(person_response_from_add, person_response_from_get);
        }
        #endregion

        #region GetAllPersons
        [Fact]
        public void GetAllPersons_EmptyList()
        {
            // Act 
            List<PersonResponse> persons_from_get = _personsService.GetAllPersons();

            // Assert 
            Assert.Empty(persons_from_get);
        }

        // First, we will add few persons; and then when we call GetAllPersons(), it should return the same persons that were added
        [Fact]
        public void GetAllPersons_AddFewPersons()
        {
            // Arrange 
            CountryAddRequest country_request_1 = new CountryAddRequest()
            {
                CountryName = "Canada"
            };

            CountryAddRequest country_request_2 = new CountryAddRequest()
            {
                CountryName = "USA"
            };

            CountryResponse country_response_1 = _countriesService.AddCountry(country_request_1);
            CountryResponse country_response_2 = _countriesService.AddCountry(country_request_2);


            List<PersonAddRequest> person_add_list = new List<PersonAddRequest>() {
             new PersonAddRequest()
            {
                PersonName = "Person name ...",
                Email = "email@example.com",
                Address = "address",
                CountryID = country_response_1.CountryId,
                DateOfBirth = DateTime.Parse("2000-01-01"),
                Gender = GenderOptions.Male,
                ReceiveNewsLetters = true
            },
                new PersonAddRequest()
            {
                PersonName = "Person name 1 ...",
                Email = "email1@example.com",
                Address = "address 1",
                CountryID = country_response_2.CountryId,
                DateOfBirth = DateTime.Parse("2001-01-01"),
                Gender = GenderOptions.Female,
                ReceiveNewsLetters = false
            },
                new PersonAddRequest()
            {
                PersonName = "Person name 2 ...",
                Email = "email2@example.com",
                Address = "address 2",
                CountryID = country_response_1.CountryId,
                DateOfBirth = DateTime.Parse("2002-01-01"),
                Gender = GenderOptions.Other,
                ReceiveNewsLetters = true
            }
        };

            List<PersonResponse> person_response_list_from_add = new List<PersonResponse>();

            foreach(PersonAddRequest person_add_request in person_add_list)
            {
                person_response_list_from_add.Add(_personsService.AddPerson(person_add_request));
            }

            // Print person_response_list_from_sort
            _testOutputHelper.WriteLine("Expected:");
            foreach (PersonResponse person_response_from_add in person_response_list_from_add)
            {
                _testOutputHelper.WriteLine(person_response_from_add.ToString());
            }


            // Act
            List<PersonResponse> person_response_list_from_get = _personsService.GetAllPersons();

            // Print person_response_list_from_sort
            _testOutputHelper.WriteLine("\nActual:");
            foreach(PersonResponse person_response_from_get in person_response_list_from_get)
            {
                _testOutputHelper.WriteLine(person_response_from_get.ToString());
            }

            // Assert 
            foreach (PersonResponse person_response in person_response_list_from_add)
            {
                Assert.Contains(person_response, person_response_list_from_get);
            }
        }
        #endregion

        #region GetFilteredPersons
        // If the search text is empty and search by is "PersonName", it should return all persons
        [Fact]
        public void GetFilteredPersons_EmptySearchText()
        {
            // Arrange 
            CountryAddRequest country_request_1 = new CountryAddRequest()
            {
                CountryName = "Canada"
            };

            CountryAddRequest country_request_2 = new CountryAddRequest()
            {
                CountryName = "USA"
            };

            CountryResponse country_response_1 = _countriesService.AddCountry(country_request_1);
            CountryResponse country_response_2 = _countriesService.AddCountry(country_request_2);


            List<PersonAddRequest> person_add_list = new List<PersonAddRequest>() {
             new PersonAddRequest()
            {
                PersonName = "Person name ...",
                Email = "email@example.com",
                Address = "address",
                CountryID = country_response_1.CountryId,
                DateOfBirth = DateTime.Parse("2000-01-01"),
                Gender = GenderOptions.Male,
                ReceiveNewsLetters = true
            },
                new PersonAddRequest()
            {
                PersonName = "Person name 1 ...",
                Email = "email1@example.com",
                Address = "address 1",
                CountryID = country_response_2.CountryId,
                DateOfBirth = DateTime.Parse("2001-01-01"),
                Gender = GenderOptions.Female,
                ReceiveNewsLetters = false
            },
                new PersonAddRequest()
            {
                PersonName = "Person name 2 ...",
                Email = "email2@example.com",
                Address = "address 2",
                CountryID = country_response_1.CountryId,
                DateOfBirth = DateTime.Parse("2002-01-01"),
                Gender = GenderOptions.Other,
                ReceiveNewsLetters = true
            }
        };

            List<PersonResponse> person_response_list_from_add = new List<PersonResponse>();

            foreach (PersonAddRequest person_add_request in person_add_list)
            {
                person_response_list_from_add.Add(_personsService.AddPerson(person_add_request));
            }

            // Print person_response_list_from_add
            _testOutputHelper.WriteLine("Expected:");
            foreach (PersonResponse person_response_from_add in person_response_list_from_add)
            {
                _testOutputHelper.WriteLine(person_response_from_add.ToString());
            }


            // Act
            List<PersonResponse> person_response_list_from_search = _personsService.GetFilteredPersons(nameof(Person.PersonName), "");

            // Print person_response_list_from_sort
            _testOutputHelper.WriteLine("\nActual:");
            foreach (PersonResponse person_response_from_search in person_response_list_from_search)
            {
                _testOutputHelper.WriteLine(person_response_from_search.ToString());
            }

            // Assert 
            foreach (PersonResponse person_response in person_response_list_from_add)
            {
                Assert.Contains(person_response, person_response_list_from_search);
            }
        }

        // First we will add few person; and then we will search based on person name with some search string.
        // It should return the matching persons
        [Fact]
        public void GetFilteredPersons_SearchByPersonName()
        {
            // Arrange 
            CountryAddRequest country_request_1 = new CountryAddRequest()
            {
                CountryName = "Canada"
            };

            CountryAddRequest country_request_2 = new CountryAddRequest()
            {
                CountryName = "USA"
            };

            CountryResponse country_response_1 = _countriesService.AddCountry(country_request_1);
            CountryResponse country_response_2 = _countriesService.AddCountry(country_request_2);


            List<PersonAddRequest> person_add_list = new List<PersonAddRequest>() {
             new PersonAddRequest()
            {
                PersonName = "Smith",
                Email = "smith@example.com",
                Address = "address",
                CountryID = country_response_1.CountryId,
                DateOfBirth = DateTime.Parse("2000-01-01"),
                Gender = GenderOptions.Male,
                ReceiveNewsLetters = true
            },
                new PersonAddRequest()
            {
                PersonName = "Mary",
                Email = "mary@example.com",
                Address = "address 1",
                CountryID = country_response_2.CountryId,
                DateOfBirth = DateTime.Parse("2001-01-01"),
                Gender = GenderOptions.Female,
                ReceiveNewsLetters = false
            },
                new PersonAddRequest()
            {
                PersonName = "Shawn",
                Email = "shawn@example.com",
                Address = "address 2",
                CountryID = country_response_1.CountryId,
                DateOfBirth = DateTime.Parse("2002-01-01"),
                Gender = GenderOptions.Other,
                ReceiveNewsLetters = true
            },
                new PersonAddRequest()
            {
                PersonName = "Rahman",
                Email = "rahman@example.com",
                Address = "address of rahman",
                CountryID = country_response_1.CountryId,
                DateOfBirth = DateTime.Parse("2002-01-01"),
                Gender = GenderOptions.Other,
                ReceiveNewsLetters = true
            },

        };

            List<PersonResponse> person_response_list_from_add = new List<PersonResponse>();

            foreach (PersonAddRequest person_add_request in person_add_list)
            {
                person_response_list_from_add.Add(_personsService.AddPerson(person_add_request));
            }

            // Print person_response_list_from_add
            _testOutputHelper.WriteLine("Expected:");
            foreach (PersonResponse person_response_from_add in person_response_list_from_add)
            {
                _testOutputHelper.WriteLine(person_response_from_add.ToString());
            }


            // Act
            List<PersonResponse> person_response_list_from_search = _personsService.GetFilteredPersons(nameof(Person.PersonName), "ma");

            // Print person_response_list_from_sort
            _testOutputHelper.WriteLine("\nActual:");
            foreach (PersonResponse person_response_from_search in person_response_list_from_search)
            {
                _testOutputHelper.WriteLine(person_response_from_search.ToString());
            }

            // Assert 
            Assert.Equal(2, person_response_list_from_search.Count);

            foreach (PersonResponse person_response in person_response_list_from_add)
            {
                if(person_response.PersonName != null)
                {
                    if (person_response.PersonName.Contains("ma", StringComparison.OrdinalIgnoreCase))
                    {
                        Assert.Contains(person_response, person_response_list_from_search);
                    }
                }
            }
        }
        #endregion

        #region GetSortedPersons
        // When we sort based on PersonName in DESC, it should return persons list in descending by PersonName
        [Fact]
        public void GetSortedPersons()
        {
            // Arrange 
            CountryAddRequest country_request_1 = new CountryAddRequest()
            {
                CountryName = "Canada"
            };

            CountryAddRequest country_request_2 = new CountryAddRequest()
            {
                CountryName = "USA"
            };

            CountryResponse country_response_1 = _countriesService.AddCountry(country_request_1);
            CountryResponse country_response_2 = _countriesService.AddCountry(country_request_2);


            List<PersonAddRequest> person_add_list = new List<PersonAddRequest>() {
             new PersonAddRequest()
            {
                PersonName = "Smith",
                Email = "smith@example.com",
                Address = "address",
                CountryID = country_response_1.CountryId,
                DateOfBirth = DateTime.Parse("2000-01-01"),
                Gender = GenderOptions.Male,
                ReceiveNewsLetters = true
            },
                new PersonAddRequest()
            {
                PersonName = "Mary",
                Email = "mary@example.com",
                Address = "address 1",
                CountryID = country_response_2.CountryId,
                DateOfBirth = DateTime.Parse("2001-01-01"),
                Gender = GenderOptions.Female,
                ReceiveNewsLetters = false
            },
                new PersonAddRequest()
            {
                PersonName = "Shawn",
                Email = "shawn@example.com",
                Address = "address 2",
                CountryID = country_response_1.CountryId,
                DateOfBirth = DateTime.Parse("2002-01-01"),
                Gender = GenderOptions.Other,
                ReceiveNewsLetters = true
            },
                new PersonAddRequest()
            {
                PersonName = "Rahman",
                Email = "rahman@example.com",
                Address = "address of rahman",
                CountryID = country_response_1.CountryId,
                DateOfBirth = DateTime.Parse("2002-01-01"),
                Gender = GenderOptions.Other,
                ReceiveNewsLetters = true
            },

        };

            List<PersonResponse> person_response_list_from_add = new List<PersonResponse>();

            foreach (PersonAddRequest person_add_request in person_add_list)
            {
                person_response_list_from_add.Add(_personsService.AddPerson(person_add_request));
            }

            person_response_list_from_add = person_response_list_from_add.OrderByDescending(p => p.PersonName).ToList();

            // Print person_response_list_from_add
            _testOutputHelper.WriteLine("Expected:");
            foreach (PersonResponse person_response_from_add in person_response_list_from_add)
            {
                _testOutputHelper.WriteLine(person_response_from_add.ToString());
            }

            List<PersonResponse> allPersons = _personsService.GetAllPersons();

            // Act
            List<PersonResponse> person_response_list_from_sort = _personsService.GetSortedPersons(allPersons, nameof(Person.PersonName), SortOrderOptions.DESC);

            // Print person_response_list_from_sort
            _testOutputHelper.WriteLine("\nActual:");
            foreach (PersonResponse person_response_from_sort in person_response_list_from_sort)
            {
                _testOutputHelper.WriteLine(person_response_from_sort.ToString());
            }


            // Assert 
            for(int i = 0; i < person_response_list_from_add.Count; i++)
            {
                Assert.Equal(person_response_list_from_add[i], person_response_list_from_sort[i]);
            }    
        }
        #endregion

        #region UpdatePerson
        // When we supply null as PersonUpdateRequest, it should throw ArgumentNullException
        [Fact]
        public void UpdatePerson_NullPerson()
        {
            // Arrange
            PersonUpdateRequest? person_update_request = null;

            // Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                // Act
                _personsService.UpdatePerson(person_update_request);
            });
            
        }

        // When we supply invalid person id, it should throw ArgumentException
        [Fact]
        public void UpdatePerson_InvalidPersonID()
        {
            // Arrange
            PersonUpdateRequest? person_update_request = new PersonUpdateRequest() { PersonID = Guid.NewGuid()};

            // Assert
            Assert.Throws<ArgumentException>(() =>
            {
                // Act
                _personsService.UpdatePerson(person_update_request);
            });

        }

        // When the PersonName is null, it should throw ArgumentException
        [Fact]
        public void UpdatePerson_PersonNameIsNull()
        {
            // Arrange
            CountryAddRequest country_add_request = new CountryAddRequest() { CountryName = "UK" };
            CountryResponse country_response_from_add = _countriesService.AddCountry(country_add_request);

            PersonAddRequest person_add_request = new PersonAddRequest()
            {
                PersonName = "Person name",
                Email = "person@example.com",
                Address = "Adress 1, example road",
                CountryID = country_response_from_add.CountryId,
                Gender = GenderOptions.Male,
                DateOfBirth = DateTime.Parse("2000-01-01"),
                ReceiveNewsLetters = true
            };

            PersonResponse person_response_from_add = _personsService.AddPerson(person_add_request);

            PersonUpdateRequest? person_update_request = person_response_from_add.ToPersonUpdateRequest();
            person_update_request.PersonName = null;

            // Assert
            Assert.Throws<ArgumentException>(() =>
            {
                // Act
                _personsService.UpdatePerson(person_update_request);
            });
        }

        // First, add a new person and try to update the person name and email
        [Fact]
        public void UpdatePerson_PersonFullDetailsUpdation()
        {
            // Arrange
            CountryAddRequest country_add_request = new CountryAddRequest() { CountryName = "UK" };
            CountryResponse country_response_from_add = _countriesService.AddCountry(country_add_request);

            PersonAddRequest person_add_request = new PersonAddRequest()
            {
                PersonName = "John",
                Email = "john@example.com",
                Address = "Adress 1, example road",
                CountryID = country_response_from_add.CountryId,
                Gender = GenderOptions.Male,
                DateOfBirth = DateTime.Parse("2000-01-01"),
                ReceiveNewsLetters = true
            };

            PersonResponse person_response_from_add = _personsService.AddPerson(person_add_request);

            PersonUpdateRequest? person_update_request = person_response_from_add.ToPersonUpdateRequest();
            person_update_request.PersonName = "William";
            person_update_request.Email = "william@example.com";

            // Act
            PersonResponse? person_response_from_update = _personsService.UpdatePerson(person_update_request);
            PersonResponse? person_response_from_get= _personsService.GetPersonByPersonID(person_response_from_update?.PersonID);

            // Assert
            Assert.Equal(person_response_from_update, person_response_from_get);
        }
        #endregion

        #region DeletePerson
        // If you supply an valid PersonID, it should return true
        [Fact]
        public void DeletePerson_ValidPersonID()
        {
            // Arrange
            CountryAddRequest country_add_request = new CountryAddRequest() { CountryName = "UK" };
            CountryResponse country_response_from_add = _countriesService.AddCountry(country_add_request);

            PersonAddRequest person_add_request = new PersonAddRequest()
            {
                PersonName = "Person name",
                Email = "person@example.com",
                Address = "Adress 1, example road",
                CountryID = country_response_from_add.CountryId,
                Gender = GenderOptions.Male,
                DateOfBirth = DateTime.Parse("2000-01-01"),
                ReceiveNewsLetters = true
            };

            PersonResponse person_response_from_add = _personsService.AddPerson(person_add_request);

            // Act
            bool isDeleted = _personsService.DeletePerson(person_response_from_add.PersonID);

            // Assert
            Assert.True(isDeleted);

        }

        // If you supply an invalid PersonID, it should return false
        [Fact]
        public void DeletePerson_InvalidPersonID()
        {
            // Act
            bool isDeleted = _personsService.DeletePerson(Guid.NewGuid());

            // Assert
            Assert.False(isDeleted);

        }
        #endregion

    }
}
