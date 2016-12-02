using Autofac;
using Domain;
using DomainModel;
using IContainer = Autofac.IContainer;

namespace WcfServiceLibrary
{
    public class Bootstrap
    {
        public static IContainer BuildContainer()
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
