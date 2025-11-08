using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using ProductsApi.Infrastructure.Extensions;
using ProductsApi.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddInfrastructure(builder.Configuration);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=products.db";

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase(connectionString)
);

var defaultKey = "DEV_SECRET_KEY_SUPER_LONG_2025_12345678";
var secret = builder.Configuration["JWT_SECRET"] ?? defaultKey;
var key = Encoding.UTF8.GetBytes(secret);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["JWT_ISSUER"] ?? "ProductsApi",
            ValidateAudience = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateLifetime = true
        };
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});
builder.WebHost.UseUrls("http://+:5000");

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5000);
}).UseSetting("AllowedHosts", "*");


builder.Services.AddAuthorization();

var app = builder.Build();

var imagesPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
Directory.CreateDirectory(imagesPath);

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(imagesPath),
    RequestPath = "/images"
});

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
