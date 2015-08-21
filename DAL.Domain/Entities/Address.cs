using System.Collections.Generic;

namespace DAL.Domain.Entities
{
    public class Address
    {
        private readonly int id;
        private readonly string street;
        private readonly string country;

        public Address(string street, string country)
        {
            this.street = street;
            this.country = country;
        }

        public Address(int id, string street, string country) : this(street, country)
        {
            this.id = id;
        }

        public IEnumerable<User> Users { get; set; }

        public int Id { get { return this.id; } }

        public string Street { get { return this.street; } }

        public string Country { get { return this.country; } }
    }
}
