using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Services;

public class PasswordHashService
{
    private readonly byte[] _salt;

    public PasswordHashService(string salt)
    {
        _salt = Encoding.UTF8.GetBytes(salt);

        if (_salt.Length != 128 / 8)
        {
            throw new Exception("Wrong salt length");
        }
    }
    
    private string Hash(string password)
    {
        var hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: _salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8
            )
        );

        return hashed;
    }
}