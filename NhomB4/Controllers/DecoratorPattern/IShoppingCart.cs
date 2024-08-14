using NhomB4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NhomB4.Controllers.DecoratorPattern
{
    // lớp trừu tượng
    public interface IShoppingCart
    {
        void AddProduct(Product product);
        double GetTotalPrice();
    }
}