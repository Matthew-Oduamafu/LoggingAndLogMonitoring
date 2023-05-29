using LoggingAndLogMonitoring.Data;
using LoggingAndLogMonitoring.Data.Entities;
using Microsoft.Extensions.Logging;

namespace LoggingAndLogMonitoring.Domain
{
    public class UserLogic : IUserLogic
    {
        private readonly ILogger<UserLogic> _logger;
        private readonly IRepository _repo;

        public UserLogic(IRepository repo, ILogger<UserLogic> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public async Task<int> AddUserAsync(User user, string propertyName)
        {
            return await _repo.AddUserAsync(user, propertyName);
        }

        public async Task DeleteUserAsync(int id)
        {
            await _repo.DeleteUserAsync(id);
        }

        public async Task<IReadOnlyList<User>> GetUsersAsync()
        {
            return await _repo.GetUsersAsync();
        }

        public User GetUser(int id)
        {
            _logger.LogDebug("Getting a single user in UserLogic for Id: ({Id})", id);

            return _repo.GetUser(id);
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            _logger.LogInformation("Getting User in UserLogic for Id {Id}", id);
            return await _repo.GetUserByIdAsync(id);
        }

        public IReadOnlyList<User> GetUsers()
        {
            return _repo.GetUsers();
        }

        public async Task<int> UpdateAsync(User user, string propertyName)
        {
            return await _repo.UpdateAsync(user, propertyName);
        }
    }
}