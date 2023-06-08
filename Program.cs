using DiscountAPI.Models;
using DiscountAPI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var _env = builder.Environment;

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
if (connectionString == null)
{
  connectionString = builder.Configuration.GetConnectionString("defaultConnection");
}

Console.WriteLine(connectionString);
builder.Services.AddDbContext<AppDbContext>(op =>
                op.UseNpgsql(connectionString));

builder.Services.AddScoped<IDiscountService, DiscountService>();
builder.Services.AddScoped<IDiscountProductService, DiscountProductService>();
builder.Services.AddScoped<IDiscountBackgroundService, DiscountBackgroundService>();
builder.Services.AddScoped<IMessageProducer, MessageProducer>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
builder.Services.AddCors();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}


app.UseAuthorization();

app.MapControllers();

app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

PrepDb.PrepPopulation(app, _env.IsProduction());

TimeZoneInfo localTimeZone = TimeZoneInfo.Local;
Console.WriteLine(localTimeZone.Id);

app.Run();
