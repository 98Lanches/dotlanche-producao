using Dotlanche.Producao.Application.DependencyInjection;
using Dotlanche.Producao.Data.DependencyInjection;
using Dotlanche.Producao.Integrations.DependencyInjection;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text.Json.Serialization;

namespace Dotlanche.Producao.WebApi;

public class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.Configure<Microsoft.AspNetCore.Mvc.JsonOptions>(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });

        builder.Services.AddMongoDatabase(builder.Configuration);
        builder.Services.AddServiceIntegrations(builder.Configuration);
        builder.Services.AddUseCases();

        builder.Services.AddHealthChecks()
                .AddMongoDb(builder.Configuration.GetConnectionString("DefaultConnection")
                                ?? throw new Exception("No connection string for mongodb provided!"),
                            timeout: TimeSpan.FromSeconds(60));

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "DotLanche Producao API",
                    Description = "Serviço de gerenciamento de pedidos em produção Dotlanches"
                });

                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });

        var app = builder.Build();

        app.MapHealthChecks("/health");

        app.UseSwagger();
        app.UseSwaggerUI();

        app.MapControllers();

        app.Run();
    }
}