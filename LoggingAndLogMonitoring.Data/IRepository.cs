using LoggingAndLogMonitoring.Data.Entities;

namespace LoggingAndLogMonitoring.Data
{
    public interface IRepository
    {
        Task<IReadOnlyList<User>> GetUsersAsync();

        Task<User> GetUserByIdAsync(int id);

        IReadOnlyList<User> GetUsers();

        User GetUser(int id);

        Task<int> AddUserAsync(User user, string propertyName);

        Task<int> UpdateAsync(User user, string propertyName);

        Task DeleteUserAsync(int id);
    }
}