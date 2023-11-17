using DeskReserve.Data.DBContext;
using DeskReserve.Domain.Service;
using DeskReserve.Mapper;
using DeskReserve.Repository;
using DeskReserve.Interfaces;
using DeskReserve.Services;
using DeskReserve.Domain;
using Microsoft.EntityFrameworkCore;
using DeskReserve.Controllers;

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

builder.Services.AddScoped<IDeskService, DeskService>();
builder.Services.AddScoped<DesksController, DesksController>();
builder.Services.AddScoped<IDeskRepository, DeskRepository>();
builder.Services.AddScoped<IBuildingService, BuildingService>();
builder.Services.AddScoped<BuildingController, BuildingController>();
builder.Services.AddScoped<IBuildingRepository, BuildingRepository>();

builder.Services.AddScoped<IFloorRepository, FloorRepository>();
builder.Services.AddScoped<IFloorService, FloorService>();
builder.Services.AddScoped<IRoomRepository, RoomRepository>();

builder.Services.AddScoped<IRoomService, RoomService>();

var app = builder.Build();

app.UseCors(CorsDisablePolicy);
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();