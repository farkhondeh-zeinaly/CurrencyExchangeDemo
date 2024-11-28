using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);
//Ocelot
builder.WebHost.ConfigureAppConfiguration((hostingContext, config) =>
{
    config
        .SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
        .AddJsonFile(path: "apigateway.json", optional: false, reloadOnChange: true);
});
builder.Services.AddOcelot();

//CORS: Allow any Origin
builder.Services.AddCors(policyBuilder =>
    policyBuilder.AddDefaultPolicy(policy =>
        policy.WithOrigins("*").AllowAnyHeader().AllowAnyMethod())
);

var app = builder.Build();

app.UseCors();

app.MapGet("/", () => "Hello World!");

await app.UseOcelot();

app.Run();
