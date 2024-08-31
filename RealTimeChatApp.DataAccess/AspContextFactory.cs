
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;


namespace RealTimeChatApp.DataAccess
{
    internal class AspContextFactory : IDesignTimeDbContextFactory<AspContext>
    {
        public AspContext CreateDbContext(string[] args)
        {
            var basePath = Path.Combine(Directory.GetCurrentDirectory(), "..", "Real-Time Chat App");
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true)
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            var optionBuilder = new DbContextOptionsBuilder<AspContext>();
            optionBuilder.UseSqlServer(connectionString);

            return new AspContext(optionBuilder.Options);

            
        }
    }
}
