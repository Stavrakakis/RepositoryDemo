namespace DAL
{
    using System;
    using System.Data.Common;
    using System.Data.Entity;
    using System.Linq;
    using System.Linq.Expressions;

    public class DataContext : DbContext
    {
        public DataContext()
        { 

        }

        // Your context has been configured to use a 'DataContext' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'DAL.DataContext' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'DataContext' 
        // connection string in the application configuration file.
        public DataContext(DbConnection connection)
            : base(connection, true)
        {
        }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        public virtual DbSet<User> Users { get; set; }
    }

    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }

        public static Expression<Func<User, bool>> NameBeginsWith(string prefix) 
        {
            return u => u.Name.StartsWith(prefix);
        }

        public static Expression<Func<User, bool>> AgeBetween(int min, int max)
        {
            return u => (min <= u.Age) && (u.Age <= max);
        }
    }
}