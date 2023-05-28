using LoggingAndLogMonitoring.Data.Entities;
using LoggingAndLogMonitoring.Domain.Models;

namespace LoggingAndLogMonitoring.Domain;

public interface IEmailSenderService
{
    Task<bool> SendEmailAsync(Email email);

    Task<bool> SendEmailAsync(IReadOnlyList<User> users, string subject, string plaintext, string htmlContext);
}