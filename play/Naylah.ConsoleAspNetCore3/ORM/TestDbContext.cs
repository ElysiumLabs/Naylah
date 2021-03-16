using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
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

        public TestDbContext(
            DbContextOptions<TestDbContext> options) : base(options)
        {
        }

        public TestDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<TestDbContext>();
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.Development.json")
                .Build();

            var connectionString = configuration.GetConnectionString("LocalDB");
            optionsBuilder.UseSqlServer(connectionString, ConfigureDBContext);

            return new TestDbContext(optionsBuilder.Options);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(schema);

            modelBuilder.Entity<Person>().HasKey(x => x.Id);

            Author.EntityConfigure(modelBuilder);
            Book.EntityConfigure(modelBuilder);
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
