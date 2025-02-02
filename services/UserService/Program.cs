using Microsoft.EntityFrameworkCore;
using UserService;
using UserService.Data;
using UserService.SyncDataServices.Grpc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddGrpc();

builder.Services.AddDependencyInjection(builder.Configuration, builder.Environment);

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

app.MapGrpcService<GrpcUserService>();

app.MapGet("/protos/users.proto", async context =>
{
    await context.Response.WriteAsync(File.ReadAllText("Protos/users.proto"));
});

app.Run();
