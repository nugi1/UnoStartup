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
                policy.WithOrigins("http://localhost:3000")
                      .AllowAnyMethod()
                      .AllowAnyHeader();
            });
        });

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<UnoContext>();
            dbContext.Database.Migrate();
        }

        app.MapHub<UnoHub>("/uno/hub");

        app.MapControllers();

        app.UseCors("AllowSpecificOrigins");

        app.Run();
    }
}
