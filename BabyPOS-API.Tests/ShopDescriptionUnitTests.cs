using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;
using BabyPOS_API.Controllers;
using BabyPOS_API.Data;
using BabyPOS_API.Models;
using System.Security.Claims;

namespace BabyPOS_API.Tests
{
    public class ShopDescriptionUnitTests : IDisposable
    {
        private readonly AppDbContext _context;
        private readonly ShopsController _controller;

        public ShopDescriptionUnitTests()
        {
            // Setup in-memory database
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: System.Guid.NewGuid().ToString())
                .Options;
            
            _context = new AppDbContext(options);
            _controller = new ShopsController(_context);
        }

        [Fact(DisplayName = "Shop model should have Description property")]
        public void Shop_ShouldHave_DescriptionProperty()
        {
            // Arrange & Act
            var shop = new Shop();
            
            // Assert
            Assert.NotNull(shop.Description);
            Assert.Equal(string.Empty, shop.Description);
            
            // Test setting description
            shop.Description = "Test description";
            Assert.Equal("Test description", shop.Description);
        }

        [Fact(DisplayName = "Shop can be created with description")]
        public async Task CreateShop_WithDescription_SavesCorrectly()
        {
            // Arrange
            var user = new User { Username = "testowner", Email = "test@mail.com" };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var shop = new Shop 
            { 
                Name = "Test Shop", 
                Description = "A wonderful test shop with great food",
                OwnerId = user.Id 
            };

            // Act
            _context.Shops.Add(shop);
            await _context.SaveChangesAsync();

            // Assert
            var savedShop = await _context.Shops.FirstOrDefaultAsync(s => s.Name == "Test Shop");
            Assert.NotNull(savedShop);
            Assert.Equal("Test Shop", savedShop.Name);
            Assert.Equal("A wonderful test shop with great food", savedShop.Description);
            Assert.Equal(user.Id, savedShop.OwnerId);
        }

        [Fact(DisplayName = "Shop description can be empty")]
        public async Task CreateShop_WithEmptyDescription_SavesCorrectly()
        {
            // Arrange
            var user = new User { Username = "testowner2", Email = "test2@mail.com" };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var shop = new Shop 
            { 
                Name = "Shop No Description", 
                Description = "", // Empty description
                OwnerId = user.Id 
            };

            // Act
            _context.Shops.Add(shop);
            await _context.SaveChangesAsync();

            // Assert
            var savedShop = await _context.Shops.FirstOrDefaultAsync(s => s.Name == "Shop No Description");
            Assert.NotNull(savedShop);
            Assert.Equal(string.Empty, savedShop.Description);
        }

        [Fact(DisplayName = "UpdateShopMain preserves OwnerId and updates Description")]
        public async Task UpdateShopMain_PreservesOwnerId_UpdatesDescription()
        {
            // Arrange
            var user = new User { Username = "shopowner", Email = "owner@mail.com" };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var originalShop = new Shop 
            { 
                Name = "Original Shop", 
                Description = "Original description",
                OwnerId = user.Id 
            };
            _context.Shops.Add(originalShop);
            await _context.SaveChangesAsync();

            // Setup controller user context
            SetupControllerUser(user.Id.ToString());

            var updateShop = new Shop 
            { 
                Id = originalShop.Id,
                Name = "Updated Shop", 
                Description = "Updated description",
                OwnerId = 99999 // This should be ignored
            };

            // Act
            var result = await _controller.UpdateShopMain(originalShop.Id, updateShop);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var updatedShop = Assert.IsType<Shop>(okResult.Value);
            
            Assert.Equal("Updated Shop", updatedShop.Name);
            Assert.Equal("Updated description", updatedShop.Description);
            Assert.Equal(user.Id, updatedShop.OwnerId); // Should preserve original owner
        }

