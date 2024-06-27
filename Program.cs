global using Custom_Hacker_News_Account_API.Models;
global using Microsoft.EntityFrameworkCore;
using Custom_Hacker_News_Account_API.Models.Context;
using Custom_Hacker_News_Account_API.Repository;

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
                          policy.WithOrigins("https://prometheus-nu.vercel.app")
                          .AllowAnyHeader().
                          AllowAnyMethod();
                      });
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