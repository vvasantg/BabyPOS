using Xunit;
using Moq;
using BabyPOS_API.Application.Services;
using BabyPOS_API.Infrastructure.Repositories;
using BabyPOS_API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BabyPOS_API.Tests
{
    public class OrderServiceTests
    {
        [Fact]
        public async Task CreateOrderAsync_ReturnsCreatedOrder()
        {
            var repoMock = new Mock<IOrderRepository>();
            var order = new Order { Id = 1, TableId = 1 };
            repoMock.Setup(r => r.AddAsync(order)).ReturnsAsync(order);
            var service = new OrderService(repoMock.Object);
            var result = await service.CreateOrderAsync(order);
            Assert.Equal(order, result);
        }

        [Fact]
        public async Task GetOrdersByTableAsync_ReturnsOrders()
        {
            var repoMock = new Mock<IOrderRepository>();
            var orders = new List<Order> { new Order { Id = 1, TableId = 1 } };
            repoMock.Setup(r => r.GetByTableAsync(1)).ReturnsAsync(orders);
            var service = new OrderService(repoMock.Object);
            var result = await service.GetOrdersByTableAsync(1);
            Assert.Single(result);
        }

        [Fact]
        public async Task GetOrderAsync_ReturnsOrderOrNull()
        {
            var repoMock = new Mock<IOrderRepository>();
            var order = new Order { Id = 1, TableId = 1 };
            repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(order);
            var service = new OrderService(repoMock.Object);
            var result = await service.GetOrderAsync(1);
            Assert.Equal(order, result);
        }

        [Fact]
        public async Task CloseOrderAsync_ReturnsClosedOrderOrNull()
        {
            var repoMock = new Mock<IOrderRepository>();
            var order = new Order { Id = 1, TableId = 1, IsClosed = true };
            repoMock.Setup(r => r.CloseAsync(1)).ReturnsAsync(order);
            var service = new OrderService(repoMock.Object);
            var result = await service.CloseOrderAsync(1);
            Assert.Equal(order, result);
        }

        [Fact]
        public async Task DeleteOrderAsync_ReturnsTrueOrFalse()
        {
            var repoMock = new Mock<IOrderRepository>();
            repoMock.Setup(r => r.DeleteAsync(1)).ReturnsAsync(true);
            var service = new OrderService(repoMock.Object);
            var result = await service.DeleteOrderAsync(1);
            Assert.True(result);
        }
    }
}
