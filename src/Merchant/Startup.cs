using System.Reflection;
using Merchant.Configs;
using Merchant.Middlewares;
using Merchant.Repositories;
using Merchant.Services;
using MongoDB.Driver;
using FluentValidation.AspNetCore;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

namespace Merchant;

public class Startup
{
    private readonly string _environment; // represents the environment that the application is currently running in.

    public Startup(IConfiguration configuration, IWebHostEnvironment env)
    {
        Configuration = configuration; // This allows you to access configuration values throughout the application.
        Console.WriteLine($"Environment: {env.EnvironmentName}");
        var envVariables = Environment.GetEnvironmentVariables();
        if (string.IsNullOrWhiteSpace(envVariables["ASPNETCORE_ENVIRONMENT"]?.ToString()))
            throw new ArgumentNullException("ASPNETCORE_ENVIRONMENT");
        _environment = envVariables["ASPNETCORE_ENVIRONMENT"].ToString();
    }

    public IConfiguration Configuration { get; }
    public MongoDBSettings MongoDbSettings { get; set; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddCors(options => // Cross Origin Source (content'i yöneten yapı)
        {
            options.AddPolicy("AllowAll", builder =>
            {
                builder
                    .WithOrigins("*") // Allowing requests from all sources.
                    //.WithOrigins("https://example.com") You can specify the allowed domain here.
                    .AllowAnyMethod()
                    .AllowAnyHeader();
                //.AllowCredentials();
            }); // Checks if incoming requests from browsers include credentials.
            // If allowing a specific domain, AllowCredentials() can be used.
            // If allowing from all sources, do not use AllowCredentials()
            // and specify a particular domain.
        });

        // Getting configuration values for MongoDBSettings class from application configuration.
        MongoDbSettings = Configuration.GetSection(nameof(MongoDBSettings)).Get<MongoDBSettings>();
        services.AddSingleton(
            MongoDbSettings); // Register MongoDBSettings object to be created only once throughout the application.
        services.AddControllers(); // Adding ASP.NET Core Web API controllers.
        services.AddEndpointsApiExplorer(); // Adding the service required for generating API documentation.
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Merchant API",
                Description = "Merchant Management API",
                TermsOfService = new Uri("https://www.tesodev.com"),
                Contact = new OpenApiContact
                {
                    Name = "Burak Albayrak",
                    Email = "burak_albayrak0@icloud.com",
                    Url = new Uri("https://www.instagram.com/burakk_albayrak/")
                },
                License = new OpenApiLicense
                {
                    Name = "TESODEV Open License",
                    Url = new Uri("https://www.tesodev.com")
                }
            });
            c.ExampleFilters();
            // generate the xml docs that'll drive the swagger docs
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath);
        }); // Adding Swagger documentation generation feature.
        services.AddSwaggerExamplesFromAssemblyOf<Startup>();
        services.AddResponseCompression(); // Adding compression service for responses.
        // Adding a concrete Service class corresponding to the IService interface.
        services.AddSingleton<IService, Service>();
        services.AddControllers() // Todo bunabakk
            .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Startup>());
        // Adding a concrete Repository class corresponding to the IRepository interface.
        services.AddSingleton<IRepository, Repository>(_ => // bu neden "_" ????????
        {
            var client = new MongoClient(MongoDbSettings.ConnectionString);
            var database = client.GetDatabase(MongoDbSettings.DBName);
            var collection = database.GetCollection<Merchant>(MongoDbSettings.CollectionName);
            var logger = _.GetService<ILogger<Repository>>();

            return new Repository(collection, logger);
        });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void
        Configure(IApplicationBuilder app,
            IWebHostEnvironment env) // This method is used for configuring the application. 
    {
        app.UseDeveloperExceptionPage(); // Error handling
        app.UseSwagger(); //Swagger
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Merchant Api v1");
        }); //SwaggerUI option
        app.UseMiddleware<ErrorHandlingMiddleware>();
        app.UseCors("AllowAll"); // Enforce CORS policies.
        app.UseResponseCompression(); // Compresses data sent from the server to improve transmission speed.
        app.UseRouting(); //  Determine which Controller a incoming request should be routed to.
        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        // Defines how requests are mapped to Controllers and Actions.
    }
}