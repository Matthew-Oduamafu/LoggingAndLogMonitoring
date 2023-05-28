using LoggingAndLogMonitoring.Domain.Models;

namespace LoggingAndLogMonitoring.Domain;

public class ServiceManagement : IServiceManagement
{
    private readonly IEmailSenderService _emailSenderService;
    private readonly IUserLogic _userLogic;

    public ServiceManagement(IEmailSenderService emailSenderService, IUserLogic userLogic)
    {
        _emailSenderService = emailSenderService;
        _userLogic = userLogic;
    }

    public async Task SendBatchMail()
    {
        var users = await _userLogic.GetUsersAsync();

        var response = await _emailSenderService.SendEmailAsync(
            users,
            "Monthly Subscription News Letter",
            EmailTemplates.HtmlEmailTemplate,
            EmailTemplates.HtmlEmailTemplate
        );

        if (!response) Console.WriteLine("Batch mail failed to send");
    }
}