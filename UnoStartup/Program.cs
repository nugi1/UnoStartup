using Microsoft.EntityFrameworkCore;
using UnoAccess.Context;

namespace UnoStartup;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);


        builder.Services.AddControllers();
        builder.Services.AddOpenApi();
        UnoAccess.Configuration.DataAccessConfiguration.AddAccessConfig(builder.Services, builder.Configuration);
        UnoServices.Configuration.DataServicesConfiguration.AddServicesConfig(builder.Services, builder.Configuration);
        builder.Services.AddSignalR();
        
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowSpecificOrigins", policy =>
            {
                policy.WithOrigins("http://localhost:3000", "http://client:3000", "http://4.180.9.160:3000/uno/hub")
                      .AllowAnyMethod()
                      .AllowAnyHeader()
                      .AllowCredentials();
            });
        });

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }
        app.UseAuthorization();

        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<UnoContext>();
            dbContext.Database.Migrate();
        }

        app.UseCors("AllowSpecificOrigins");

        app.MapHub<UnoHub>("/uno/hub");

        app.MapControllers();
        app.Run();
    }
}
