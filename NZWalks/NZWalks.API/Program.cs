using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Inject Dependency to our db 
builder.Services.AddDbContext<NZWalksDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("NZWalks"));
});

// Whenever we ask for the IRegionRepository interface,
// give me the implementation in RegionRepository
builder.Services.AddScoped<IRegionRepository, RegionRepository>();

// Whenever we ask for the IWalksRepository interface,
// give me the implementation in WalksRepository
builder.Services.AddScoped<IWalksRepository, WalksRepository>();

// Whenever we ask for the IWalkDifficultyRepository interface,
// give me the implementation in WalkDifficultyRepository
builder.Services.AddScoped<IWalkDifficultyRepository, WalkDifficultyRepository>();


builder.Services.AddAutoMapper(typeof(Program).Assembly);
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
