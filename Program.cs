using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PmsApi.DataContexts;
using PmsApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("PmsContext");


var serverVersion = ServerVersion.AutoDetect(connectionString);
builder.Services.AddAuthorization();
builder.Services
.AddIdentityApiEndpoints<User>()
//.AddIdentity<User, Role>()
.AddEntityFrameworkStores<PmsContext>()
.AddApiEndpoints()
.AddDefaultTokenProviders();

builder.Services.AddDbContext<PmsContext>(
    opt => opt.UseMySql(connectionString, serverVersion)
    );
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;

});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseAuthorization();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapIdentityApi<User>();
app.UseHttpsRedirection();



app.MapControllers();

app.Run();
