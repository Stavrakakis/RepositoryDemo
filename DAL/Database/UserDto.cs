using DAL.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Database
{

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

        public static Expression<Func<UserDto, bool>> NameBeginsWith(string prefix)
        {
            return u => u.Name.StartsWith(prefix);
        }

        public static Expression<Func<UserDto, bool>> AgeBetween(int min, int max)
        {
            return u => (min <= u.Age) && (u.Age <= max);
        }

        
        public string Surname
        {
            get { return string.Empty; }
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
