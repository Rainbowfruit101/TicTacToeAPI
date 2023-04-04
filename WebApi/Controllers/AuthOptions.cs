using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace WebApi.Controllers;

public class AuthOptions
{
    public const string ISSUER = "TicTacToeServer"; // издатель токена
    public const string AUDIENCE = "TicTacToeClient"; // потребитель токена
    const string KEY = "mysupersecret_secretkey!123";   // ключ для шифрации
    public static SymmetricSecurityKey GetSymmetricSecurityKey() => 
        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
}