using Xunit;
using Moq;
using BabyPOS_API.Application.Services;
using BabyPOS_API.Infrastructure.Repositories;
using BabyPOS_API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BabyPOS_API.Tests.Services
{
    public class TableServiceTests
    {
        [Fact]
        public async Task CreateTableAsync_ReturnsCreatedTable()
        {
            var repoMock = new Mock<ITableRepository>();
            var table = new Table { Id = 1, Name = "T1", ShopId = 1 };
            repoMock.Setup(r => r.AddAsync(table)).ReturnsAsync(table);
            var service = new TableService(repoMock.Object);
            var result = await service.CreateTableAsync(table);
            Assert.Equal(table, result);
        }

        [Fact]
        public async Task GetTablesByShopAsync_ReturnsTables()
        {
            var repoMock = new Mock<ITableRepository>();
            var tables = new List<Table> { new Table { Id = 1, Name = "T1", ShopId = 1 } };
            // repoMock.Setup(r => r.GetByShopAsync(1)).ReturnsAsync(tables);
            var service = new TableService(repoMock.Object);
            var result = await service.GetTablesByShopAsync(1);
            Assert.Single(result);
        }

        [Fact]
        public async Task GetTableAsync_ReturnsTableOrNull()
        {
            var repoMock = new Mock<ITableRepository>();
            var table = new Table { Id = 1, Name = "T1", ShopId = 1 };
            repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(table);
            var service = new TableService(repoMock.Object);
            var result = await service.GetTableAsync(1);
            Assert.Equal(table, result);
        }

        [Fact]
        public async Task UpdateTableAsync_ReturnsUpdatedTableOrNull()
        {
            var repoMock = new Mock<ITableRepository>();
            var table = new Table { Id = 1, Name = "T1", ShopId = 1 };
            repoMock.Setup(r => r.UpdateAsync(1, table)).ReturnsAsync(table);
            var service = new TableService(repoMock.Object);
            var result = await service.UpdateTableAsync(1, table);
            Assert.Equal(table, result);
        }

        [Fact]
        public async Task DeleteTableAsync_ReturnsTrueOrFalse()
        {
            var repoMock = new Mock<ITableRepository>();
            repoMock.Setup(r => r.DeleteAsync(1)).ReturnsAsync(true);
            var service = new TableService(repoMock.Object);
            var result = await service.DeleteTableAsync(1);
            Assert.True(result);
        }
    }
}
