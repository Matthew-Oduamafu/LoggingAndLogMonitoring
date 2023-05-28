namespace LoggingAndLogMonitoring.Domain.Models;

public class Email
{
    public string To { get; set; } = null!;
    public string ToName { get; set; } = null!;
    public string Subject { get; set; } = null!;
    public string PlainContent { get; set; } = null!;
    public string HtmlContent { get; set; } = null!;
}