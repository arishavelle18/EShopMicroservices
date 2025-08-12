var builder = WebApplication.CreateBuilder(args);
// register services
// add services to the container

builder.Services.AddCarter(new DependencyContextAssemblyCustom<Program>()); // Register Carter services
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(typeof(Program).Assembly);
    config.AddOpenBehavior(typeof(LoggingBehavior<,>)); // Register the logging behavior for MediatR
    config.AddOpenBehavior(typeof(ValidationBehavior<,>)); // Register the validation behavior for MediatR
});

builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly); // Register FluentValidation validators
builder.Services.AddMarten(opts =>
{
    opts.Connection(builder.Configuration.GetConnectionString("CatalogDb")!);
}).UseLightweightSessions();

if (builder.Environment.IsDevelopment())
    builder.Services.InitializeMartenWith<CatalogInitialData>();

builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
{
    options.SerializerOptions.PropertyNameCaseInsensitive = true;
});

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddHealthChecks()
    .AddNpgSql(builder.Configuration.GetConnectionString("CatalogDb")!);

var app = builder.Build();

// middlewares or useMethods
// configure the HTTP request pipeline

app.MapCarter(); // Carter is a library for building HTTP APIs in .NET using a modular approach
app.UseExceptionHandler();
app.MapHealthChecks("/health", new HealthCheckOptions { ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse });

app.Run();
