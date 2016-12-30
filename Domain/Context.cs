using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Annotations;
using DomainModel.Models;

namespace Domain
{
    public class Init : DropCreateDatabaseIfModelChanges<TrainContext>
    {
        protected override void Seed(TrainContext context)
        {
            context.Configuration.AutoDetectChangesEnabled = false;
            context.Configuration.ValidateOnSaveEnabled = false;
            seed.Seed(context);
            base.Seed(context);
            context.Configuration.AutoDetectChangesEnabled = true;
            context.Configuration.ValidateOnSaveEnabled = true;


        }
    }
    public class TrainContext : DbContext
    {

        public TrainContext() : 
            base("Server=tcp:trains.database.windows.net,1433;Initial Catalog=trains;Persist Security Info=False;User ID=szwedor;Password=lollollol9!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;")
      // public TrainContext() : base("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\Trains.mdf;Integrated Security=True")
        {
            Configuration.AutoDetectChangesEnabled = false;
            Configuration.ValidateOnSaveEnabled = false;
               Database.SetInitializer<TrainContext>(new Init());
            var ensureDllIsCopied =
                System.Data.Entity.SqlServer.SqlProviderServices.Instance;
            //    Database.SetInitializer<Context>(new DropCreateDatabaseAlways<Context>());

        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {


            modelBuilder.Entity<Connection>().Property(p => p.Version).IsRowVersion();
            modelBuilder.Entity<ConnectionDefinition>().Property(p => p.Version).IsRowVersion();
            modelBuilder.Entity<Station>().Property(p => p.Version).IsRowVersion();
            modelBuilder.Entity<Ticket>().Property(p => p.Version).IsRowVersion();
            modelBuilder.Entity<Train>().Property(p => p.Version).IsRowVersion();
            modelBuilder.Entity<User>().Property(p => p.Version).IsRowVersion();

            //    modelBuilder.Entity<ConnectionDefinition>().Property(p => p.Name).HasMaxLength(120);
            modelBuilder.Entity<Connection>().Property(t => t.DepartureTime).HasColumnAnnotation("Index",
        new IndexAnnotation(new IndexAttribute("IXC_time") { IsUnique = false }));

            modelBuilder.Entity<User>().Property(p => p.Email).HasMaxLength(120);
            modelBuilder.Entity<User>().Property(t => t.Email).HasColumnAnnotation("Index",
        new IndexAnnotation(new IndexAttribute("IXS_Email") { IsUnique = true }));

            //    modelBuilder.Entity<Train>().Property(p => p.Name).HasMaxLength(120);
            //    modelBuilder.Entity<Train>().Property(t => t.Name).HasColumnAnnotation("Index",
            //new IndexAnnotation(new IndexAttribute("IXT_Name") { IsUnique = false }));
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Connection> Connections { get; set; }
        public DbSet<Attribute> Attributes { get; set; }
        public DbSet<ConnectionDefinition> ConnectionDefinitions { get; set; }
        public DbSet<Station> Stations { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Train> Trains { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
