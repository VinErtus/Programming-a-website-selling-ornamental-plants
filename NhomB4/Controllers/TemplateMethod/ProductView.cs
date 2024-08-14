using NhomB4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NhomB4.Controllers.TemplateMethod
{
    public abstract class ProductView
    {
        protected List<Product> products;

        public void DisplayProducts()
        {
            FetchProducts();
            RenderProducts();
        }

        protected abstract void FetchProducts();

        protected virtual void RenderProducts()
        {
        }
    }
}