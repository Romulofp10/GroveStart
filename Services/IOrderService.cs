using GroveStart.Model;
using GroveStart.Repository;

namespace GroveStart.Services
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(Order order);
        Task UpdateOrderAsync(Order order);
        Task DeleteOrderAsync(int id);
        Task<List<Order>> GetAllOrdersAsync();
        Task<Order?> GetOrderByIdAsync(int id);
        Task<List<Order>> GetOrdersByPeriodAsync(Period period);
        Task<List<Order>> GetOrdersByUserAsync(int userId);
    }
}