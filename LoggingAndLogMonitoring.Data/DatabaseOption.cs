namespace LoggingAndLogMonitoring.Data
{
    public class DatabaseOption
    {
        public string ConnectionString { get; set; } = null!;
        public int MaxRetryCount { get; set; }
        public int MaxRetryDelay { get; set; }
        public IEnumerable<int>? ErrorNumbersToAdd { get; set; } = null;
        public int CommandTimeout { get; set; }
    }
}