using Merchant.Configs;
using Merchant.Middlewares;
using Merchant.Repositories;
using Merchant.Services;
using MongoDB.Driver;

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
        services.Configure<MongoDBSettings>(Configuration.GetSection(nameof(MongoDBSettings)));
        var settings = Configuration.GetSection(nameof(MongoDBSettings)).Get<MongoDBSettings>();
        services.AddSingleton(
            settings); // Register MongoDBSettings object to be created only once throughout the application.
        services.AddControllers(); // Adding ASP.NET Core Web API controllers.
        services.AddEndpointsApiExplorer(); // Adding the service required for generating API documentation.
        services.AddSwaggerGen(); // Adding Swagger documentation generation feature.
        services.AddResponseCompression(); // Adding compression service for responses.
        // Adding a concrete Service class corresponding to the IService interface.
        services.AddSingleton<IService, Service>();
        // Adding a concrete Repository class corresponding to the IRepository interface.
        services.AddSingleton<IRepository, Repository>(_ => // bu neden "_" ????????
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DBName);
            var collection = database.GetCollection<Merchant>(settings.CollectionName);

            return new Repository(collection);
        });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void
        Configure(IApplicationBuilder app,
            IWebHostEnvironment env) // This method is used for configuring the application. 
    {
        app.UseDeveloperExceptionPage(); // Error handling
        app.UseSwagger(); //Swagger
        app.UseSwaggerUI(); //Swagger
        app.UseMiddleware<ErrorHandlingMiddleware>();
        app.UseCors("AllowAll"); // Enforce CORS policies.
        app.UseResponseCompression(); // Compresses data sent from the server to improve transmission speed.
        app.UseRouting(); //  Determine which Controller a incoming request should be routed to.
        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        // Defines how requests are mapped to Controllers and Actions.
    }
}