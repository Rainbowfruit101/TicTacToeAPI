using System.Text.RegularExpressions;
using Database.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Models;
using Services.Exceptions;

namespace Services;

public class PlayerService
{
    private readonly PlayersCrudRepository _repository;
    private readonly PasswordHasher<PlayerModel> _passwordHasher;

    private const string EmailRegexp =
        "^(([^<>()[\\].,;:\\s@\"]+(\\.[^<>()[\\].,;:\\s@\"]+)*)|(\".+\"))@(([^<>()[\\].,;:\\s@\"]+\\.)+[^<>()[\\].,;:\\s@\"]{2,})$";

    private const string PasswordRegexp = @"(?=.*[0-9])(?=.*[!@#$%^\&*])(?=.*[a-z])(?=.*[A-Z])[0-9a-zA-Z!@#$%^\&*]{8,}";

    public PlayerService(PlayersCrudRepository repository, string salt, PasswordHasher<PlayerModel> passwordHasher)
    {
        _repository = repository;
        _passwordHasher = passwordHasher;
    }

    public async Task<PlayerModel> Register(string email, string password)
    {
        if (!Regex.IsMatch(email, EmailRegexp, RegexOptions.IgnoreCase))
        {
            throw new InvalidEmailException(email);
        }

        if (!Regex.IsMatch(password, PasswordRegexp, RegexOptions.None))
        {
            throw new InvalidPasswordFormatException();
        }

        var existingPlayer = await _repository.GetQueryable().FirstOrDefaultAsync(player => player.Email == email);
        if (existingPlayer != null)
        {
            throw new EmailAlreadyRegisteredException(email);
        }

        return await _repository.CreateAsync(id => new PlayerModel()
        {
            Id = id,
            Email = email,
            Password = _passwordHasher.HashPassword(null, password),
        });
    }

    public async Task<PlayerModel> Update(PlayerModel source)
    {
        var player = await _repository.ReadAsync(source.Id);
        if (source.Email != player.Email)
        {
            throw new EmailCannotBeChangedException(player.Email);
        }

        if (!Regex.IsMatch(source.Password, PasswordRegexp, RegexOptions.Singleline))
        {
            throw new InvalidPasswordFormatException();
        }

        return await _repository.UpdateAsync(source);
    }

    public async Task<PlayerModel> ReadAsync(Guid id)
    {
        return await _repository.ReadAsync(id);
    }
}