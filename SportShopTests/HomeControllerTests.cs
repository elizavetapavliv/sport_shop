using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SportShop.Controllers;
using SportShop.Models;
using SportShop.Models.ViewModels;
using Xunit;

namespace SportShopTests
{
    public class HomeControllerTests
    {
        [Fact]
        public void CanUseRepository()
        {
            Mock<IShopRepository> mock = new Mock<IShopRepository>();
            mock.Setup(m => m.Products)
                .Returns(new[]
                {
                    new Product {ProductID = 1, Name = "P1"}, new Product {ProductID = 2, Name = "P2"}
                }.AsQueryable());
            HomeController controller = new HomeController(mock.Object);

            if (controller.Index(null).ViewData.Model is ProductsListViewModel result)
            {
                Product[] prodArray = result.Products.ToArray();
                Assert.True(prodArray.Length == 2);
                Assert.Equal("P1", prodArray[0].Name);
                Assert.Equal("P2", prodArray[1].Name);
            }
        }

        [Fact]
        public void CanPaginate()
        {
            Mock<IShopRepository> mock = new Mock<IShopRepository>();
            mock.Setup(m => m.Products)
                .Returns(new[]
                {
                    new Product {ProductID = 1, Name = "P1"}, new Product {ProductID = 2, Name = "P2"},
                    new Product {ProductID = 3, Name = "P3"}, new Product {ProductID = 4, Name = "P4"},
                    new Product {ProductID = 5, Name = "P5"}
                }.AsQueryable());
            HomeController controller = new HomeController(mock.Object) {PageSize = 3};
            if (controller.Index(null, 2).ViewData.Model is ProductsListViewModel result)
            {
                Product[] prodArray = result.Products.ToArray();
                Assert.True(prodArray.Length == 2);
                Assert.Equal("P4", prodArray[0].Name);
                Assert.Equal("P5", prodArray[1].Name);
            }
        }

        [Fact]
        public void CanSendPaginationViewModel()
        {
            Mock<IShopRepository> mock = new Mock<IShopRepository>();
            mock.Setup(m => m.Products).Returns(new[]
            {
                new Product {ProductID = 1, Name = "P1"},
                new Product {ProductID = 2, Name = "P2"},
                new Product {ProductID = 3, Name = "P3"},
                new Product {ProductID = 4, Name = "P4"},
                new Product {ProductID = 5, Name = "P5"}
            }.AsQueryable());

            HomeController controller =
                new HomeController(mock.Object) {PageSize = 3};

            if (controller.Index(null, 2).ViewData.Model is ProductsListViewModel result)
            {
                PagingInfo pageInfo = result.PagingInfo;
                Assert.Equal(2, pageInfo.CurrentPage);
                Assert.Equal(3, pageInfo.ItemsPerPage);
                Assert.Equal(5, pageInfo.TotalItems);
                Assert.Equal(2, pageInfo.TotalPages);
            }
        }

        [Fact]
        public void CanFilterProducts()
        {
            Mock<IShopRepository> mock = new Mock<IShopRepository>();
            mock.Setup(m => m.Products).Returns(new[] {
                new Product {ProductID = 1, Name = "P1", Category = "Cat1"},
                new Product {ProductID = 2, Name = "P2", Category = "Cat2"},
                new Product {ProductID = 3, Name = "P3", Category = "Cat1"},
                new Product {ProductID = 4, Name = "P4", Category = "Cat2"},
                new Product {ProductID = 5, Name = "P5", Category = "Cat3"}
            }.AsQueryable());
            HomeController controller = new HomeController(mock.Object) {PageSize = 3};
            Product[] result = (controller.Index("Cat2").ViewData.Model as ProductsListViewModel)?.Products.ToArray();
            if (result != null)
            {
                Assert.Equal(2, result.Length);
                Assert.True(result[0].Name == "P2" && result[0].Category == "Cat2");
                Assert.True(result[1].Name == "P4" && result[1].Category == "Cat2");
            }
        }
        [Fact]
        public void GenerateCategorySpecificProductCount()
        {
            Mock<IShopRepository> mock = new Mock<IShopRepository>();
            mock.Setup(m => m.Products).Returns(new [] {
                new Product {ProductID = 1, Name = "P1", Category = "Cat1"},
                new Product {ProductID = 2, Name = "P2", Category = "Cat2"},
                new Product {ProductID = 3, Name = "P3", Category = "Cat1"},
                new Product {ProductID = 4, Name = "P4", Category = "Cat2"},
                new Product {ProductID = 5, Name = "P5", Category = "Cat3"}
            }.AsQueryable());

            HomeController target = new HomeController(mock.Object) {PageSize = 3};

            Func<ViewResult, ProductsListViewModel> getModel = result =>
                result?.ViewData?.Model as ProductsListViewModel;
    
            int? res1 = getModel(target.Index("Cat1"))?.PagingInfo.TotalItems;
            int? res2 = getModel(target.Index("Cat2"))?.PagingInfo.TotalItems;
            int? res3 = getModel(target.Index("Cat3"))?.PagingInfo.TotalItems;
            int? resAll = getModel(target.Index(null))?.PagingInfo.TotalItems;
    
            Assert.Equal(2, res1);
            Assert.Equal(2, res2);
            Assert.Equal(1, res3);
            Assert.Equal(5, resAll);
        }
    }
}