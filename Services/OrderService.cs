using GroveStart.Model;
using GroveStart.Repository;

namespace GroveStart.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUserService   _userService;

        public OrderService(IOrderRepository orderRepository, IUserService userService)
        {
            _orderRepository = orderRepository;
            _userService = userService;
        }

        public async Task<Order> CreateOrderAsync(Order order)
        {
          
          var existsUser =  await _userService.Get(order.UserId);

         var existsCustomer = await _userService.Get(order.CustomerId);
         
         if (existsCustomer is null || existsUser is null)
         {
            throw new InvalidDataException("Nao contem user e customer no banco de dados.");
         }

            var createdOrder = await _orderRepository.AddAsync(order);
            return order;
        }

        public async Task UpdateOrderAsync(Order order)
        {
            // Validações de negócio ficam aqui no Service
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            var existingOrder = await _orderRepository.GetByIdAsync(order.Id);
            if (existingOrder == null)
                throw new KeyNotFoundException("Pedido não encontrado");

            // Regra de negócio: não pode alterar período se já foi processado
            if (existingOrder.Period != order.Period)
            {
                // Exemplo: verificar status do pedido
                // if (existingOrder.Status == OrderStatus.Processed)
                //     throw new InvalidOperationException("Não pode alterar período de pedido processado");
            }

            // Regra: validar datas
            if (order.EndDate <= order.StartDate)
                throw new InvalidOperationException("Data de fim deve ser posterior à data de início");

            // Aplicar mudanças
            existingOrder.Update(
                order.UserId,
                order.CustomerId,
                order.Title,
                order.Description,
                order.Period,
                order.StartDate,
                order.EndDate
            );

            // Repository cuida apenas da persistência
            await _orderRepository.UpdateOrderAsync(existingOrder);
            await _orderRepository.SaveChangesAsync();
        }

        public async Task DeleteOrderAsync(int id)
        {
            // Validações de negócio no Service
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null)
                throw new KeyNotFoundException("Pedido não encontrado");

            // Regra: não pode deletar pedidos processados
            // if (order.Status == OrderStatus.Processed)
            //     throw new InvalidOperationException("Não pode deletar pedido processado");

            // Regra: verificar se tem dependências
            var ordersCount = await _orderRepository.GetOrdersByUserIdAsync(order.UserId);
            if (ordersCount.Count == 1)
                throw new InvalidOperationException("Não pode deletar o último pedido do usuário");

            // Repository cuida da exclusão física
            await _orderRepository.DeleteByIdAsync(id);
            await _orderRepository.SaveChangesAsync();
        }

        public async Task<List<Order>> GetAllOrdersAsync()
        {
            return await _orderRepository.GetAllAsync();
        }

        public async Task<Order?> GetOrderByIdAsync(int id)
        {
            return await _orderRepository.GetByIdAsync(id);
        }

        public async Task<List<Order>> GetOrdersByPeriodAsync(Period period)
        {
            return await _orderRepository.GetOrdersByPeriodAsync(period);
        }

        public async Task<List<Order>> GetOrdersByUserAsync(int userId)
        {
            return await _orderRepository.GetOrdersByUserIdAsync(userId);
        }
    }
}