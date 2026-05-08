using GroveStart.Infra;
using GroveStart.Model;
using Microsoft.EntityFrameworkCore;

namespace GroveStart.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ConnectionContext _context;
        private readonly DbSet<Order> _dbSet;

        public OrderRepository(ConnectionContext context)
        {
            _context = context;
            _dbSet = _context.Set<Order>();
        }

        public async Task<List<Order>> GetAllAsync()
            => await _dbSet.ToListAsync();

        public async Task<Order?> GetByIdAsync(int id)
            => await _dbSet.FindAsync(id);

        public async Task AddAsync(Order order)
            => await _dbSet.AddAsync(order);

        public void Delete(Order order)
            => _dbSet.Remove(order);

        public async Task DeleteByIdAsync(int id)
        {
            var order = await GetByIdAsync(id);
            if (order != null)
                Delete(order);
        }

        public async Task SaveChangesAsync()
            => await _context.SaveChangesAsync();

        public async Task UpdateOrderAsync(Order order)
        {
            // Apenas lógica de acesso a dados
            // Validações ficam no Service Layer
            _dbSet.Update(order);
        }

        public async Task<List<Order>> GetOrdersByPeriodAsync(Period period)
        {
            return await _dbSet
                .Where(o => o.Period == period)
                .ToListAsync();
        }

        public async Task<List<Order>> GetOrdersByUserIdAsync(int userId)
        {
            return await _dbSet
                .Where(o => o.UserId == userId)
                .ToListAsync();
        }
    }
}