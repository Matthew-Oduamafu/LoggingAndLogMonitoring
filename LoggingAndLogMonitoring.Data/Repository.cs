using LoggingAndLogMonitoring.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace LoggingAndLogMonitoring.Data
{
    public class Repository : IRepository
    {
        private readonly HangfireSendGridDbContext _dbContext;

        public Repository(HangfireSendGridDbContext dbContext)
        {
            _dbContext = dbContext;
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
            if (user == null) { return; }

            _dbContext.Remove(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<User>> GetUsersAsync()
        {
            return await _dbContext.Users.ToListAsync();
        }

        public User GetUser(int id)
        {
            return _dbContext.Users.FirstOrDefault(x => x.Id == id)!;
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == id)!;
        }

        public IReadOnlyList<User> GetUsers()
        {
            return _dbContext.Users.ToList();
        }
    }
}