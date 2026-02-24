using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using tedMovieApp;
using tedMovieApp.Repositories;
using tedMovieApp.Repositories.Interfaces;
using tedMovieApp.Services;
using tedMovieApp.Services.Interfaces;
using tedMovieApp.Tools;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddRazorPages();
//builder.Services.AddControllers();

// Controllers + camelCase JSON so React sees reviewId/title/reviewText/etc.
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
});

// CORS for your React dev origin
builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});



// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Configuration.AddEnvironmentVariables();

builder.Services.AddScoped<IOmdbApiService, OmdbApiService>();
builder.Services.AddSingleton<IJsonProcessor, JsonProcessor>();
builder.Services.AddScoped<IMovieReviewService, MovieReviewService>();
builder.Services.AddScoped<IMovieService, MovieService>();
builder.Services.AddScoped<IMovieRepository, MovieRepository>();
builder.Services.AddScoped<IMovieReviewRepository, MovieReviewRepository>();


var dbHost = builder.Configuration["DB_HOST"];
var dbName = builder.Configuration["DB_NAME"];
var dbUser = builder.Configuration["DB_USER"];
var dbPass = builder.Configuration["DB_PASS"];

var connectionString = $"Host={dbHost};Database={dbName};Username={dbUser};Password={dbPass}";

builder.Services.AddDbContext<MovieReviewApiContext>(options =>
    options.UseNpgsql(connectionString));


builder.Services.Configure<OmdbSettings>( builder.Configuration.GetSection("Omdb"));
builder.Services.AddScoped<IOmdbApiService, OmdbApiService>();


var jwtKey = builder.Configuration["JWT_KEY"];
var jwtIssuer = builder.Configuration["JWT_ISSUER"];
var jwtAudience = builder.Configuration["JWT_AUDIENCE"];

var key = Encoding.UTF8.GetBytes(jwtKey);


builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };
    });


builder.Services.AddIdentityCore<IdentityUser>(options =>
    {
        options.User.RequireUniqueEmail = true;
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<MovieReviewApiContext>()
    .AddDefaultTokenProviders();




var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<MovieReviewApiContext>(); 
    db.Database.Migrate();

    await IdentitySeeder.SeedRolesAndAdmin(scope.ServiceProvider);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else 
    app.UseHttpsRedirection();


app.UseCors("FrontendPolicy");


app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();
app.MapControllers();

app.Run();