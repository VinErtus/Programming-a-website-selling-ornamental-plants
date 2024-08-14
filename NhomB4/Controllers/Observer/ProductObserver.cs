using NhomB4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NhomB4.Controllers.Observer
{
    public class ProductObserver : IProductObserver
    {
        public void ProductUpdated(Product product)
        {
            Console.WriteLine($"Product {product.NamePro} updated. New price: {product.Price}");
        }
    }
}