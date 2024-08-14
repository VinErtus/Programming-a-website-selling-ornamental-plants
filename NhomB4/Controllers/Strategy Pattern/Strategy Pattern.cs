using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NhomB4.Controllers.Strategy_Pattern
{
    public class Strategy_Pattern
    {

        //Lựa chọn hình thức vận chuyển tiêu chuẩn hoặc nhanh
        public interface IShippingPriceStrategy
        {
            double CalculatePrice(double weight);
        }

        public class StandardShippingPriceStrategy : IShippingPriceStrategy
        {
            public double CalculatePrice(double weight)
            {
                //Tính giá vận chuyển tiêu chuẩn
                return weight * 0.1; // Giả sử giá là 0.1 đơn vị tiền cho mỗi đơn vị khối trọng lượng
            }
        }

        public class ExpressShippingPriceStrategy : IShippingPriceStrategy
        {
            public double CalculatePrice(double weight)
            {
                //Tính giá vận chuyển nhanh
                return weight * 0.3; // Giả sử giá là 0.3 đơn vị tiền cho mỗi đơn vị khối trọng lượng
            }
        }

        //Lựa chọn thay đổi hình thức vận chuyển
        public class OrderController : Controller
        {
            private IShippingPriceStrategy _shippingPriceStrategy;

            public OrderController(IShippingPriceStrategy shippingPriceStrategy)
            {
                _shippingPriceStrategy = shippingPriceStrategy;
            }

            public ActionResult CalculateShippingPrice(double weight, string shippingOption)
            {
                double shippingPrice = 0;

                if (shippingOption == "standard")
                {
                    _shippingPriceStrategy = new StandardShippingPriceStrategy();
                }
                else if (shippingOption == "express")
                {
                    _shippingPriceStrategy = new ExpressShippingPriceStrategy();
                }

                // Tính toán giá vận chuyển
                shippingPrice = _shippingPriceStrategy.CalculatePrice(weight);
                return View(shippingPrice);
            }
        }
    }
}