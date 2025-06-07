using Microsoft.EntityFrameworkCore;
using UtterlyAPI.DAL;
using UtterlyAPI.Models;

namespace UtterlyAPI;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        var connectionString = builder.Configuration.GetConnectionString("UtterlyContextConnection") ?? throw new InvalidOperationException("Connection string 'UtterlyContextConnection' not found.");

        builder.Services.AddDbContext<UtterlyContext>(options => options.UseSqlServer(connectionString));
        builder.Services.AddTransient<PostManager>();

        builder.Services.AddControllers();

        // Add Swagger services
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            // Enable Swagger UI
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
