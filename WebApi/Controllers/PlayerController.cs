using Microsoft.AspNetCore.Mvc;
using Models;
using Services;

namespace WebApi.Controllers;

[Route("/players")]
public class PlayerController : Controller
{
    private readonly PlayerService _service;
    public PlayerController(PlayerService service)
    {
        _service = service;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string token)
    {
        return Json(await _service.GetByToken(token));
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] string email, string password)
    {
        return Json(await _service.Register(email, password));
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> Update(string token, [FromBody] PlayerModel player)
    {
        return Json(await _service.Update(token, player));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string token)
    {
        return Json(await _service.Delete(token));
    }
}