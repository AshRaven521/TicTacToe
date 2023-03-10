using DrfLikePaginations;
using Microsoft.EntityFrameworkCore;
using TicTacToe.BusinessLogic;
using TicTacToe.Data;
using TicTacToe.DataAccessLayer;
using TicTacToe.Mapping;
using TicTacToe.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContextPool<ApplicationDbContext>(opt =>
                    opt.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
                    ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))));

builder.Services.AddAutoMapper(typeof(MappingProfiles).Assembly);

builder.Services.AddScoped<IPlayerDAL, PlayerDAL>();
builder.Services.AddScoped<IGameDAL, GameDAL>();
builder.Services.AddScoped<IBoardDAL, BoardDAL>();

builder.Services.AddScoped<IGameService, GameService>();
builder.Services.AddScoped<IBoardDealer, BoardDealer>();
builder.Services.AddScoped<IBoardJudge, BoardJudge>();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
