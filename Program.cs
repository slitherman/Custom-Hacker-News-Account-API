global using Custom_Hacker_News_Account_API.Models;
global using Microsoft.EntityFrameworkCore;
using Custom_Hacker_News_Account_API.Models.Context;
using Custom_Hacker_News_Account_API.Models.Helper;
using Custom_Hacker_News_Account_API.Models.Login;
using Custom_Hacker_News_Account_API.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
ConfigureDbContext(builder);
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("https://prometheus-nu.vercel.app", "https://localhost:3001", "https://localhost:3000")
                           .AllowAnyHeader().
                          AllowAnyMethod();
                      });
});

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
      .AddEntityFrameworkStores<AccountDbContext>()
      .AddDefaultTokenProviders();
var key = JwtSettings.Key;
var issuer = JwtSettings.Issuer;
var audience = JwtSettings.Audience;

if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(issuer) || string.IsNullOrEmpty(audience))
{
    throw new InvalidOperationException("JWT configuration is missing.");
}
var keyBytes = Encoding.UTF8.GetBytes(key);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = issuer,
        ValidAudience = audience,
        IssuerSigningKey = new SymmetricSecurityKey(keyBytes)
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdminRole", policy => policy.RequireRole(Roles.Admin));
    options.AddPolicy("RequireModeratorRole", policy => policy.RequireRole(Roles.Moderator));
    options.AddPolicy("RequireUserRole", policy => policy.RequireRole(Roles.User));
});


var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(MyAllowSpecificOrigins);
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();


static void ConfigureDbContext(WebApplicationBuilder builder)
{
    var connectionString = Secret.Conn;
    if (string.IsNullOrEmpty(connectionString))
    {
        throw new InvalidOperationException("Connection string is null or empty.");
    }

    builder.Services.AddDbContext<AccountDbContext>(options =>
        options.UseSqlServer(connectionString)
               .EnableSensitiveDataLogging());
}

public class RoleInitializationStartupFilter : IStartupFilter
{
    public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
    {
        return builder =>
        {
            // Ensure roles are created during application startup
            using (var scope = builder.ApplicationServices.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                var roleInitializer = serviceProvider.GetRequiredService<RoleInitializer>();
                roleInitializer.InitializeAsync().GetAwaiter().GetResult();
            }

            next(builder);
        };
    }
}

public class RoleInitializer
{
    private readonly IServiceProvider _serviceProvider;

    public RoleInitializer(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task InitializeAsync()
    {
        var roleManager = _serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        foreach (var roleName in new[] { Roles.Admin, Roles.Moderator, Roles.User })
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }
    }
}