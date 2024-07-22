using Autofac;
using Autofac.Extensions.DependencyInjection;
using ServiceContracts;
using Services;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

builder.Services.AddControllersWithViews();
/*builder.Services.Add(new ServiceDescriptor(
    typeof(ICitiesService),
    typeof(CitiesService),
    ServiceLifetime.Scoped
    ));
builder.Services.AddTransient<ICitiesService, CitiesService>();*/

// *************** AutoFac ************************
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder => {
    // containerBuilder.RegisterType<CitiesService>().As<ICitiesService>().InstancePerDependency(); // AddTransient
    containerBuilder.RegisterType<CitiesService>().As<ICitiesService>().InstancePerLifetimeScope(); // AddScoped
    // containerBuilder.RegisterType<CitiesService>().As<ICitiesService>().SingleInstance(); // AddTransient
});

// builder.Services.AddScoped<ICitiesService, CitiesService>();   
//builder.Services.AddSingleton<ICitiesService, CitiesService>();
var app = builder.Build();

app.UseRouting();
app.MapControllers();
app.UseStaticFiles();

app.Run();
