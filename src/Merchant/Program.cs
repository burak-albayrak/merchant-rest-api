using Merchant;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run(); //This is how ASP.NET Core applications are configured and run.
    }

    //This method performs the main configuration of the application.
    //The IHostBuilder type is used to configure the runtime behavior of the application.
    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args) //This method is used to create a general default configuration.
            .ConfigureWebHostDefaults(webBuilder => //This method is used to configure the web server.
            {
                webBuilder
                    .SuppressStatusMessages(true) //This line is used to hide server status-related messages.
                    .UseStartup<Startup>(); //This line specifies the startup class of the application.
            });
}