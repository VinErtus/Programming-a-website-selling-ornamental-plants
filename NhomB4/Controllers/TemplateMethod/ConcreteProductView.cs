using NhomB4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NhomB4.Controllers.TemplateMethod
{
    public class ConcreteProductView : ProductView
    {
        // Danh sách các sản phẩm
        public IEnumerable<Product> Products { get; set; }
        protected override void FetchProducts()
        {
            using (var db = new DBCayCanhNhomB04Entities())
            {
                products = db.Products.ToList();
            }
        }
        protected override void RenderProducts()
        {
            Console.WriteLine("List of Products:");
            base.RenderProducts();
        }
    }
}