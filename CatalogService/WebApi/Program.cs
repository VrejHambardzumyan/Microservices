using CatalogService.Infrastructure.Persistence;
using Common.Messaging.Rabbit;
using Common.Swagger;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// PostgreSQL
builder.Services.AddDbContext<CatalogDbContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("CatalogDb")));

CatalogService.Infrastructure.Config.DependencyInjection.AddCatalog(builder.Services);
CatalogService.Application.Services.DependencyInjection.AddCatalogApplication(builder.Services);

// RabbitMQ
builder.Services.AddSingleton<RabbitConnection>();
builder.Services.AddSingleton<RabbitPublisher>();
builder.Services.AddHostedService<RabbitConsumerHostedService>(); 

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt => SwaggerSetup.Configure(opt, "Catalog Service"));
builder.Services.AddControllers();


var app = builder.Build();

// Migrate
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<CatalogDbContext>();
    db.Database.Migrate();
}

// Swagger
app.UseSwagger();
app.UseSwaggerUI(c => SwaggerSetup.ConfigureUI(c, "Catalog Service"));

app.MapControllers();

app.Run();