using Microsoft.AspNetCore.Mvc;
using UniqueUuidApi.Services;

namespace UniqueUuidApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UuidController : Controller
{
    private readonly IUuidService _uuidService;
    
    public UuidController(IUuidService uuidService)
    {
        _uuidService = uuidService;
    }
    
    [HttpGet("id")]
    public IActionResult GenerateUuid()
    {
        var uuid = _uuidService.GetStoredUuid();
        return Ok(new { Uuid = uuid });
    }
}