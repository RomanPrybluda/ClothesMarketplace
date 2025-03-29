using DAL;
using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Services;
using System.Text.Json.Serialization;

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

builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<AppUserService>();

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ClothesMarketplaceDbContext>(options =>
{
    options.UseSqlServer(connectionString, b => b.MigrationsAssembly("DAL"));
});

builder.Logging.AddConsole();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});

builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<ClothesMarketplaceDbContext>()
    .AddDefaultTokenProviders();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ClothesMarketplaceDbContext>();
    
    context.Database.Migrate();

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

    var adAndProductInitializer = new AdAndProductInitializer(context);
    adAndProductInitializer.InitializeAdsAndProducts();

    var userManager = services.GetRequiredService<UserManager<AppUser>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    await RoleInitializer.Initialize(userManager, roleManager);
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();
