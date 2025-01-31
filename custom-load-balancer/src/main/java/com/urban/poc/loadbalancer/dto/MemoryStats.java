package com.urban.poc.loadbalancer.dto;

public class MemoryStats {

    private double cpuPercentageUsage;
    private long availableMB;
    private long usageMB;
    private long limitMB;

    // Getters and setters
    public double getCpuPercentageUsage() {
        return cpuPercentageUsage;
    }

    public void setCpuPercentageUsage(double cpuPercentageUsage) {
        this.cpuPercentageUsage = cpuPercentageUsage;
    }

    public long getAvailableMB() {
        return availableMB;
    }

    public void setAvailableMB(long availableMB) {
        this.availableMB = availableMB;
    }

    public long getUsageMB() {
        return usageMB;
    }

    public void setUsageMB(long usageMB) {
        this.usageMB = usageMB;
    }

    public long getLimitMB() {
        return limitMB;
    }

    public void setLimitMB(long limitMB) {
        this.limitMB = limitMB;
    }
}

