namespace DAL.Database.Dtos
{
    using System.Data.Entity.ModelConfiguration;

    public class AddressDto
    {
        public int Id { get; set; }

        public string Street { get; set; }

        public string Country { get; set; }
    }

    public class AddressDtoConfiguration : EntityTypeConfiguration<AddressDto>
    {
        public AddressDtoConfiguration()
        {
            this.HasKey(u => u.Id);
        }
    }
}
