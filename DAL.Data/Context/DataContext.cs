namespace DAL.Data.Context
{
    using Database.Dtos;
    using System.Data.Common;
    using System.Data.Entity;

    public class DataContext : DbContext
    {
        public DataContext()
        {
            this.Configuration.LazyLoadingEnabled = false;
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

        public virtual DbSet<UserDto> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Configurations.Add(new UserDtoConfigution());
            modelBuilder.Configurations.Add(new AddressDtoConfiguration());
        }
    }
}