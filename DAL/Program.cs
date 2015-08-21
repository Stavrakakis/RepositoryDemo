namespace DAL
{
    using Autofac;
    using Services;
    using Domain.Repositories;
    using Domain.Criteria;
    using Database.Dtos;
    using Domain.Entities;
    using Data.Repositories;
    using Data.UnitOfWork;
    using Mapping;

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
            builder.RegisterType<Mapper>().As<IMapper>();
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
}
