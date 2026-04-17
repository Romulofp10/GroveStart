using GroveStart.Model;

namespace GroveStart.Repository
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllAsync();
        Task<User?> GetByIdAsync(int id);
        Task <User> AddAsync(User user);
        void  Update(User user);
        void Delete(User user);
        Task DeleteByIdAsync(int id);
        Task SaveChangesAsync();
    }
}