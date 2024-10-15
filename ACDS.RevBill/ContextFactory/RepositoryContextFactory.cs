using System;
using ACDS.RevBill.Entities.Models;
using ACDS.RevBill.Helpers;
using ACDS.RevBill.Repository;
using ACDS.RevBill.Service.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Options;

namespace ACDS.RevBill.ContextFactory
{
    public class RepositoryContextFactory : IDesignTimeDbContextFactory<RepositoryContext>
    {
        public RepositoryContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json", false)
                    .Build();

            var connectionString = configuration.GetConnectionString("sqlConnection");

            var builder = new DbContextOptionsBuilder<RepositoryContext>()
                   .UseSqlServer(connectionString, b => b.MigrationsAssembly("ACDS.RevBill"));

            var httpContextAccessor = new HttpContextAccessor();

            return new RepositoryContext(builder.Options, httpContextAccessor);
        }
    }
}

