using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Text;
using System.Text.RegularExpressions;
using Database.Repositories;
using Microsoft.EntityFrameworkCore;
using Models;
using Services.Exceptions;

namespace Services;

public class PlayerService
{
    private readonly PlayersCrudRepository _repository;
    private readonly byte[] _salt;
    private const string EmailRegexp = "^(([^<>()[\\].,;:\\s@\"]+(\\.[^<>()[\\].,;:\\s@\"]+)*)|(\".+\"))@(([^<>()[\\].,;:\\s@\"]+\\.)+[^<>()[\\].,;:\\s@\"]{2,})$";
    private const string PasswordRegexp = @"(?=.*[0-9])(?=.*[!@#$%^\&*])(?=.*[a-z])(?=.*[A-Z])[0-9a-zA-Z!@#$%^\&*]{8,}";

    public PlayerService(PlayersCrudRepository repository, string salt)
    {
        _repository = repository;
        _salt = Encoding.UTF8.GetBytes(salt);

        if (_salt.Length != 128 / 8)
        {
            throw new Exception("Wrong salt length");
        }
    }

    public async Task<PlayerModel> Register(string email, string password)
    {
        if (!Regex.IsMatch(email, EmailRegexp, RegexOptions.IgnoreCase))
        {
            throw new WrongEmailException(email);
        }
        if (!Regex.IsMatch(password, PasswordRegexp, RegexOptions.None))
        {
            throw new InvalidPasswordFormatException(password);
        }

        var existingPlayer = await _repository.GetQueryable().FirstOrDefaultAsync(player => player.Email == email);
        if (existingPlayer != null)
        {
            throw new EmailAlreadyRegisteredException(email);
        }

        var hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: _salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: 256 / 8));

        return await _repository.CreateAsync(id => new PlayerModel()
        {
            Id = id,
            Email = email,
            Password = hashed,
        });
    }

    public async Task<string> LogIn(string email, string password)
    {
        var player = await _repository.GetQueryable()                          
            .FirstOrDefaultAsync(player => player.Email == email);

        if (player == null)
        {
            throw new NotRegisterPlayerException(email);
        }

        if (player.Password != password)
        {
            throw new WrongPasswordException(password);
        }

        var token = Tokenize(email, password);
        return token;
    }

    public async Task<PlayerModel> Update(string token, PlayerModel source)
    {
        var player = await Detokenize(token);
        if (source.Email != player.Email)
        {
            throw new EmailCannotBeChangedException(player.Email);
        }
        if (!Regex.IsMatch(source.Password, PasswordRegexp, RegexOptions.Singleline))
        {
            throw new InvalidPasswordFormatException(source.Password);
        }

        return await _repository.UpdateAsync(source);
    }

    public async Task<PlayerModel> GetByToken(string token)
    {
        return await Detokenize(token);
    }

    private string Tokenize(string email, string password)
    {
        var credentials = $"{email}:{password}";
        var bytes = Encoding.UTF8.GetBytes(credentials);
        var token = Convert.ToBase64String(bytes);
        return token;
    }

    private async Task<PlayerModel> Detokenize(string token)
    {
        var bytes = Convert.FromBase64String(token);
        var credentials = Encoding.UTF8.GetString(bytes);
        var credentialsParts = credentials.Split(":");
        if (credentialsParts.Length != 2)
        {
            throw new InvalidTokenException(token);
        }

        var email = credentialsParts[0];
        var password = credentialsParts[1];
        var player = await _repository.GetQueryable()                         
            .FirstOrDefaultAsync(player => player.Email == email);

        if (player == null)
        {
            throw new NotRegisterPlayerException(email);
        }

        if (player.Password != password)
        {
            throw new WrongPasswordException(password);
        }

        return player;
    }
}