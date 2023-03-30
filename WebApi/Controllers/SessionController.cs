using Microsoft.AspNetCore.Mvc;
using Services;

namespace WebApi.Controllers;

[Route("/sessions")]
public class SessionController : Controller
{
    private readonly SessionService _service;

    public SessionController(SessionService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromBody] string token)
    {
        return Json(await _service.FindJoinSession());
    }
}