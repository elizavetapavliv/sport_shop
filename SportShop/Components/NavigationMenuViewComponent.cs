using Microsoft.AspNetCore.Mvc;
using SportShop.Models;
using System.Collections.Generic;
using System.Linq;

namespace SportShop.Components
{
    public class NavigationMenuViewComponent : ViewComponent
    {
        private IShopRepository repository;
        public NavigationMenuViewComponent(IShopRepository repo)
        {
            repository = repo;
        }
        public IViewComponentResult Invoke()
        {
            ViewBag.SelectedCategory = RouteData?.Values["category"];
            return View(repository.Products
                .Select(x => x.Category)
                .Distinct()
                .OrderBy(x => x));
        }
    }
}