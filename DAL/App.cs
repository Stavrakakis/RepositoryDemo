namespace DAL
{
    using DAL.Domain.Criteria;
    using DAL.Domain.Repositories;
    using DAL.Services;

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
            // Allow criteria approach where builder pattern is used
            // by service consumers to choose their data
            var criteria = new UserCriteria().ById(1).ById(2);
            var users = this.userService.GetUsers(criteria);

            // Have service expose specific methods for retrieval
        }
    }
}
