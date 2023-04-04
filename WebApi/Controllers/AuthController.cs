using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using WebApi.Services;


namespace WebApi.Controllers;

public class AuthController : Controller
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginRequestModel loginModel)
    {
        return Json(await _authService.Login(loginModel));
    }
}