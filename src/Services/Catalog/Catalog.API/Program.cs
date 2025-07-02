using BuildingBlocks.GenericAssembly;
using Carter;
using Catalog.API;

var builder = WebApplication.CreateBuilder(args);
// register services
// add services to the container

builder.Services.AddCarter(new DependencyContextAssemblyCustom<Program>()); // Register Carter services
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(typeof(Program).Assembly);
});

var app = builder.Build();

// middlewares or useMethods
// configure the HTTP request pipeline

app.MapCarter(); // Carter is a library for building HTTP APIs in .NET using a modular approach

app.Run();
