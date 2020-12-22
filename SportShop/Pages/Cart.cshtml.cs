using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SportShop.Infrastructure;
using SportShop.Models;
using System.Linq;

namespace SportShop.Pages
{
    public class CartModel : PageModel
    {
        private readonly IShopRepository _repository;
        public CartModel(IShopRepository repo, Cart cartService)
        {
            _repository = repo;
            Cart = cartService;
        }
        public Cart Cart { get; set; }
        public string ReturnUrl { get; set; }
        public void OnGet(string returnUrl)
        {
            ReturnUrl = returnUrl ?? "/";
        }
        public IActionResult OnPost(long productId, string returnUrl)
        {
            Product product = _repository.Products.FirstOrDefault(p => p.ProductID == productId);
            Cart.AddItem(product, 1);
            return RedirectToPage(new {returnUrl });
        }
        public IActionResult OnPostRemove(long productId, string returnUrl)
        {
            Cart.RemoveItem(Cart.Items.First(cartItem => 
                cartItem.Product.ProductID == productId).Product);
            return RedirectToPage(new { returnUrl });
        }
    }
}
