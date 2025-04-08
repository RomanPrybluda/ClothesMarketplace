using DAL;
using Domain;
using Domain.Abstractions;
using Domain.Helpers;
using Domain.Services.Images;
using Domain.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using System.Text.Json.Serialization;
using Domain.Mapping;

var builder = WebApplication.CreateBuilder(args);

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


builder.Services.AddAutoMapper(typeof(ProductCreationProfileMap).Assembly);

builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddScoped<IImageEncoderFactory, ImageEncoderFactory>();

builder.Services.AddValidatorsFromAssemblyContaining<ImageValidator>();

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddSwaggerGen(options =>
{
    options.MapType<IFormFile>(() => new OpenApiSchema { Type = "string", Format = "binary" });
});

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

    //if (!appliedMigrations.Any())
    //{
    //    context.Database.Migrate();
    //}
    //else if (pendingMigrations.Any())
    //{
    //    foreach (var migration in pendingMigrations)
    //    {
    //        migrator.Migrate(migration);
    //    }
    //}

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

app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
