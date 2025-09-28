using FanficsWorldAPI.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace FanficsWorldAPI.WebApi
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<FanficsWorldDbContext>
    {
        public FanficsWorldDbContext CreateDbContext(string[] args)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
                .Build();
            var builder = new DbContextOptionsBuilder<FanficsWorldDbContext>();
            var connectionString = configuration.GetConnectionString("FanficsWorldDb");
            builder.UseSqlServer(connectionString);
            return new FanficsWorldDbContext(builder.Options);
        }
    }
}
