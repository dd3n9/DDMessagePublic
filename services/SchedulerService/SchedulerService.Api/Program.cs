using Microsoft.EntityFrameworkCore;
using SchedulerService.Infrastructure;
using SchedulerService.Infrastructure.EF.Context;
using SchedulerService.Application;
using SchedulerService.Infrastructure.Common.SyncDataServices;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddGrpc();

builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration, builder.Environment);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using var serviceScope = app.Services.CreateScope();
using var dbContext = serviceScope.ServiceProvider.GetService<AppDbContext>();
dbContext?.Database.Migrate();


app.UseAuthorization();

app.MapControllers();

app.MapGrpcService<GrpcScheduledTaskService>();
app.MapGet("/protos/scheduledtask.proto", async context =>
{
    await context.Response.WriteAsync(File.ReadAllText("../SchedulerService.Infrastructure/Protos/scheduledtask.proto"));
});


app.Run();
