using ConfigurationExample;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.Configure<WEatherApiOptions>(builder.Configuration.GetSection("WeatherApi"));

// Load MyOwnConfig.json
builder.Host.ConfigureAppConfiguration((hosting, config) =>
{
    config.AddJsonFile("MyOwnConfig.json", optional: true, reloadOnChange: true);
});

var app = builder.Build();
app.UseStaticFiles();
app.UseRouting();
app.MapControllers();

// Configuration examples
/*app.UseEndpoints(endpoints =>
{
    endpoints.Map("/config", async context =>
    {
       await context.Response.WriteAsync(app.Configuration["MyKey"] + "\n") ;
       await context.Response.WriteAsync(app.Configuration.GetValue<string>("MyKey") + "\n");
        await context.Response.WriteAsync(app.Configuration.GetValue<int>("notexisted", 0) + "\n");

    });
});*/

app.Run();
