using ContactCore.DTOs;
using ContactCore.Interfaces;
using ContactCore.Validators;
using ContactInfrastructure.Data;
using ContactInfrastructure.Repository;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ContactDbContext>(options =>
    options.UseSqlServer("DefaultConnection", b =>
        b.MigrationsAssembly("ContactInfrastructure")));
builder.Services.AddScoped<IContactRepository, ContactRepository>();
builder.Services.AddTransient<IValidator<ContactDTO>, ContactValidator>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
