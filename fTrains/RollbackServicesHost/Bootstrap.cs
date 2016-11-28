using Autofac;
using Domain;
using DomainModel;
using IContainer = Autofac.IContainer;

namespace RollbackServicesHost
{
    public class Bootstrap
    {
        public static IContainer BuildContainer()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<RollbackedUnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(Repository<>)).As(typeof(IRepository<>)).InstancePerLifetimeScope();
            
            Container = builder.Build();
            return Container;
        }

        public static IContainer Container;
    }
}
