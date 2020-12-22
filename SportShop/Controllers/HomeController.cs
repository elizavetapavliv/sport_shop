using Microsoft.AspNetCore.Mvc;
using SportShop.Models;
using SportShop.Models.ViewModels;
using System.Linq;

namespace SportShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly IShopRepository _repository;
        public int PageSize = 4;

        public HomeController(IShopRepository repository)
        {
            _repository = repository;
        }

        public ViewResult Index(string category, int productPage = 1)
            => View(new ProductsListViewModel
            {
                Products = _repository.Products
                    .Where(p => category == null || p.Category == category)
                    .OrderBy(p => p.ProductID)
                    .Skip((productPage - 1) * PageSize)
                    .Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = productPage,
                    ItemsPerPage = PageSize,
                    TotalItems = category == null ?
                        _repository.Products.Count() :
                        _repository.Products.Count(e => e.Category == category)
                },
                CurrentCategory = category
            });
    }
}
