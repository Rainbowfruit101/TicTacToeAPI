using Database;
using Database.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Models;
using Services;
using WebApi.Controllers;
using WebApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<TicTacToeDbContext>(ConfigureDefaultConnection);

builder.Services.AddScoped<ICrudRepository<TicTacToeDbContext, SessionModel>, SessionsCrudRepository>();
builder.Services.AddScoped<ICrudRepository<TicTacToeDbContext, PlayerModel>, PlayersCrudRepository>();

builder.Services.AddScoped<IPasswordHasher<PlayerModel>, PasswordHasher<PlayerModel>>();

builder.Services.AddScoped<SessionService>();
builder.Services.AddScoped<PlayerService>();
builder.Services.AddScoped<AuthService>(provider =>
    {
        var repository = provider.GetService<ICrudRepository<TicTacToeDbContext, PlayerModel>>();
        if (repository == null)
        {
            throw new Exception();
        }

        var tokenExpirationTimeMin = builder.Configuration
            .GetSection("Auth")?
            .GetValue<long>("TokenExpirationTimeMin");
        if (tokenExpirationTimeMin == null)
        {
            throw new Exception();
        }

        var passwordHasher = provider.GetService<IPasswordHasher<PlayerModel>>();
        if (passwordHasher == null)
        {
            throw new Exception();
        }

        return new AuthService(tokenExpirationTimeMin.Value, repository, passwordHasher);
    }
);

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            // указывает, будет ли валидироваться издатель при валидации токена
            ValidateIssuer = true,
            // строка, представляющая издателя
            ValidIssuer = AuthOptions.ISSUER,
            // будет ли валидироваться потребитель токена
            ValidateAudience = true,
            // установка потребителя токена
            ValidAudience = AuthOptions.AUDIENCE,
            // будет ли валидироваться время существования
            ValidateLifetime = true,
            // установка ключа безопасности
            IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
            // валидация ключа безопасности
            ValidateIssuerSigningKey = true,
        };
    });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
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