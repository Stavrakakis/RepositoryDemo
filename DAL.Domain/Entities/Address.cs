namespace DAL.Domain.Entities
{
    public class Address
    {
        private readonly int id;
        private readonly string street;
        private readonly string country;

        public Address(int id, string street, string country)
        {
            this.id = id;
            this.street = street;
            this.country = country;
        }

        public int Id { get { return this.id; } }

        public string Street { get { return this.street; } }

        public string Country { get { return this.country; } }
    }
}
