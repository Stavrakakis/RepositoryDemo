namespace DAL.Domain.Entities
{
    public class User
    {
        private readonly int id;
        private readonly string firstname;
        private readonly string surname;
        private readonly int age;
        private readonly Address address;

        public User() { }

        public User(int id, string firstName, string surname, int age)
        {
            this.id = id;
            this.firstname = firstName;
            this.surname = surname;
            this.age = age;
            this.address = new Address();
        }

        public int Id { get { return this.id; } }

        public string FirstName { get { return this.firstname; } }

        public string Surname { get { return this.surname; } }

        public int Age { get { return this.age; } }

        public Address Address { get { return this.address; } }
    }
}
