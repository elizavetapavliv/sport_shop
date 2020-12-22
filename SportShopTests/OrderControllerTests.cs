using Microsoft.AspNetCore.Mvc;
using Moq;
using SportShop.Controllers;
using SportShop.Models;
using Xunit;

namespace SportShopTests
{
    public class OrderControllerTests
    {
        [Fact]
        public void CannotCheckoutEmptyCart()
        {
            Mock<IOrderRepository> mock = new Mock<IOrderRepository>();

            Cart cart = new Cart();
            Order order = new Order();

            OrderController target = new OrderController(mock.Object, cart);

            ViewResult result = target.Checkout(order) as ViewResult;

            mock.Verify(repository => repository.SaveOrder(It.IsAny<Order>()), Times.Never);
            Assert.True(result != null && string.IsNullOrEmpty(result.ViewName));
            Assert.False(result.ViewData.ModelState.IsValid);
        }

        [Fact]
        public void CannotCheckoutInvalidShippingDetails()
        {
            Mock<IOrderRepository> mock = new Mock<IOrderRepository>();

            Cart cart = new Cart();
            cart.AddItem(new Product(), 1);
            OrderController target = new OrderController(mock.Object, cart);
   
            target.ModelState.AddModelError("error", "error");
            
            ViewResult result = target.Checkout(new Order()) as ViewResult;
    
            mock.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Never);
            Assert.True(result != null && string.IsNullOrEmpty(result.ViewName));
            Assert.False(result.ViewData.ModelState.IsValid);
        }

        [Fact]
        public void CanCheckoutAndSubmitOrder()
        {
            Mock<IOrderRepository> mock = new Mock<IOrderRepository>();
            Cart cart = new Cart();
            cart.AddItem(new Product(), 1);

            OrderController target = new OrderController(mock.Object, cart);
            RedirectToPageResult result = target.Checkout(new Order()) as RedirectToPageResult;
            mock.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Once);
            Assert.True(result != null);
            Assert.Equal("/Completed", result.PageName);
        }
    }
}