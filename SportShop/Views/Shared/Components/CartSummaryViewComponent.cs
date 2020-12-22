using Microsoft.AspNetCore.Mvc;
using SportShop.Models;

namespace SportShop.Views.Shared.Components
{
    public class CartSummaryViewComponent : ViewComponent
    {
        private readonly Cart _cart;
        public CartSummaryViewComponent(Cart cartService)
        {
            _cart = cartService;
        }
        public IViewComponentResult Invoke()
        {
            return View(_cart);
        }
    }
}
