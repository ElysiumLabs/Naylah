using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Naylah.ConsoleAspNetCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Naylah.ConsoleAspNetCore.ORM
{
    public class TestDbContext : DbContext, IDesignTimeDbContextFactory<TestDbContext>
    {
        const string schema = "Test";

        public TestDbContext()
        {

        }

        public TestDbContext(DbContextOptions<TestDbContext> options) : base(options)
        {
        }

        public TestDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<TestDbContext>();
            optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=NaylahTestDevDB;Trusted_Connection=True;MultipleActiveResultSets=false", ConfigureDBContext);

            return new TestDbContext(optionsBuilder.Options);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(schema);

            modelBuilder.Entity<Person>().HasKey(x => x.Id);
        }

        internal static void ConfigureDBContext(SqlServerDbContextOptionsBuilder obj)
        {
            obj.MigrationsHistoryTable("__Migrations", schema);
        }

        public int Commit()
        {
            return SaveChanges();
        }
    }

}
