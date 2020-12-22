using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace SportShop.Models
{
    public class EFOrderRepository : IOrderRepository
    {
        private readonly ShopDbContext _context;

        public EFOrderRepository(ShopDbContext context)
        {
            _context = context;
        }

        public IQueryable<Order> Orders => _context.Orders
            .Include(order => order.Items)
            .ThenInclude(item => item.Product);
        public void SaveOrder(Order order)
        {
            _context.AttachRange(order.Items.Select(item => item.Product));
            if (order.OrderID == 0)
            {
                _context.Orders.Add(order);
            }
            _context.SaveChanges();
        }
    }
}
