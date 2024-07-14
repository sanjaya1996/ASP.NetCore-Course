using ModelValidationsExample.CustomModelBinder;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers(options =>
{
    //options.ModelBinderProviders.Insert(0, new PersonBinderProvider());
});
// Add below if you need to accept xml data in the request
builder.Services.AddControllers().AddXmlSerializerFormatters();
var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.MapControllers();

app.Run();
