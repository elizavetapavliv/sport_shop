using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Moq;
using SportShop.Components;
using SportShop.Models;
using Xunit;

namespace SportShopTests
{
    public class NavigationMenuViewComponentTests
    {
        [Fact]
        public void CanSelectCategories()
        {
            Mock<IShopRepository> mock = new Mock<IShopRepository>();
            mock.Setup(m => m.Products).Returns(new[]
            {
                new Product {ProductID = 1, Name = "P1", Category = "Apples"},
                new Product {ProductID = 2, Name = "P2", Category = "Apples"},
                new Product {ProductID = 3, Name = "P3", Category = "Plums"},
                new Product {ProductID = 4, Name = "P4", Category = "Oranges"},
            }.AsQueryable());
            NavigationMenuViewComponent target =
                new NavigationMenuViewComponent(mock.Object);
            string[] results = ((IEnumerable<string>) ((ViewViewComponentResult) target.Invoke()).ViewData.Model)
                .ToArray();
            Assert.True(new[]
            {
                "Apples",
                "Oranges", 
                "Plums"
            }.SequenceEqual(results));
        }

        [Fact]
        public void IndicatesSelectedCategory()
        {
            string categoryToSelect = "Apples";
            Mock<IShopRepository> mock = new Mock<IShopRepository>();
            mock.Setup(m => m.Products).Returns(new[]
            {
                new Product {ProductID = 1, Name = "P1", Category = "Apples"},
                new Product {ProductID = 4, Name = "P2", Category = "Oranges"},
            }.AsQueryable());

            NavigationMenuViewComponent target = new NavigationMenuViewComponent(mock.Object)
            {
                ViewComponentContext = new ViewComponentContext
                {
                    ViewContext = new ViewContext {RouteData = new Microsoft.AspNetCore.Routing.RouteData()}
                }
            };
            target.RouteData.Values["category"] = categoryToSelect;
            string result = (string) (target.Invoke() as ViewViewComponentResult)?
                .ViewData["SelectedCategory"];

            Assert.Equal(categoryToSelect, result);
        }
    }
}
