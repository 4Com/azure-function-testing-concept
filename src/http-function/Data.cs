using http_function.Models;
using System.Collections.Generic;

namespace http_function
{
    internal static class Data
    {
        public static List<Person> People
        {
            get
            {
                return new List<Person>
                {
                    new Person{Id = 1, Name = "John", Address = "19828 Valerio St, Winnetka, CA 91306, USA", PhoneNumber = "888-519-1991"},
                    new Person{Id = 2,Name = "Miles", Address = "30065 CA-1, Malibu, CA 90265, USA", PhoneNumber = "888-584-2019"},
                    new Person{Id = 3,Name = "Peter", Address = "11600 Eldridge Ave, Sylmar, CA 91342, USA", PhoneNumber = "888-555-2003"},
                    new Person{Id = 4,Name = "Sarah", Address = "14239 Gilmore St, Van Nuys, CA 91401, USA", PhoneNumber = "888-512-1984"}
                };
            }
        }
    }
}