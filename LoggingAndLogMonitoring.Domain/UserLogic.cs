using LoggingAndLogMonitoring.Data;
using LoggingAndLogMonitoring.Data.Entities;

namespace LoggingAndLogMonitoring.Domain
{
    public class UserLogic : IUserLogic
    {
        private readonly IRepository _repo;

        public UserLogic(IRepository repo)
        {
            _repo = repo;
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
            return _repo.GetUser(id);
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
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