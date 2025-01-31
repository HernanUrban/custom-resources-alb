namespace UniqueUuidApi.Dtos;

public class MemoryStatsDTO
{
    public double CpuPercentageUsage { get; set; }
    public int AvailableMB { get; set; }
    public int UsageMB { get; set; }
    public int LimitMB { get; set; }

}