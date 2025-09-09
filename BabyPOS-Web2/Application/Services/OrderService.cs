using BabyPOS_Web2.Application.DTOs;
using BabyPOS_Web2.Infrastructure.Services;

namespace BabyPOS_Web2.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IApiService _apiService;

        public OrderService(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<List<OrderDto>> GetOrdersAsync(int shopId)
        {
            return await _apiService.GetOrdersAsync(shopId);
        }

        public async Task<List<OrderDto>> GetPendingOrdersAsync(int shopId)
        {
            var allOrders = await _apiService.GetOrdersByStatusAsync(shopId, "Pending");
            var inProgressOrders = await _apiService.GetOrdersByStatusAsync(shopId, "InProgress");
            
            var pendingOrders = new List<OrderDto>();
            pendingOrders.AddRange(allOrders);
            pendingOrders.AddRange(inProgressOrders);
            
            return pendingOrders.OrderBy(o => o.CreatedAt).ToList();
        }

        public async Task<List<OrderDto>> GetReadyOrdersAsync(int shopId)
        {
            return await _apiService.GetOrdersByStatusAsync(shopId, "Ready");
        }

        public async Task<OrderDto?> CreateOrderAsync(CreateOrderDto order)
        {
            return await _apiService.CreateOrderAsync(order);
        }

        public async Task<bool> UpdateOrderStatusAsync(int orderId, string status)
        {
            var statusUpdate = new UpdateOrderStatusDto { Status = status };
            return await _apiService.UpdateOrderStatusAsync(orderId, statusUpdate);
        }

        public async Task<bool> MarkOrderAsReadyAsync(int orderId)
        {
            return await UpdateOrderStatusAsync(orderId, "Ready");
        }

        public async Task<bool> MarkOrderAsCompletedAsync(int orderId)
        {
            return await UpdateOrderStatusAsync(orderId, "Completed");
        }
    }
}
