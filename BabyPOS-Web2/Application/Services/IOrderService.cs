using BabyPOS_Web2.Application.DTOs;

namespace BabyPOS_Web2.Application.Services
{
    public interface IOrderService
    {
        Task<List<OrderDto>> GetOrdersAsync(int shopId);
        Task<List<OrderDto>> GetPendingOrdersAsync(int shopId);
        Task<List<OrderDto>> GetReadyOrdersAsync(int shopId);
        Task<OrderDto?> CreateOrderAsync(CreateOrderDto order);
        Task<bool> UpdateOrderStatusAsync(int orderId, string status);
        Task<bool> MarkOrderAsReadyAsync(int orderId);
        Task<bool> MarkOrderAsCompletedAsync(int orderId);
    }
}
