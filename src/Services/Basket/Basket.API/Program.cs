using Basket.API.Repositories;
using BuildingBlocks.Exceptions.Handler;
using Discount.Grpc.Protos;
using HealthChecks.UI.Client;
using ImTools;
using JasperFx;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);
// add services or register services to the container
builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddMarten(opt =>
{
    opt.Connection(builder.Configuration.GetConnectionString("BasketDb")!);
    opt.Schema.For<ShoppingCart>().Identity(x => x.UserName);
    opt.AutoCreateSchemaObjects = AutoCreate.CreateOnly;
}).UseLightweightSessions();


builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
{
    options.SerializerOptions.PropertyNameCaseInsensitive = true;
});

//Data Services and Repositories
builder.Services.AddCarter(new DependencyContextAssemblyCustom<Program>());
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(typeof(Program).Assembly);
    config.AddOpenBehavior(typeof(LoggingBehavior<,>)); // Register the logging behavior for MediatR
    config.AddOpenBehavior(typeof(ValidationBehavior<,>)); // Register the validation behavior for MediatR
});
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly); // Register FluentValidation validators
builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.Decorate<IBasketRepository,CachedBasketRepository>();

builder.Services.AddStackExchangeRedisCache(opt =>
{
    opt.Configuration = builder.Configuration.GetConnectionString("Redis");

});

//Add GRPC
builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(opt =>
{
    opt.Address = new Uri(builder.Configuration["GrpcSettings:DiscountUrl"]!);
});

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
//health checks for this app ,database and cache
builder.Services.AddHealthChecks()
    .AddNpgSql(builder.Configuration.GetConnectionString("BasketDb")!)
    .AddRedis(builder.Configuration.GetConnectionString("Redis")!);
var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
// middlewares and useMethods
// configure the HTTP request pipeline
app.MapCarter();
app.MapHealthChecks("/health", new HealthCheckOptions { ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse });

app.Run();
