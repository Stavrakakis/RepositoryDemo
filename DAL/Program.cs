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
                    .ForMember(u => u.Address, c => c.MapFrom(dto => dto.Address))
                    .ForMember(u => u.Id, c => c.MapFrom(dto => dto.Id));

                cfg.CreateMap<Address, AddressDto>()
                    .ForMember(a => a.Country, c => c.MapFrom(dto => dto.Country))
                    .ForMember(a => a.Street, c => c.MapFrom(dto => dto.Street))
                    .ForMember(a => a.Id, c => c.MapFrom(dto => dto.Id))
                    .ForMember(a => a.Users, c => c.Ignore());
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
