namespace UniqueUuidApi.Services;

public class UuidService : IUuidService
{
    private readonly string _storedUuid;
    private readonly ILogger<UuidService> _logger;

    public UuidService(ILogger<UuidService> logger)
    {
        // Generate the UUID once during the app startup and store it
        _storedUuid = Guid.NewGuid().ToString();
        _logger = logger;
    }

    public string GetStoredUuid()
    {
        _logger.LogInformation("Retrieved UUID: {Uuid}", _storedUuid);
        return _storedUuid; // Return the same UUID for every request
    }
}