using LoggingAndLogMonitoring.Api.Models;
using Microsoft.Extensions.Options;

namespace LoggingAndLogMonitoring.Api.Configurations
{
    public class EmailSettingSetup : IConfigureOptions<EmailSetting>
    {
        private readonly IConfiguration _configuration;

        public EmailSettingSetup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Configure(EmailSetting options)
        {
            options.ApiKey = Environment.GetEnvironmentVariable("SendGridApiKey", EnvironmentVariableTarget.Machine)!;
            _configuration.GetSection(nameof(EmailSetting)).Bind(options);
        }
    }
}
