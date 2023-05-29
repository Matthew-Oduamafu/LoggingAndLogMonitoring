using System.Diagnostics;
using Hangfire;
using Hangfire.Storage.SQLite;
using HangfireBasicAuthenticationFilter;
using LoggingAndLogMonitoring.Api.Configurations;
using LoggingAndLogMonitoring.Data;
using LoggingAndLogMonitoring.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);


// specifying log entries destination
var path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
var tracePath = Path.Join(path, $"LoggingAndLogMonitoring_{DateTime.UtcNow:yyyyMMdd-HHmm}.txt");
Trace.Listeners.Add(new TextWriterTraceListener(File.CreateText(tracePath)));
Trace.AutoFlush = true;


// Add services to the container.
builder.Services.ConfigureOptions<DatabaseOptionSetup>();
builder.Services.ConfigureOptions<EmailSettingSetup>();

builder.Services.AddTransient<IUserLogic, UserLogic>();
builder.Services.ConfigureDomainService();

builder.Services.AddDbContext<HangfireSendGridDbContext>((serviceProvider, dbContextOptionsBuilder) =>
{
    var databaseOptions = serviceProvider.GetService<IOptions<DatabaseOption>>()!.Value;

    dbContextOptionsBuilder.UseSqlServer(databaseOptions.ConnectionString,
        sqlOptionsAction =>
        {
            sqlOptionsAction.EnableRetryOnFailure(databaseOptions.MaxRetryCount,
                TimeSpan.FromSeconds(databaseOptions.MaxRetryDelay), databaseOptions.ErrorNumbersToAdd);
            sqlOptionsAction.CommandTimeout(databaseOptions.CommandTimeout);
        });

    // dbContextOptionsBuilder.EnableSensitiveDataLogging();
    // dbContextOptionsBuilder.EnableDetailedErrors();
});

builder.Services.AddTransient<IRepository, Repository>();

builder.Services.AddHangfire(opt =>
    opt.UseSQLiteStorage(builder.Configuration.GetValue<string>("SqliteDb:SqliteDbName"))
);

builder.Services.AddHangfireServer();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapFallback(() => Results.Redirect("/swagger"));
app.UseHttpsRedirection();

app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    DashboardTitle = "Logging And Log Monitoring Background Jobs",
    Authorization = new[]
    {
        new HangfireCustomBasicAuthenticationFilter
        {
            Pass = "mattie",
            User = "Euler"
        }
    }
});

app.UseAuthorization();
app.MapControllers();

#pragma warning disable CS0618
// RecurringJob.AddOrUpdate<IServiceManagement>(x => x.SendBatchMail(), "0 */2 * ? * *");
#pragma warning restore CS0618

app.Run();