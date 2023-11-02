using DeskReserve.Controllers;
using DeskReserve.Data.DBContext;
using DeskReserve.Data.DBContext.Entity;
using DeskReserve.Domain;
using Microsoft.EntityFrameworkCore;

const String CorsDisablePolicy = "AllowAnyOrigin";
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy(CorsDisablePolicy, builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

String connectionString = builder.Configuration["ConnectionStrings:DefaultConnection"];
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddScoped<IBuildingService, BuildingService>();
builder.Services.AddScoped<IBuildingController, BuildingController>();
builder.Services.AddScoped<IRepository, Repository>();


var app = builder.Build();

app.UseCors(CorsDisablePolicy);
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
