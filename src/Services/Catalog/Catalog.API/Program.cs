using BuildingBlocks.GenericAssembly;
using Microsoft.AspNetCore.Http.Json;

var builder = WebApplication.CreateBuilder(args);
// register services
// add services to the container

builder.Services.AddCarter(new DependencyContextAssemblyCustom<Program>()); // Register Carter services
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(typeof(Program).Assembly);
});
builder.Services.AddMarten(opts =>
{
    opts.Connection(builder.Configuration.GetConnectionString("CatalogDb")!);
}).UseLightweightSessions();
builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
{
    options.SerializerOptions.PropertyNameCaseInsensitive = true;
});

var app = builder.Build();

// middlewares or useMethods
// configure the HTTP request pipeline

app.MapCarter(); // Carter is a library for building HTTP APIs in .NET using a modular approach

app.Run();
