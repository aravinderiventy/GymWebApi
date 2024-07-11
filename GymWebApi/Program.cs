using GymWebApi.Data;
using GymWebApi.Interfaces;
using GymWebApi.Services;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;

var builder = WebApplication.CreateBuilder(args);
Batteries_V2.Init();
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<UserDbContext>(options =>
        options.UseSqlite("DataSource=:memory:"));

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddControllers();

var serviceProvider = builder.Services.BuildServiceProvider();
var context = serviceProvider.GetRequiredService<UserDbContext>();
context.Database.OpenConnection();
context.Database.EnsureCreated();

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
