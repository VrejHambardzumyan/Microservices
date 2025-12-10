using EnrollmentService.Infrastructure.Persistence;
using Common.Messaging.Rabbit;
using Common.Swagger;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<EnrollmentDbContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("EnrollmentDb")));

EnrollmentService.Infrastructure.Config.DependencyInjection.AddEnrollment(builder.Services);
EnrollmentService.Application.Services.DependencyInjection.AddEnrollmentApplication(builder.Services);

builder.Services.AddSingleton<RabbitConnection>();
builder.Services.AddSingleton<RabbitPublisher>();
builder.Services.AddHostedService<RabbitConsumerHostedService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt => SwaggerSetup.Configure(opt, "Enrollment Service"));
builder.Services.AddControllers();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<EnrollmentDbContext>();
    db.Database.Migrate();
}

app.UseSwagger();
app.UseSwaggerUI(c => SwaggerSetup.ConfigureUI(c, "Enrollment Service"));

app.MapControllers();
app.Run();