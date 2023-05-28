using LoggingAndLogMonitoring.Api.Configurations;
using LoggingAndLogMonitoring.Data;
using LoggingAndLogMonitoring.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.ConfigureOptions<DatabaseOptionSetup>();
builder.Services.ConfigureOptions<EmailSettingSetup>();

builder.Services.AddDbContext<HangfireSendGridDbContext>((serviceProvider, dbContextOptionsBuilder) =>
{
    var databaseOptions = serviceProvider.GetService<IOptions<DatabaseOption>>()!.Value;

    dbContextOptionsBuilder.UseSqlServer(databaseOptions.ConnectionString,

        sqlOptionsAction =>
        {
            sqlOptionsAction.EnableRetryOnFailure(databaseOptions.MaxRetryCount, TimeSpan.FromSeconds(databaseOptions.MaxRetryDelay), databaseOptions.ErrorNumbersToAdd);
            sqlOptionsAction.CommandTimeout(databaseOptions.CommandTimeout);
        });

    dbContextOptionsBuilder.EnableSensitiveDataLogging(true);
    dbContextOptionsBuilder.EnableDetailedErrors(true);
});

builder.Services.AddTransient<IRepository, Repository>();
builder.Services.AddTransient<IUserLogic, UserLogic>();

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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();