namespace LoggingAndLogMonitoring.Data.Entities
{
    public class EmailMessage
    {
        public int Id { get; set; }
        public string Subject { get; set; } = null!;
        public string Body { get; set; } = null!;
        public string HtmlContext { get; set; } = null!;
    }
}