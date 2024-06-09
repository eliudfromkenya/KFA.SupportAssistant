using SQLite.CodeFirst;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectMaker.Data
{
    public class MyDbInitializer : SqliteDropCreateDatabaseWhenModelChanges<MyDbContext>
    {
        public MyDbInitializer(DbModelBuilder modelBuilder)
            : base(modelBuilder, typeof(CustomHistory))
        { }

        protected override void Seed(MyDbContext context)
        {
            // Here you can seed your core data if you have any.
        }
    }
    public class CustomHistory : IHistory
    {
        public int Id { get; set; }
        public string Hash { get; set; }
        public string Context { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
