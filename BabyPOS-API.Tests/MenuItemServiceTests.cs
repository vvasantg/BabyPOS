using Xunit;
using Moq;
using BabyPOS_API.Application.Services;
using BabyPOS_API.Infrastructure.Repositories;
using BabyPOS_API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BabyPOS_API.Tests
{
    public class MenuItemServiceTests
    {
        [Fact]
        public async Task CreateMenuItemAsync_ReturnsCreatedMenuItem()
        {
            var repoMock = new Mock<IMenuItemRepository>();
            var menuItem = new MenuItem { Id = 1, Name = "Test", Price = 100, ShopId = 1 };
            repoMock.Setup(r => r.AddAsync(menuItem)).ReturnsAsync(menuItem);
            var service = new MenuItemService(repoMock.Object);
            var result = await service.CreateMenuItemAsync(menuItem);
            Assert.Equal(menuItem, result);
        }


        [Fact]
        public async Task GetMenuItemAsync_ReturnsMenuItemOrNull()
        {
            var repoMock = new Mock<IMenuItemRepository>();
            var item = new MenuItem { Id = 1, Name = "A", Price = 10, ShopId = 1 };
            repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(item);
            var service = new MenuItemService(repoMock.Object);
            var result = await service.GetMenuItemAsync(1);
            Assert.Equal(item, result);
        }

        [Fact]
        public async Task UpdateMenuItemAsync_ReturnsUpdatedMenuItemOrNull()
        {
            var repoMock = new Mock<IMenuItemRepository>();
            var item = new MenuItem { Id = 1, Name = "A", Price = 10, ShopId = 1 };
            repoMock.Setup(r => r.UpdateAsync(1, item)).ReturnsAsync(item);
            var service = new MenuItemService(repoMock.Object);
            var result = await service.UpdateMenuItemAsync(1, item);
            Assert.Equal(item, result);
        }

        [Fact]
        public async Task DeleteMenuItemAsync_ReturnsTrueOrFalse()
        {
            var repoMock = new Mock<IMenuItemRepository>();
            repoMock.Setup(r => r.DeleteAsync(1)).ReturnsAsync(true);
            var service = new MenuItemService(repoMock.Object);
            var result = await service.DeleteMenuItemAsync(1);
            Assert.True(result);
        }
    }
}
