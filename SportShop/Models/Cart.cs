using System.Collections.Generic;
using System.Linq;

namespace SportShop.Models
{
    public class Cart
    {
        public List<CartItem> Items { get; set; } = new List<CartItem>();
        public virtual void AddItem(Product product, int quantity)
        {
            CartItem item = Items.FirstOrDefault(p => p.Product.ProductID == product.ProductID);
            if (item == null)
            {
                Items.Add(new CartItem
                {
                    Product = product,
                    Quantity = quantity
                });
            }
            else
            {
                item.Quantity += quantity;
            }
        }
        public virtual void RemoveItem(Product product) =>
            Items.RemoveAll(l => l.Product.ProductID == product.ProductID);
        public decimal ComputeTotalValue() =>
            Items.Sum(e => e.Product.Price * e.Quantity);
        public virtual void Clear() => Items.Clear();
    }
    public class CartItem
    {
        public int CartItemID { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }
}
