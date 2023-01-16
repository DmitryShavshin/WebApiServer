using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebApiServer.Configuration;
using WebApiServer.Data;
using WebApiServer.Interfaces;
using WebApiServer.Models;
using WebApiServer.Repository;

var builder = WebApplication.CreateBuilder(args);
var connectionString =      builder.Configuration.GetConnectionString("DefaultConnection");
var jwtConfig =             builder.Configuration.GetSection("JwtConfig");
var jwtSecret =             builder.Configuration.GetSection("JwtConfig:Secret").Value;


builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(connectionString);
})
.AddIdentity<User, Role>(config => {
    config.Password.RequireDigit = false;
    config.Password.RequireLowercase = false;
    config.Password.RequireUppercase = false;
    config.Password.RequireNonAlphanumeric = false;
    config.Password.RequiredLength = 6;
})
  .AddRoles<Role>()
  .AddEntityFrameworkStores<DataContext>();

builder.Services.Configure<JwtConfig>(jwtConfig);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(jwt =>
{
    var key = Encoding.ASCII.GetBytes(jwtSecret);

    jwt.SaveToken = true;
    jwt.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false,
        RequireExpirationTime = false, // To do -- needs to be implement update generate tokens  
        ValidateLifetime = true
    };
});

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddTransient<IUser, UserRepository>();
builder.Services.AddTransient<IAuth, AuthRepository>();
builder.Services.AddTransient<ICustomer, CustomerRepository>();
builder.Services.AddTransient<IProduct, ProductRepository>();

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
