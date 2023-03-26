using Database;
using Database.Repositories;
using Microsoft.EntityFrameworkCore;
using Models;
using Services;
using WebApi.Exceptions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<TicTacToeDbContext>(ConfigureDefaultConnection);

builder.Services.AddScoped<ICrudRepository<TicTacToeDbContext, SessionModel>, SessionsCrudRepository>();
builder.Services.AddScoped<ICrudRepository<TicTacToeDbContext, PlayerModel>, PlayersCrudRepository>();

builder.Services.AddScoped<SessionService>();
builder.Services.AddScoped<PlayerService>(provider =>
{
    var repository = provider.GetService<PlayersCrudRepository>();
    var salt = builder.Configuration.GetSection("Salts")?.GetValue<string>("PasswordSalt");
    if (salt==null)
    {
        throw new ServerStartException("Password salt not specified by path Salts.PasswordSalt");
    }
    return new PlayerService(repository, salt);
});

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

void ConfigureDefaultConnection(DbContextOptionsBuilder options)
{
    var connectionString = builder.Configuration.GetConnectionString("TicTacToeDatabase");
    
    if (string.IsNullOrWhiteSpace(connectionString))
    {
        Console.WriteLine("TicTacToeDatabase connection string is not set");
        return;
    }

    if (connectionString.StartsWith("$"))
    {
        var envValue = Environment.GetEnvironmentVariable(connectionString.TrimStart('$'));
        connectionString = string.IsNullOrWhiteSpace(envValue) ? string.Empty : envValue;
    }
    
    if (string.IsNullOrWhiteSpace(connectionString))
    {
        Console.WriteLine("TicTacToeDatabase connection string is not set");
        return;
    }
    
    options.UseNpgsql(connectionString);
}