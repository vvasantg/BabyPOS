using Microsoft.EntityFrameworkCore;
using BabyPOS_API.Data;
using BabyPOS_API.Models;
using Xunit;

namespace BabyPOS_API.Tests
{
    public class SimpleDescriptionTest
    {
        [Fact]
        public void Shop_ShouldHave_DescriptionProperty()
        {
            // Arrange
            var shop = new Shop
            {
                Name = "Test Shop",
                Description = "Test Description"
            };

            // Act & Assert
            Assert.Equal("Test Description", shop.Description);
            Assert.NotNull(shop.Description);
        }

        [Fact]
        public void ShopWithFoodsDto_ShouldHave_DescriptionProperty()
        {
            // Arrange
            var dto = new ShopWithFoodsDto
            {
                Id = 1,
                Name = "Test Shop",
                Description = "Test Description",
                Foods = new List<MenuItemDto>()
            };

            // Act & Assert
            Assert.Equal("Test Description", dto.Description);
            Assert.NotNull(dto.Description);
        }

        [Fact]
        public void Shop_DefaultDescription_ShouldBeEmptyString()
        {
            // Arrange
            var shop = new Shop
            {
                Name = "Test Shop"
            };

            // Act & Assert
            Assert.Equal(string.Empty, shop.Description);
        }
    }
}
