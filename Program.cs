using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:3000") // Update if deployed
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services.AddControllers();

var app = builder.Build();

// Middleware
app.UseCors("AllowFrontend");

app.UseHttpsRedirection();

// Optional: UseAuthentication and UseAuthorization if using Auth
// app.UseAuthentication();
// app.UseAuthorization();

app.MapControllers(); // Route to your controller endpoints

app.MapReverseProxy();

app.Run();
