namespace UniqueUuidApi.Services;

public class UuidService : IUuidService
{
    private readonly string _storedUuid;
    private readonly ILogger<UuidService> _logger;
    private readonly IMemoryStatsService _memoryStatsService;
    private Random _rnd;

    public UuidService(ILogger<UuidService> logger, IMemoryStatsService memoryStatsService)
    {
        // Generate the UUID once during the app startup and store it
        _storedUuid = Guid.NewGuid().ToString();
        _logger = logger;
        _memoryStatsService = memoryStatsService;
        _rnd = new Random();
    }

    public string GetStoredUuid()
    {
        var mem  = _rnd.Next(1, 11);
        _memoryStatsService.ConsumeMemory(mem);
        _logger.LogInformation("Retrieved UUID: {Uuid}, memory used {mem}Mb", _storedUuid, mem);
        return _storedUuid; // Return the same UUID for every request
    }
}