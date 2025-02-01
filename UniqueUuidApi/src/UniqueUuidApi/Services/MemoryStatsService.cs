using UniqueUuidApi.Dtos;

namespace UniqueUuidApi.Services;

public class MemoryStatsService : IMemoryStatsService
{
    private readonly double _cpuPercentageUsage = 0.05;
    private int _availableMB = 4096;
    private int _usageMB = 0;
    private readonly int _limitMB = 3072;

    public MemoryStatsDTO GetMemoryStats()
    {
        // Create and return MemoryStatsDTO with current values
        return new MemoryStatsDTO
        {
            CpuPercentageUsage = _cpuPercentageUsage,
            AvailableMB = _availableMB,
            UsageMB = _usageMB,
            LimitMB = _limitMB
        };
    }

    public MemoryStatsDTO ConsumeMemory()
    {
        // Ensure memory usage does not exceed the limit
        if (_usageMB + 100 <= _limitMB)
        {
            _usageMB += 100;
            _availableMB -= 100;
        }

        // Return the updated stats
        return GetMemoryStats();
    }
    
    public MemoryStatsDTO ConsumeMemory(int memoryUsed)
    {
        // Ensure memory usage does not exceed the limit
        if (_usageMB + memoryUsed <= _limitMB)
        {
            _usageMB += memoryUsed;
            _availableMB -= memoryUsed;
        }

        // Return the updated stats
        return GetMemoryStats();
    }

    public MemoryStatsDTO ReleaseMemory()
    {
        // Ensure memory usage does not go below zero
        if (_usageMB - 100 >= 0)
        {
            _usageMB -= 100;
            _availableMB += 100;
        }

        // Return the updated stats
        return GetMemoryStats();
    }
}
