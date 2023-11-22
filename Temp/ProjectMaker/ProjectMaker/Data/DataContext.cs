
using EntityFramework.Triggers;
using PPMS.Console.Models;
using System.Data.Common;
using System.Data.Entity;

namespace ProjectMaker.Data
{
    public class MyDbContext : DbContextWithTriggers
    {
        public DbSet<DatabaseTable> Tables { get; set; }
        public DbSet<DataGroup> Groups { get; set; }
        public DbSet<TableColumn> Columns { get; set; }
        public DbSet<TablePrimary> PrimaryKeys { get; set; }
        public DbSet<TableRelation> Relations { get; set; }
       // public DbSet<InitialData> InitialData { get; set; }

       
        public MyDbContext(string nameOrConnectionString)
           : base(nameOrConnectionString)
        {
            Configure();
        }

        public MyDbContext(System.Data.Common.DbConnection connection, bool contextOwnsConnection)
            : base(connection, contextOwnsConnection)
        {
            Configure();
        }

        private void Configure()
        {
            Configuration.ProxyCreationEnabled = true;
            Configuration.LazyLoadingEnabled = true;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //ModelConfiguration.Configure(modelBuilder);
            var initializer = new MyDbInitializer(modelBuilder);
            Database.SetInitializer(initializer);
        }
    }
}