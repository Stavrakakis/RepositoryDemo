using Autofac;
using LinqKit;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.EntityClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    class Program
    {
        static void Main(string[] args)
        {

            var builder = new ContainerBuilder();
            
            builder.RegisterType<UnitOfWorkFactory>().As<IUnitOfWorkFactory>();
            builder.RegisterType<App>().As<App>();

            var container = builder.Build();

            using (var scope = container.BeginLifetimeScope())
            {
                var app = scope.Resolve<App>();

                app.Run();
            }
        }
    }

    public class App 
    {
        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        public App(IUnitOfWorkFactory factory) 
        {
            this.unitOfWorkFactory = factory;
        }

        public void Run() 
        {
            using (var session = this.unitOfWorkFactory.Create())
            {
                var repo = session.UserRepository;

                repo.Insert(new User { Id = 1, Name = "Nico", Age = 29 });
                repo.Insert(new User { Id = 2, Name = "Bob", Age = 33 });
                repo.Insert(new User { Id = 2, Name = "Brian", Age = 45 });

                session.Save();

                var nameBeginsWithB = User.NameBeginsWith("B");
                var inTheirTwenties = User.AgeBetween(20, 29);
                var inTheirFourties = User.AgeBetween(40, 49);

                var query = Filter.And(nameBeginsWithB, Filter.Or(inTheirTwenties, inTheirFourties));

                var entities = repo.GetAll(query);
            }
        }
    }
}
