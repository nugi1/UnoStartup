using Microsoft.EntityFrameworkCore;
using UnoAccess.Context;
using UnoStartup.Hub;


var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole(); 
builder.Logging.AddDebug();


builder.Services.AddDbContext<UnoContext>();

UnoAccess.Configure.ConfigureUnoAccess(builder.Services, builder.Configuration);
UnoServices.Configure.ConfigureUnoServices(builder.Services);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost3000", policy =>
    {
        policy.WithOrigins("http://localhost:3000")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<UnoContext>();
    dbContext.Database.Migrate();
}

app.UseRouting();
app.UseAuthorization();
app.MapControllers();
app.UseCors("AllowLocalhost3000");
app.MapHub<UnoHub>("/hub/uno");
app.Run();