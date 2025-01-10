namespace UnoAccess.Context;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

public class UnoContextFactory : IDesignTimeDbContextFactory<UnoContext>
{
    public UnoContext CreateDbContext(string[] args)
    {
        var basePath = Path.Combine(Directory.GetCurrentDirectory(), "..", "UnoStartup");
        var configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<UnoContext>();

        return new UnoContext(optionsBuilder.Options);
    }
}