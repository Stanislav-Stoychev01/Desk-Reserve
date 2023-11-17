using DeskReserve.Data.DBContext;
using DeskReserve.Repository;
using DeskReserve.Interfaces;
using DeskReserve.Services;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

const String CorsDisablePolicy = "AllowAnyOrigin";
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            });
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
builder.Services.AddScoped<IDeskRepository, DeskRepository>();
builder.Services.AddScoped<IBuildingService, BuildingService>();
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