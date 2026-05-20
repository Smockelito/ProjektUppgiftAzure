using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Data
{
    public class DbContextFactory : IDesignTimeDbContextFactory<NoseworkDbContext>
    {
        public NoseworkDbContext CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .AddJsonFile("appsettings.Development.json", optional: true)
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<NoseworkDbContext>();
            optionsBuilder.UseSqlServer(
                config.GetConnectionString("DefaultConnection"),
                options => options.EnableRetryOnFailure());

            return new NoseworkDbContext(optionsBuilder.Options);
        }
    }
}
