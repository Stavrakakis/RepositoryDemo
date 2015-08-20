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
using DAL.UnitOfWork;
using DAL.Services;
using DAL.Database;
using DAL.Domain;
using DAL.Criteria;

namespace DAL
{
    class Program
    {
        static void Main(string[] args)
        {

            var builder = new ContainerBuilder();

            AutoMapper.Mapper.CreateMap<User, UserDto>()
                .ForMember(u => u.AgeInYears, c => c.MapFrom(dto => dto.Age))
                .ForMember(u => u.Name, c => c.MapFrom(dto => dto.FirstName)); 

            AutoMapper.Mapper.CreateMap<UserDto, User>();

            builder.RegisterType<UnitOfWorkFactory>().As<IUnitOfWorkFactory>();
            builder.RegisterType<UserService>().As<IUserService>();
            builder.RegisterGeneric(typeof(GenericRepository<,>)).As(typeof(IRepository<>));

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
        private readonly IUserService userService;

        public App(IUnitOfWorkFactory factory, IUserService userService) 
        {
            this.unitOfWorkFactory = factory;
            this.userService = userService;
        }

        public void Run()
        {
            using (var session = this.unitOfWorkFactory.Create())
            {
                var criteria = new UserCriteria().ById(1).ById(2);

                var british = this.userService.GetUsers(criteria);
            }

        }
    }
}
