using NhomB4.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NhomB4.Controllers.FactoryMethod
{
    public class ProductFactory
    {
        public static Product CreateProduct(string name, decimal price,  string decriptionPro, string category, string imagePro)
        {
            return new Product {NamePro = name, Price = price, DecriptionPro = decriptionPro, Category = category , ImagePro = imagePro};
        }
    }
}