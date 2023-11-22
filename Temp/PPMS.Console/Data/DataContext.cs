using EntityFrameworkCore.Triggers;
using Microsoft.EntityFrameworkCore;
using PPMS.Console.Models;

namespace PPMS.Console.Data
{
    public class Context : DbContextWithTriggers
    {
        public DbSet<DatabaseTable> Tables { get; set; }
        public DbSet<DataGroup> Groups { get; set; }
        public DbSet<TableColumn> Columns { get; set; }
        public DbSet<TablePrimary> PrimaryKeys { get; set; }
        public DbSet<TableRelation> Relations { get; set; }
        public DbSet<InitialData> InitialData { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(@"Filename=Database.db");
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // builder.Entity<Like>()
            //     .HasOne(u => u.Liker)
            //     .WithMany(u => u.Likees)
            //     .HasForeignKey(u => u.LikerId)
            //     .OnDelete(DeleteBehavior.Restrict);
            // builder.Entity<Photo>().HasQueryFilter(p => p.IsApproved);
        }
    }
}