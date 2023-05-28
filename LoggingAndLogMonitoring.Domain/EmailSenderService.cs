using System.Net;
using LoggingAndLogMonitoring.Api.Models;
using LoggingAndLogMonitoring.Data.Entities;
using LoggingAndLogMonitoring.Domain.Models;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace LoggingAndLogMonitoring.Domain;

public class EmailSenderService : IEmailSenderService
{
    private readonly EmailSetting _emailSetting;

    public EmailSenderService(IOptions<EmailSetting> options)
    {
        _emailSetting = options.Value;
    }

    public async Task<bool> SendEmailAsync(Email email)
    {
        var client = new SendGridClient(_emailSetting.ApiKey);

        var to = new EmailAddress {Name = email.ToName, Email = email.To};
        var from = new EmailAddress {Name = _emailSetting.FromName, Email = _emailSetting.FromAddress};

        var msg = MailHelper.CreateSingleEmail(from, to, email.Subject,
            email.PlainContent, email.HtmlContent);
        var response = await client.SendEmailAsync(msg);

        if (response.IsSuccessStatusCode)
            return response.StatusCode is HttpStatusCode.Accepted or HttpStatusCode.OK;

        Console.WriteLine("Email wasn't sent");
        Console.WriteLine($"status code {response.StatusCode}");
        Console.WriteLine(response.ToString());

        return response.StatusCode is HttpStatusCode.Accepted or HttpStatusCode.OK;
    }

    public async Task<bool> SendEmailAsync(IReadOnlyList<User> users, string subject, string plaintext,
        string htmlContext)
    {
        var client = new SendGridClient(_emailSetting.ApiKey);

        var tos = users.Select(u => new EmailAddress {Name = u.Name, Email = u.EmailAddress});
        var from = new EmailAddress {Name = _emailSetting.FromName, Email = _emailSetting.FromAddress};

        var msg = MailHelper.CreateSingleEmailToMultipleRecipients(from, tos.ToList(), subject,
            plaintext, htmlContext);
        var response = await client.SendEmailAsync(msg);

        if (response.IsSuccessStatusCode)
            return response.StatusCode is HttpStatusCode.Accepted or HttpStatusCode.OK;

        Console.WriteLine("Email wasn't sent");
        Console.WriteLine($"status code {response.StatusCode}");
        Console.WriteLine(response.ToString());

        return response.StatusCode is HttpStatusCode.Accepted or HttpStatusCode.OK;
    }
}