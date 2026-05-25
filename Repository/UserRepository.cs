using GroveStart.Infra;
using GroveStart.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GroveStart.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ConnectionContext _context;
        private readonly DbSet<User> _dbSet;
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(ConnectionContext context, ILogger<UserRepository> logger)
        {
            _context = context;
            _dbSet = _context.Set<User>();
            _logger = logger;
        }

        public async Task<List<User>> GetAllAsync()
            => await _dbSet.ToListAsync();

        public async Task<User?> GetByIdAsync(int id)
            => await _dbSet.FindAsync(id);

        /// <summary>Busca por e-mail (comparação case-insensitive). Repositório só consulta; a regra “duplicado” fica no serviço.</summary>
        public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            var normalized = email?.Trim() ?? string.Empty;
            if (normalized.Length == 0)
                return null;
            return await _dbSet
                .Where(u => u.Email.ToLower() == normalized.ToLower())
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<User> AddAsync(User user)
        {

            try
            {
                var entity = (await _dbSet.AddAsync(user)).Entity;
                var affected = await _context.SaveChangesAsync();
                _logger.LogInformation(
                    "UserRepository.AddAsync: SaveChanges concluído. Linhas afetadas={Affected}, Id gerado={UserId}",
                    affected,
                    entity.Id);
                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "UserRepository.AddAsync falhou ao persistir usuário Email={Email}", user.Email);
                throw;
            }
        }

        public async Task DeleteByIdAsync(int id)
        {
            var user = await GetByIdAsync(id);
            _dbSet.Remove(user);
            await SaveChangesAsync();
        }

        public async Task SaveChangesAsync()
            => await _context.SaveChangesAsync();

        public async Task<User> UpdateAsync(User user)
        {
            _dbSet.Update(user);
            await SaveChangesAsync();
            return user;
        }
    }
}
