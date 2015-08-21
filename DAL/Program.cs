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

            builder.RegisterType<UnitOfWorkFactory>().As<IUnitOfWorkFactory>();
            builder.RegisterType<UserService>().As<IUserService>();
            builder.RegisterType<Mapper>().As<IMapper>();
            builder.RegisterGeneric(typeof(GenericRepository<,>)).As(typeof(IRepository<>));

            builder.RegisterType<App>().As<App>();

            var container = builder.Build();

            AutoMapper.Mapper.Initialize(cfg => {

                cfg.CreateMap<User, UserDto>()
                    .ForMember(u => u.AgeInYears, c => c.MapFrom(dto => dto.Age))
                    .ForMember(u => u.Name, c => c.MapFrom(dto => dto.FirstName))
                    .ForMember(u => u.Id, c => c.MapFrom(dto => dto.Id));
                
                cfg.CreateMap<UserDto, User>()
                .ForMember(u => u.Age, c => c.MapFrom(dto => dto.AgeInYears))
                .ForMember(u => u.Id, c => c.MapFrom(dto => dto.Id));
                
            });

            AutoMapper.Mapper.AssertConfigurationIsValid();

            using (var scope = container.BeginLifetimeScope())
            {
                var app = scope.Resolve<App>();

                app.Run();
            }
        }
    }
}
