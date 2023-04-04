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

    [HttpPatch("{id}")]
    public async Task<IActionResult> Update(string token, [FromBody] PlayerModel player)
    {
        return Json(await _service.Update(player));
    }

    [HttpGet("{id}")]
    public async Task<PlayerModel> GetUser(Guid id)
    {
        return await _service.ReadAsync(id);
    }
}