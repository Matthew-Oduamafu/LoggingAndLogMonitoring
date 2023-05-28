using Microsoft.Extensions.DependencyInjection;

namespace LoggingAndLogMonitoring.Domain;

public static class DomainServiceRegistration
{
    public static IServiceCollection ConfigureDomainService(this IServiceCollection service)
    {
        service.AddTransient<IEmailSenderService, EmailSenderService>();
        service.AddTransient<IServiceManagement, ServiceManagement>();

        return service;
    }
}