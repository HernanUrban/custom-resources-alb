using Microsoft.AspNetCore.Mvc;
using UniqueUuidApi.Services;

namespace UniqueUuidApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MemoryStatsController : Controller
{
    private readonly IMemoryStatsService _memoryStatsService;
    
    public MemoryStatsController(IMemoryStatsService memoryStatsService)
    {
        _memoryStatsService = memoryStatsService;
    }
    
    [HttpGet("memoryStatus")]
    public IActionResult MemoryStatus()
    {
        var memoryStats = _memoryStatsService.GetMemoryStats();
        return Ok(memoryStats);
    }
    
    [HttpPost("consume")]
    public IActionResult ConsumeMemory()
    {
        var memoryStats = _memoryStatsService.ConsumeMemory();
        return Ok(memoryStats);
    }
    
    [HttpPost("release")]
    public IActionResult ReleaseMemory()
    {
        var memoryStats = _memoryStatsService.ReleaseMemory();
        return Ok(memoryStats);
    }
}