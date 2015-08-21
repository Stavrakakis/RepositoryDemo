namespace DAL.Database.Dtos
{
    using System.Data.Entity.ModelConfiguration;

    public class UserDtoConfigution : EntityTypeConfiguration<UserDto>
    {
        public UserDtoConfigution()
        {
            this.HasKey(u => u.Id);
            this.HasRequired(u => u.Address);
        }
    }

    public class UserDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int AgeInYears { get; set; }

        public AddressDto Address { get; set; }
                
        public string Surname
        {
            get
            {
                return string.Empty;
            }
        }
        
        public int Age
        {
            get
            {
                return this.AgeInYears;
            }
        }
    }
}
