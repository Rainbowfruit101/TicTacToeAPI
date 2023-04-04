using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Models;
using Services.Exceptions;
using WebApi.Controllers;
using WebApi.Exceptions;
using WebApi.Models;

namespace WebApi.Services;

public class AuthService
{
    private readonly long _tokenExpirationTimeMin;
    private readonly ICrudRepository<TicTacToeDbContext, PlayerModel> _repository;
    private readonly IPasswordHasher<PlayerModel> _passwordHasher;

    public AuthService(long tokenExpirationTimeMin, ICrudRepository<TicTacToeDbContext, PlayerModel> repository,
        IPasswordHasher<PlayerModel> passwordHasher)
    {
        _tokenExpirationTimeMin = tokenExpirationTimeMin;
        _repository = repository;
        _passwordHasher = passwordHasher;
    }

    public async Task<LoginResponseModel> Login(LoginRequestModel loginModel)
    {
        var player = _repository.GetQueryable()
            .FirstOrDefault(player => player.Email == loginModel.Login);
        if (player == null)
        {
            throw new NotRegisteredPlayerException(loginModel.Login);
        }

        if (_passwordHasher.VerifyHashedPassword(player, player.Password, loginModel.Password) ==
            PasswordVerificationResult.Failed)
        {
            throw new WrongPasswordException();
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, loginModel.Login)
        };

        // создаем JWT-токен
        var jwt = new JwtSecurityToken(
            issuer: AuthOptions.ISSUER,
            audience: AuthOptions.AUDIENCE,
            claims: claims,
            expires: DateTime.UtcNow.Add(TimeSpan.FromMilliseconds(_tokenExpirationTimeMin)),
            signingCredentials: new SigningCredentials(
                AuthOptions.GetSymmetricSecurityKey(),
                SecurityAlgorithms.HmacSha256
            )
        );

        return new LoginResponseModel()
        {
            Login = loginModel.Login,
            Token = new JwtSecurityTokenHandler().WriteToken(jwt)
        };
    }
}