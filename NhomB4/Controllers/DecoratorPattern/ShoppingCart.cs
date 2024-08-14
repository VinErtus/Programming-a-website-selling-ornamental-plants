using NhomB4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NhomB4.Controllers.DecoratorPattern
{
    //xử lý shopping cart
    public class ShoppingCart : IShoppingCart
    {
        private List<Product> _products = new List<Product>();

        public void AddProduct(Product product)
        {
            _products.Add(product);
        }

        public double GetTotalPrice()
        {
            return (double)_products.Sum(p => p.Price);
        }
    }
}