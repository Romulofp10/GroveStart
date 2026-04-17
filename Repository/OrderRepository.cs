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

        public void Update(Order order)
            => _dbSet.Update(order);

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

            if (order == null)
                throw new ArgumentNullException(nameof(order));

            var existingOrder = await GetByIdAsync(order.Id);
            if (existingOrder == null)
                throw new KeyNotFoundException("Order não encontrada");

            // Aplicar mudanças usando o método Update da classe
            existingOrder.Update( order.UserId,
                order.CustomerId,
                order.Title,
                order.Description,
                order.Period,
                order.StartDate,
                order.EndDate);
            Update(existingOrder);
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