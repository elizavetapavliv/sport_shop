using Moq;
using SportShop.Models;
using SportShop.Pages;
using System.Linq;
using Xunit;

namespace SportShopTests
{
    public class CartPageTests
    {
        [Fact]
        public void CanLoadCart()
        {
            Product p1 = new Product { ProductID = 1, Name = "P1" };
            Product p2 = new Product { ProductID = 2, Name = "P2" };

            Mock<IShopRepository> mockRepo = new Mock<IShopRepository>();
            mockRepo.Setup(m => m.Products).Returns(new[] {p1, p2}.AsQueryable());

            Cart testCart = new Cart();
            testCart.AddItem(p1, 2);
            testCart.AddItem(p2, 1);

            CartModel cartModel = new CartModel(mockRepo.Object, testCart);

            cartModel.OnGet("myUrl");
            Assert.Equal(2, cartModel.Cart.Items.Count);
            Assert.Equal("myUrl", cartModel.ReturnUrl);
        }

        [Fact]
        public void CanUpdateCart()
        {
            Mock<IShopRepository> mockRepo = new Mock<IShopRepository>();
            mockRepo.Setup(m => m.Products).Returns(new[] {
                new Product { ProductID = 1, Name = "P1" }
            }.AsQueryable());
            Cart testCart = new Cart();

            CartModel cartModel = new CartModel(mockRepo.Object, testCart);

            cartModel.OnPost(1, "myUrl");

            Assert.Single(testCart.Items);
            Assert.Equal("P1", testCart.Items.First().Product.Name);
            Assert.Equal(1, testCart.Items.First().Quantity);
        }
    }
}
