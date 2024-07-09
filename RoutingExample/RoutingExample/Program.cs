using RoutingExample.CustomConstraints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRouting(options =>
{
    options.ConstraintMap.Add("months", typeof(MonthsCustomConstraint));
});

var app = builder.Build();

app.Use(async (context, next) => {
    Endpoint? endpoint = context.GetEndpoint();
    if (endpoint != null)
    {
        await context.Response.WriteAsync($"Endpoint: {endpoint.DisplayName}\n");
    }
    await next(context);
});

// Enable routing
app.UseRouting();

app.Use(async (context, next) => {
    Endpoint? endpoint = context.GetEndpoint();
    if(endpoint != null)
    {
        await context.Response.WriteAsync($"Endpoint: {endpoint.DisplayName}\n");
    }
    await next(context);
});

app.UseEndpoints(endpoints =>
{

    // ************************** BASIC ROUTES **********************
    // add your endpoints here

    //endpoints.Map("map1", async context =>
    //{
    //    await context.Response.WriteAsync("In Map 1");
    //});

    endpoints.MapGet("map1", async context => {
        await context.Response.WriteAsync("In Map 1");
    });

    endpoints.MapPost("map2", async context => {
        await context.Response.WriteAsync("In Map 2");
    });

    // ********** ROUTES WITH PARAMETERS ******************

    endpoints.Map("files/{filename}.{extension}", async (context) =>
    {
        string? fileName = Convert.ToString(context.Request.RouteValues["filename"]);
        string ? extension = Convert.ToString(context.Request.RouteValues["extension"]);

        await context.Response.WriteAsync($"In files = {fileName} - {extension}");
    });

    // ************** ROUTES WITH CONSTRAINTS *****************

    endpoints.Map("employee/profile/{EmployeeName:length(4,7):alpha=DefaultName}", async context =>
    {
        string? employeeName = Convert.ToString(context.Request.RouteValues["employeename"]);
        await context.Response.WriteAsync($"In Employee profile - {employeeName}");

    });

    // Eg: products/details/ 1 
    endpoints.Map("products/details/{id:range(1,1000)?}", async context =>
    {
        if (context.Request.RouteValues.ContainsKey("id"))
        {
            int id = Convert.ToInt32(context.Request.RouteValues["id"]);
            await context.Response.WriteAsync($"Products details - {id}");

        } else
        {
            await context.Response.WriteAsync("Products details - id is not supplied");
        }
    });

    //Eg: daily-digest-report/{reportdate}
    endpoints.Map("daily-digest-report/{reportdate:datetime}", async context =>
    {
       DateTime reportDate = Convert.ToDateTime(context.Request.RouteValues["reportdate"]);
        await context.Response.WriteAsync($"In daily-digest-report - {reportDate}");
    });

    //Eg: cities/cityid
    endpoints.Map("cities/{cityid:guid}", async context =>
    {
        Guid cityId = Guid.Parse(Convert.ToString(context.Request.RouteValues["cityid"])!);
        await context.Response.WriteAsync($"City information - {cityId}");
    });

    //sales-report/2030/apr
    endpoints.Map("sales-report/{year:int:min(1900)}/{month:regex(^(apr|jul|oct|jan)$)}", async context =>
    {
        int year = Convert.ToInt32(context.Request.RouteValues["year"]);
        string? month = Convert.ToString(context.Request.RouteValues["month"]);

        await context.Response.WriteAsync($"Sales report - {year} - {month}");
    });


    // *********************ROUTE WITH CUSTOM CONSTRAINT ****************

    // Custom Route Constraint class for sales-report endpoint
    endpoints.Map("sales-report-with-custom-constraint/{year:int:min(1900)}/{month:months}", async context =>
    {
        int year = Convert.ToInt32(context.Request.RouteValues["year"]);
        string? month = Convert.ToString(context.Request.RouteValues["month"]);

        await context.Response.WriteAsync($"Sales report with custom constraint - {year} - {month}");
    });

    // ********* ENDPOINT SELECTION ORDER EXAMPLES ***********
    endpoints.Map("sales-report/2024/jan", async context =>
    {
        await context.Response.WriteAsync($"Sales report exclusively for 2024 - jan");
    });
});

app.Run(async context =>
{
    await context.Response.WriteAsync($"No route matched at {context.Request.Path}");
});

app.Run();
