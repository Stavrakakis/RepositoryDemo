namespace DAL.Domain.Entities
{
    public class User
    {
        private readonly int id;
        private readonly string firstname;
        private readonly string surname;
        private readonly int age;
        private readonly Address address;

        public User(string firstname, string surname, int age, Address address)
        {
            this.address = address;
            this.firstname = firstname;
            this.surname = surname;
            this.age = age;
        }

        public User(int id, string firstName, string surname, int age)
        {
            this.id = id;
            this.firstname = firstName;
            this.surname = surname;
            this.age = age;
            //this.address = new Address(0, "", "");
        }

        public int Id { get { return this.id; } }

        public string FirstName { get { return this.firstname; } }

        public string Surname { get { return this.surname; } }

        public int Age { get { return this.age; } }

        public Address Address { get { return this.address; } }
    }
}
