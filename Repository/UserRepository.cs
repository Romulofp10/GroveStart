using GroveStart.Infra;
using GroveStart.Model;
using Microsoft.EntityFrameworkCore;

namespace GroveStart.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ConnectionContext _context;
        private readonly DbSet<User> _dbSet;

        public UserRepository(ConnectionContext context)
        {
            _context = context;
            _dbSet = _context.Set<User>();
        }

        public async Task<List<User>> GetAllAsync()
            => await _dbSet.ToListAsync();

        public async Task<User?> GetByIdAsync(int id)
            => await _dbSet.FindAsync(id);

public async Task<User> AddAsync(User user)
{
    Console.WriteLine("Dentro do repository");
    try
    {
        return (await _dbSet.AddAsync(user)).Entity;
    }
    catch (System.Exception error)
    {
        Console.WriteLine("error", error);
        throw;
    }
}

        public void Update(User user)
            => _dbSet.Update(user);

        public void Delete(User user)
            => _dbSet.Remove(user);

        public async Task DeleteByIdAsync(int id)
        {
            var user = await GetByIdAsync(id);
            if (user != null)
                Delete(user);
        }

        public async Task SaveChangesAsync()
            => await _context.SaveChangesAsync();

    }
}
