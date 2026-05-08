using GroveStart.Model;

namespace GroveStart.Repository
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllAsync();
        Task<User?> GetByIdAsync(int id);
        Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
        Task <User> AddAsync(User user);
        Task<User> UpdateAsync(User user);
        Task DeleteByIdAsync(int id);
        Task SaveChangesAsync();
    }
}