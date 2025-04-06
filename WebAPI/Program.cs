using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Domain.Services.Auth.ExtraServices;
using System.Text.Json.Serialization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Domain.Services.Auth.Login;
using Microsoft.OpenApi.Models;
using System.Text;
using Domain;
using WebAPI;
using DAL;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

var connectionString = builder.Configuration.GetConnectionString("Default");

var localConnectionString = builder.Configuration["ConnectionStrings:LocalConnectionString"];

if (!string.IsNullOrWhiteSpace(localConnectionString))
{
    connectionString = localConnectionString;
}

if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException("Connection string is not set. Check environment variables, appsettings.json, or secrets.");
}

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = configuration["Jwt:Issuer"],
            ValidAudience = configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
        };
    });

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Введіть токен у форматі: Bearer {your_token}"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
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

builder.Services.AddAuthorization();

builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<ClothesMarketplaceDbContext>()
    .AddDefaultTokenProviders();
builder.Services.AddScoped<EmailService>();
builder.Services.AddScoped<JwtService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<ProductService>();

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ClothesMarketplaceDbContext>(options =>
{
    options.UseSqlServer(connectionString, b => b.MigrationsAssembly("DAL"));
});

builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<ClothesMarketplaceDbContext>()
    .AddDefaultTokenProviders();

builder.Logging.AddConsole();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ClothesMarketplaceDbContext>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    var migrator = context.Database.GetService<IMigrator>();

    var appliedMigrations = context.Database.GetAppliedMigrations().ToList();
    var pendingMigrations = context.Database.GetPendingMigrations().ToList();

    if (!appliedMigrations.Any())
    {
        context.Database.Migrate();
    }
    else if (pendingMigrations.Any())
    {
        foreach (var migration in pendingMigrations)
        {
            migrator.Migrate(migration);
        }
    }

    var categoryInitializer = new CategoryInitializer(context);
    categoryInitializer.InitializeCategories();

    var brandInitializer = new BrandInitializer(context);
    brandInitializer.InitializeBrands();

    var colorInitializer = new ColorInitializer(context);
    colorInitializer.InitializeColors();

    var forWhomInitializer = new ForWhomInitializer(context);
    forWhomInitializer.InitializeForWhom();

    var productSizeInitializer = new ProductSizeInitializer(context);
    productSizeInitializer.InitializeProductSizes();

    var deliveryMethodInitializer = new DeliveryMethodInitializer(context);
    deliveryMethodInitializer.InitializeDeliveryMethods();

    var productConditionInitializer = new ProductConditionInitializer(context);
    productConditionInitializer.InitializeProductConditions();

    var roleInitializer = new RoleInitializer(roleManager);
    roleInitializer.InitializeRoles();

    var appUserInitializer = new AppUserInitializer(userManager);
    appUserInitializer.InitializeAppUsers();

    var productInitializer = new ProductInitializer(context);
    productInitializer.InitializeProducts();
}

//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI();
//}

app.UseMiddleware<ExceptionHandlerMiddleware>();
app.UseMiddleware<RequestLoggingMiddleware>();
app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
