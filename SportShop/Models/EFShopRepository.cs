using System.Linq;

namespace SportShop.Models
{
    public class EFShopRepository : IShopRepository
    {
        private readonly ShopDbContext _context;

        public EFShopRepository(ShopDbContext context)
        {
            _context = context;
        }
        public IQueryable<Product> Products => _context.Products;
        public void CreateProduct(Product p)
        {
            _context.Add(p);
            _context.SaveChanges();
        }
        public void DeleteProduct(Product p)
        {
            _context.Remove(p);
            _context.SaveChanges();
        }
        public void SaveProduct(Product p)
        {
            _context.SaveChanges();
        }
    }
}
