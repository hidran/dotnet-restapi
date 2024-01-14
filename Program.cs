using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PmsApi.DataContexts;
using PmsApi.Models;
using Microsoft.OpenApi.Models;
using PmsApi.Utilities;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("PmsContext");


var serverVersion = ServerVersion.AutoDetect(connectionString);
builder.Services.AddAuthorization( opt =>
{
    opt.AddPolicy("IsAdmin", p => p.RequireRole(["Admin"]));
    opt.AddPolicy("IsSuperAdmin", p => p.RequireClaim("SuperAdmin"));
});
builder.Services
.AddIdentityApiEndpoints<User>()
.AddRoles<Role>()
.AddEntityFrameworkStores<PmsContext>()
.AddApiEndpoints()
.AddDefaultTokenProviders();

builder.Services.AddDbContext< PmsContext>(
    opt => opt.UseMySql(connectionString, serverVersion)
    );
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;

});
builder.Services.AddScoped<IUserContextHelper, UserContextHelper>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
    opt =>
    {
        opt.AddSecurityDefinition("oauth2",
             new OpenApiSecurityScheme
             {
                 In = ParameterLocation.Header,
                 Name = "Authorization",
                 Type = SecuritySchemeType.ApiKey
             }
            );

        opt.AddSecurityRequirement(
           new OpenApiSecurityRequirement
           {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "oauth2"
                        }

                    },
                    []

            }
           }
       );
    }
    );

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
