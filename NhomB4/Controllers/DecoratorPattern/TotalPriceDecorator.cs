using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NhomB4.Controllers.DecoratorPattern
{
    // hàm xử lý tổng giá
    public class TotalPriceDecorator : ShoppingCartDecorator
    {
        private double _additionalPrice;

        public TotalPriceDecorator(IShoppingCart shoppingCart, double additionalPrice) : base(shoppingCart)
        {
            _additionalPrice = additionalPrice;
        }

        public override double GetTotalPrice()
        {
            return base.GetTotalPrice() + _additionalPrice;
        }
    }
}