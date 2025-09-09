using Microsoft.EntityFrameworkCore;
using BabyPOS_API.Data;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "BabyPOS API", Version = "v1" });
    // Add JWT Bearer support to Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' [space] and then your valid JWT token. Example: Bearer eyJhbGci..."
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.WriteIndented = true;
});
builder.Services.AddScoped<BabyPOS_API.Infrastructure.Repositories.IMenuItemRepository, BabyPOS_API.Infrastructure.Repositories.MenuItemRepository>();
builder.Services.AddScoped<BabyPOS_API.Application.Services.IMenuItemService, BabyPOS_API.Application.Services.MenuItemService>();
builder.Services.AddScoped<BabyPOS_API.Infrastructure.Repositories.ITableRepository, BabyPOS_API.Infrastructure.Repositories.TableRepository>();
builder.Services.AddScoped<BabyPOS_API.Application.Services.ITableService, BabyPOS_API.Application.Services.TableService>();
builder.Services.AddScoped<BabyPOS_API.Infrastructure.Repositories.IOrderRepository, BabyPOS_API.Infrastructure.Repositories.OrderRepository>();
builder.Services.AddScoped<BabyPOS_API.Application.Services.IOrderService, BabyPOS_API.Application.Services.OrderService>();
builder.Services.AddScoped<BabyPOS_API.Infrastructure.Repositories.IOrderItemRepository, BabyPOS_API.Infrastructure.Repositories.OrderItemRepository>();
builder.Services.AddScoped<BabyPOS_API.Application.Services.IOrderItemService, BabyPOS_API.Application.Services.OrderItemService>();
builder.Services.AddOpenApi();

// JWT Authentication
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"] ?? "BabyPOS",
            ValidAudience = builder.Configuration["Jwt:Audience"] ?? "BabyPOSUsers",
            IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(
                System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? "SuperSecretKey"))
        };
    });

// Enable CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "BabyPOS API V1");
    });
    app.MapOpenApi();
}


app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

// Make Program class accessible for testing
public partial class Program { }
