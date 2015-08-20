using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Database
{
    public class AddressDto
    {
        public int Id { get; set; }
        public string Street { get; set; }
        public string Country { get; set; }

        // Mapping to IAddress

        public string CountryName
        {
            get { return Country; }
        }
    }

    public class AddressDtoConfiguration : EntityTypeConfiguration<AddressDto>
    {
        public AddressDtoConfiguration()
        {
            this.HasKey(u => u.Id);
        }
    }
}
