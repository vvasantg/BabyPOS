using Bunit;
using Xunit;
using BabyPOS_Web.Presentation.Pages;

namespace BabyPOS_Web.Tests
{
    public class RegisterTests : TestContext
    {
        [Fact]
        public void RegisterForm_Rendered()
        {
            var cut = RenderComponent<Register>();
            cut.Markup.Contains("Register");
            cut.Find("button").MarkupMatches("<button type=\"submit\" class=\"btn btn-primary\">Register</button>");
        }

        [Fact]
        public void RegisterForm_HasLoginLink()
        {
            var cut = RenderComponent<Register>();
            var link = cut.Find("a[href='/login']");
            Assert.NotNull(link);
        }
    }
}
