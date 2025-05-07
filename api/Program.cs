using api.Data;
using api.Repositories;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using System.Text;
using api.Hubs;
using Microsoft.AspNetCore.Identity;
using api.Entities.Auth;
using api.Options;

var builder = WebApplication.CreateBuilder(args);

// Add Authorization and Authentication services
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = IdentityConstants.BearerScheme;
    options.DefaultChallengeScheme = IdentityConstants.BearerScheme;
})
.AddJwtBearer(options =>
{
    var jwtKey = builder.Configuration["Jwt:Key"];

    if (string.IsNullOrEmpty(jwtKey))
    {
        throw new InvalidOperationException("JWT key is not configured. Please set the Jwt:Key in appsettings.json or environment variables.");
    }

    options.Authority = builder.Configuration["Jwt:Issuer"];
    options.Audience = builder.Configuration["Jwt:Audience"];
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
});

// Connect to DB
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Identity services
builder.Services
    .AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Cors Policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// SignalR
builder.Services.AddSignalR();

// Options
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));
builder.Services.Configure<RefreshTokenOptions>(builder.Configuration.GetSection("RefreshToken"));

// Controllers
builder.Services.AddControllers();

// Repositories
builder.Services.AddScoped<TokenRepository>();

var app = builder.Build();
// Middleware
app.UseHttpsRedirection();

app.UseRouting();
app.UseCors("AllowReactApp");
app.UseAuthentication();  // Use JWT Bearer Authentication
app.UseAuthorization();

// Endpoints
app.MapControllers();
app.MapHub<QuizHub>("/quizHub");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Use(async (context, next) =>
{
    Console.WriteLine($"Request: {context.Request.Method} {context.Request.Path}");
    await next();
});

app.Run();
