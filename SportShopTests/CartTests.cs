using SportShop.Models;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SportShopTests
{
    public class CartTests
    {
        [Fact]
        public void CanAddNewLines()
        {
            Product p1 = new Product { ProductID = 1, Name = "P1" };
            Product p2 = new Product { ProductID = 2, Name = "P2" };

            Cart target = new Cart();
            target.AddItem(p1, 1);
            target.AddItem(p2, 1);
            List<CartItem> results = target.Items;

            Assert.Equal(2, results.Count);
            Assert.Equal(p1, results[0].Product);
            Assert.Equal(p2, results[1].Product);
        }

        [Fact]
        public void CanAddQuantityForExistingLines()
        {
            Product p1 = new Product { ProductID = 1, Name = "P1" };
            Product p2 = new Product { ProductID = 2, Name = "P2" };

            Cart target = new Cart();
            target.AddItem(p1, 1);
            target.AddItem(p2, 1);
            target.AddItem(p1, 10);

            List<CartItem> results = target.Items.OrderBy(c => c.Product.ProductID).ToList();
            Assert.Equal(2, results.Count);
            Assert.Equal(11, results[0].Quantity);
            Assert.Equal(1, results[1].Quantity);
        }

        [Fact]
        public void CanRemoveLine()
        {
            Product p1 = new Product { ProductID = 1, Name = "P1" };
            Product p2 = new Product { ProductID = 2, Name = "P2" };
            Product p3 = new Product { ProductID = 3, Name = "P3" };

            Cart target = new Cart();
            target.AddItem(p1, 1);
            target.AddItem(p2, 3);
            target.AddItem(p3, 5);
            target.AddItem(p2, 1);

            target.RemoveItem(p2);
            Assert.Empty(target.Items.Where(c => c.Product == p2));
            Assert.Equal(2, target.Items.Count);
        }

        [Fact]
        public void CalculateCartTotal()
        {
            Product p1 = new Product { ProductID = 1, Name = "P1", Price = 100M };
            Product p2 = new Product { ProductID = 2, Name = "P2", Price = 50M };

            Cart target = new Cart();
            target.AddItem(p1, 1);
            target.AddItem(p2, 1);
            target.AddItem(p1, 3);
            decimal result = target.ComputeTotalValue();
            Assert.Equal(450M, result);
        }

        [Fact]
        public void CanClearContents()
        {
            Product p1 = new Product { ProductID = 1, Name = "P1", Price = 100M };
            Product p2 = new Product { ProductID = 2, Name = "P2", Price = 50M };
   
            Cart target = new Cart();
            
            target.AddItem(p1, 1);
            target.AddItem(p2, 1);

            target.Clear();
            Assert.Empty(target.Items);
        }
    }
}
