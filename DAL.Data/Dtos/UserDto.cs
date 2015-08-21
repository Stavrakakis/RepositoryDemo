namespace DAL.Database.Dtos
{
    using Domain.Entities;
    using System.Data.Entity.ModelConfiguration;

    public class UserDtoConfigution : EntityTypeConfiguration<UserDto>
    {
        public UserDtoConfigution()
        {
            this.ToTable("Users");
            this.HasKey(u => u.Id);
            this.HasRequired(u => u.Address);
        }
    }

    public class UserDto : IMapTo<User>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int AgeInYears { get; set; }

        public AddressDto Address { get; set; }

        public User Map()
        {
            return new User(this.Id, this.Name, string.Empty, this.AgeInYears);
        }

        //public AddressDto Address { get; set; }
    }

    public interface IMapTo<TEntity>
    {
        TEntity Map();
    }
}
