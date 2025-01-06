using UnoAccess.Entities;
using UnoAccess.Interfaces;
using UnoServices.Interfaces;
using UnoServices.Services;
using UnoStartup.Hub;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
UnoAccess.Configure.ConfigureUnoAccess(builder.Services, builder.Configuration);
UnoServices.Configure.ConfigureUnoServices(builder.Services);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost3000", policy =>
    {
        policy.WithOrigins("http://localhost:3000") // Allow requests from localhost:3000
            .AllowAnyMethod()                    // Allow any HTTP method (GET, POST, etc.)
            .AllowAnyHeader()                    // Allow any headers
            .AllowCredentials();                 // Allow credentials (e.g., cookies, authentication)
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.MapHub<UnoHub>("/hub/uno");

app.Run(); 