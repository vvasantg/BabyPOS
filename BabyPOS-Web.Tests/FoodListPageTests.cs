using Xunit;
using Bunit;
using BabyPOS_Web.Pages;

namespace BabyPOS_Web.Tests;

public class FoodListPageTests : TestContext
{
    [Fact]
    public void FoodList_Rendered_ShouldShowAddButton()
    {
        // Arrange & Act
        var cut = RenderComponent<FoodList>(parameters => parameters.Add(p => p.ShopId, 1));
        // Assert
        cut.Markup.Contains("เพิ่มรายการอาหาร");
    }
}
