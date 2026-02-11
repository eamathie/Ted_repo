using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using tedMovieApp;
using tedMovieApp.Repositories;
using tedMovieApp.Repositories.Interfaces;
using tedMovieApp.Services;
using tedMovieApp.Services.Interfaces;
using tedMovieApp.Tools;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IOmdbApiService, OmdbApiService>();
builder.Services.AddSingleton<IJsonProcessor, JsonProcessor>();
builder.Services.AddScoped<IMovieReviewService, MovieReviewService>();
builder.Services.AddScoped<IMovieService, MovieService>();
builder.Services.AddScoped<IMovieRepository, MovieRepository>();
builder.Services.AddScoped<IMovieReviewRepository, MovieReviewRepository>();


builder.Services.AddDbContext<MovieReviewApiContext>(options => options.UseNpgsql("Host=localhost;Port=5432;Database=tedmovieapp;User Id=postgres;Password=emilisverycool")); 

builder.Services.AddIdentity<IdentityUser, IdentityRole>() 
    .AddEntityFrameworkStores<MovieReviewApiContext>() 
    .AddDefaultUI() 
    .AddDefaultTokenProviders();


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    await IdentitySeeder.SeedRolesAndAdmin(scope.ServiceProvider);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();
app.MapControllers();

app.Run();