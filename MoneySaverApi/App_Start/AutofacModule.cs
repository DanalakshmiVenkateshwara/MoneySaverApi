using Autofac;
using BusinessManagers.Interfaces;
using DataAccess.Repositories.Interfaces;
using DataAccess.Repositories;
using DataAccess;
using BusinessManagers.Managers;

namespace MoneySaverApi.App_Start
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<ConnectionFactory>().As<IConnectionFactory>();
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();
            builder.RegisterType<UserManager>().As<IUserManager>();
            builder.RegisterType<RepositoryBase>().As<IRepository>();
            builder.RegisterType<UserRepository>().As<IUserRepository>();
        }
    }
}
