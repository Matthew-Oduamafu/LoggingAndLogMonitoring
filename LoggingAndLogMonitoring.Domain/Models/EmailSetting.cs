namespace LoggingAndLogMonitoring.Api.Models
{
    public class EmailSetting
    {
        public string ApiKey { get; set; } = null!;
        public string FromName { get; set; } = null!;
        public string FromAddress { get; set; } = null!;
    }
}