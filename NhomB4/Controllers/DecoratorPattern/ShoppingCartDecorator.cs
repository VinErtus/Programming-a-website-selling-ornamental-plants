using NhomB4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NhomB4.Controllers.DecoratorPattern
{
    //shopping cart decorator
    public abstract class ShoppingCartDecorator : IShoppingCart
    {
        protected IShoppingCart _shoppingCart;

        public ShoppingCartDecorator(IShoppingCart shoppingCart)
        {
            _shoppingCart = shoppingCart;
        }

        public virtual void AddProduct(Product product)
        {
            _shoppingCart.AddProduct(product);
        }

        public virtual double GetTotalPrice()
        {
            return _shoppingCart.GetTotalPrice();
        }
    }
}