        [Fact(DisplayName = "UpdateShop management endpoint preserves OwnerId and updates Description")]
        public async Task UpdateShop_ManagementEndpoint_PreservesOwnerId_UpdatesDescription()
        {
            // Arrange
            var user = new User { Username = "manageowner", Email = "manage@mail.com" };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var originalShop = new Shop 
            { 
                Name = "Manage Shop", 
                Description = "Manage description",
                OwnerId = user.Id 
            };
            _context.Shops.Add(originalShop);
            await _context.SaveChangesAsync();

            // Setup controller user context
            SetupControllerUser(user.Id.ToString());

            var updateShop = new Shop 
            { 
                Id = originalShop.Id,
                Name = "Updated Manage Shop", 
                Description = "Updated manage description",
                OwnerId = 88888 // This should be ignored
            };

            // Act
            var result = await _controller.UpdateShop(originalShop.Id, updateShop);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var updatedShop = Assert.IsType<Shop>(okResult.Value);
            
            Assert.Equal("Updated Manage Shop", updatedShop.Name);
            Assert.Equal("Updated manage description", updatedShop.Description);
            Assert.Equal(user.Id, updatedShop.OwnerId); // Should preserve original owner
        }

        [Fact(DisplayName = "Update shop fails when user is not owner")]
        public async Task UpdateShop_FailsWhen_UserIsNotOwner()
        {
            // Arrange
            var owner = new User { Username = "realowner", Email = "real@mail.com" };
            var notOwner = new User { Username = "notowner", Email = "not@mail.com" };
            _context.Users.AddRange(owner, notOwner);
            await _context.SaveChangesAsync();

            var shop = new Shop 
            { 
                Name = "Owner's Shop", 
                Description = "Owner's description",
                OwnerId = owner.Id 
            };
            _context.Shops.Add(shop);
            await _context.SaveChangesAsync();

            // Setup controller with different user (not owner)
            SetupControllerUser(notOwner.Id.ToString());

            var updateShop = new Shop 
            { 
                Id = shop.Id,
                Name = "Hacked Shop", 
                Description = "Hacked description",
                OwnerId = notOwner.Id
            };

            // Act
            var result = await _controller.UpdateShopMain(shop.Id, updateShop);

            // Assert
            var forbidResult = Assert.IsType<ForbidResult>(result);
            
            // Verify shop was not changed
            var unchangedShop = await _context.Shops.FindAsync(shop.Id);
            Assert.Equal("Owner's Shop", unchangedShop.Name);
            Assert.Equal("Owner's description", unchangedShop.Description);
            Assert.Equal(owner.Id, unchangedShop.OwnerId);
        }

        [Fact(DisplayName = "Shop description supports Thai characters")]
        public async Task Shop_DescriptionSupportsThai_Characters()
        {
            // Arrange
            var user = new User { Username = "thaiuser", Email = "thai@mail.com" };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var thaiDescription = "ร้านอาหารไทยแสนอร่อย บริการดี ราคาย่อมเยา มีเมนูหลากหลาย ต้มยำกุ้ง ผัดไทย ส้มตำ";
            var shop = new Shop 
            { 
                Name = "ร้านอาหารไทย", 
                Description = thaiDescription,
                OwnerId = user.Id 
            };

            // Act
            _context.Shops.Add(shop);
            await _context.SaveChangesAsync();

            // Assert
            var savedShop = await _context.Shops.FirstOrDefaultAsync(s => s.Name == "ร้านอาหารไทย");
            Assert.NotNull(savedShop);
            Assert.Equal(thaiDescription, savedShop.Description);
        }

        [Fact(DisplayName = "Shop description supports long text")]
        public async Task Shop_DescriptionSupports_LongText()
        {
            // Arrange
            var user = new User { Username = "longuser", Email = "long@mail.com" };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var longDescription = string.Join(" ", Enumerable.Repeat("This is a very long description text.", 50));
            var shop = new Shop 
            { 
                Name = "Long Description Shop", 
                Description = longDescription,
                OwnerId = user.Id 
            };

            // Act
            _context.Shops.Add(shop);
            await _context.SaveChangesAsync();

            // Assert
            var savedShop = await _context.Shops.FirstOrDefaultAsync(s => s.Name == "Long Description Shop");
            Assert.NotNull(savedShop);
            Assert.Equal(longDescription, savedShop.Description);
            Assert.True(savedShop.Description.Length > 1000); // Verify it's long
        }

        private void SetupControllerUser(string userId)
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim("sub", userId)
            }, "mock"));

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
