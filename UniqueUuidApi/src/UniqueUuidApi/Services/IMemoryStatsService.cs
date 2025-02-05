using UniqueUuidApi.Dtos;

namespace UniqueUuidApi.Services;

public interface IMemoryStatsService
{
    public MemoryStatsDTO GetMemoryStats();
    public MemoryStatsDTO ConsumeMemory();

    public MemoryStatsDTO ConsumeMemory(int memoryUsed);
    
    public MemoryStatsDTO ReleaseMemory();
}