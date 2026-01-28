using Asp.Versioning;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using Order.Infra.Context;
using Order.IoC.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();

// Infrastructure 
builder.Services.AddInfrastructure(builder.Configuration);

// OpenAPI/Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    const string apiVersion = "v1";
    options.EnableAnnotations();

    options.SwaggerDoc(apiVersion, new OpenApiInfo
    {
        Title = "Order API",
        Version = apiVersion,
        Description = "Order API"
    });
});

// API Versioning
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
})
.AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

var app = builder.Build();

// Dev OpenAPI
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
}

// Database migrate at startup (optional, but OK)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<OrderDbContext>();

    if (db.Database.IsRelational())
        await db.Database.MigrateAsync();
    else
        await db.Database.EnsureCreatedAsync();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
