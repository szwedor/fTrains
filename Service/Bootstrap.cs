using Autofac;
using Domain;
using DomainModel;
using IContainer = Autofac.IContainer;

namespace Service
{
    public class Bootstrap
    {
        public static IContainer BuildContainer()//4
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(Repository<>)).As(typeof(IRepository<>)).InstancePerLifetimeScope();
            
            Container = builder.Build();
            return Container;
        }

        public static IContainer Container;
    }
}
