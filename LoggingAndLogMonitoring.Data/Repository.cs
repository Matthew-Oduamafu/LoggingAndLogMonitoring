using System.Diagnostics;
using LoggingAndLogMonitoring.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LoggingAndLogMonitoring.Data
{
    public class Repository : IRepository
    {
        private readonly HangfireSendGridDbContext _dbContext;
        private readonly ILogger<Repository> _logger;
        private readonly ILogger _loggerFactory;

        public Repository(HangfireSendGridDbContext dbContext, ILogger<Repository> logger, ILoggerFactory loggerFactory)
        {
            _dbContext = dbContext;
            _logger = logger;
            _loggerFactory = loggerFactory.CreateLogger("DataAccessLayer");
        }

        public async Task<int> AddUserAsync(User user, string propertyName)
        {
            await _dbContext.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            return _dbContext.Entry(user).CurrentValues.GetValue<int>(propertyName);
        }

        public async Task<int> UpdateAsync(User user, string propertyName)
        {
            _dbContext.Entry(user).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();

            return _dbContext.Entry(user).CurrentValues.GetValue<int>(propertyName);
        }

        public async Task DeleteUserAsync(int id)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (user == null) return;

            _dbContext.Remove(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<User>> GetUsersAsync()
        {
            return await _dbContext.Users.ToListAsync();
        }

        public User GetUser(int id)
        {
            var timer = new Stopwatch();
            timer.Start();
            var user = _dbContext.Users.FirstOrDefault(x => x.Id == id)!;
            timer.Stop();

            _logger.LogDebug("Querying users for {Id} finished in {Milliseconds} milliseconds",
                id, timer.ElapsedMilliseconds
            );
            _loggerFactory.LogInformation("(F) Querying users for {Id} finished in {Ticks} ticks",
                id, timer.ElapsedTicks
            );

            return user;
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            _logger.LogInformation("Getting a single user in repository for Id: ({Id})", id);
            return await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == id)!;
        }

        public IReadOnlyList<User> GetUsers()
        {
            return _dbContext.Users.ToList();
        }
    }
}