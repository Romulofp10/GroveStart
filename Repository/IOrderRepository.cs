using GroveStart.Model;

namespace GroveStart.Repository
{
    public interface IOrderRepository
    {
        Task<List<Order>> GetAllAsync();
        Task<Order?> GetByIdAsync(int id);
        Task AddAsync(Order order);
        Task DeleteByIdAsync(int id);
        Task SaveChangesAsync();
        
        // Métodos específicos do Order
        Task UpdateOrderAsync(Order order);
        Task<List<Order>> GetOrdersByPeriodAsync(Period period);
        Task<List<Order>> GetOrdersByUserIdAsync(int userId);
    }
}