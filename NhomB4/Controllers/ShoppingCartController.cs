using NhomB4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PayPal.Api;
using NhomB4.Controllers.DecoratorPattern;
namespace NhomB4.Controllers
{
    public class ShoppingCartController : Controller
    {
        // GET: ShoppingCart
        DBCayCanhNhomB04Entities database = new DBCayCanhNhomB04Entities();
        private dynamic number = 0;
        static List<double> numbers = new List<double>();
        /* public ActionResult Index()
         {
             return View();
         }*/
        public ActionResult ShowCart()
        {
            if (Session["Cart"] == null)
                return View("ShowCart");
            Cart _cart = Session["Cart"] as Cart;
            return View(_cart);
        }
        public Cart GetCart()
        {
            Cart cart = Session["Cart"] as Cart;
            if (cart == null || Session["Cart"] == null)
            {
                cart = new Cart();
                Session["Cart"] = cart;
            }
            return cart;
        }

        /* public ActionResult AddToCart(int id)
         {
             // lấy sản phẩm theo id
             var _pro = database.Products.SingleOrDefault(s => s.ProductID == id);
             if (_pro != null)
             {
                 GetCart().Add_Product_Cart(_pro);
             }
             return RedirectToAction("ShowCart", "ShoppingCart");
         }*/
        public ActionResult AddToCart(int id)
        {
            Product product = database.Products.SingleOrDefault(s => s.ProductID == id);

            IShoppingCart cart = Session["ShoppingCart"] as IShoppingCart;
            if (cart == null)
            {
                cart = new ShoppingCart();
                GetCart().Add_Product_Cart(product);
            }

            // Áp dụng decorator để cập nhật tổng giá tiền sau khi thêm sản phẩm vào giỏ hàng
            cart = new TotalPriceDecorator(cart,(double)product.Price);
            cart.AddProduct(product);

            return RedirectToAction("ShowCart", "ShoppingCart");
        }

        // Cập nhật số lượng và tính lại tổng tiền

        public ActionResult Update_Cart_Quantity(FormCollection form)
        {
            Cart cart = Session["Cart"] as Cart;
            int id_pro = int.Parse(Request.Form["idPro"]);
            int _quantity = int.Parse(Request.Form["carQuantity"]);
            cart.Update_quantity(id_pro, _quantity);
            return RedirectToAction("ShowCart", "ShoppingCart");
        }
        // Xóa dòng sản phẩm trong giỏ hàng
        public ActionResult RemoveCart(int id)
        {
            Cart cart = Session["Cart"] as Cart;
            cart.Remove_CartItem(id);
            return RedirectToAction("ShowCart", "ShoppingCart");
        }
        public PartialViewResult BagCart()
        {
            int total_quantity_item = 0;
            Cart cart = Session["Cart"] as Cart;
            if (cart != null)
                total_quantity_item = cart.Total_quantity();
            ViewBag.TotalCart = total_quantity_item;
            return PartialView("BagCart");
        }
        public ActionResult CheckOut(FormCollection form)
        {
            try
            {
             
                    Cart cart = Session["Cart"] as Cart;
                    OrderPro _order = new OrderPro();
                    _order.DateOrder = DateTime.Now;
                    _order.AddressDeliverry = form["AddressDeliverry"];
                    _order.IDCus = int.Parse(form["CodeCustomer"]);
                    database.OrderProes.Add(_order);
                    foreach (var item in cart.Items)
                    {
                        // lưu dòng sản phẩm vào chi tiết hóa đơn
                        OrderDetail _order_detail = new OrderDetail();
                        _order_detail.IDOrder = _order.ID;
                        _order_detail.IDProduct = item._product.ProductID;
                        _order_detail.UnitPrice = (double)item._product.Price;
                        _order_detail.Quantity = item._quantity;
                    numbers.Add(item._quantity * (double)item._product.Price);
                        database.OrderDetails.Add(_order_detail);
                    }
                    database.SaveChanges();
                    cart.ClearCart();
                    return RedirectToAction("PaymentWithPaypal", "ShoppingCart");
                
               
            }
            catch
            {
                return Content("Có sai sót! Xin kiểm tra lại thông tin"); ;
            }
        }
        public ActionResult CheckOut_Success()
        {
            return View();

        }
        public ActionResult FailureView()
        {
            return View();
        }
        public ActionResult SuccessView()
        {
            return View();
        }

        public ActionResult PaymentWithPaypal(string Cancel = null)
        {
            //getting the apiContext  
            APIContext apiContext = PaypalConfiguration.GetAPIContext();
            try
            {
                //A resource representing a Payer that funds a payment Payment Method as paypal  
                //Payer Id will be returned when payment proceeds or click to pay  
                string payerId = Request.Params["PayerID"];
                if (string.IsNullOrEmpty(payerId))
                {
                    //this section will be executed first because PayerID doesn't exist  
                    //it is returned by the create function call of the payment class  
                    // Creating a payment  
                    // baseURL is the url on which paypal sendsback the data.  
                    string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority + "/ShoppingCart/PaymentWithPayPal?";
                    //here we are generating guid for storing the paymentID received in session  
                    //which will be used in the payment execution  
                    var guid = Convert.ToString((new Random()).Next(100000));
                    //CreatePayment function gives us the payment approval url  
                    //on which payer is redirected for paypal account payment  
                    var createdPayment = this.CreatePayment(apiContext, baseURI + "guid=" + guid);
                    //get links returned from paypal in response to Create function call  
                    var links = createdPayment.links.GetEnumerator();
                    string paypalRedirectUrl = null;
                    while (links.MoveNext())
                    {
                        Links lnk = links.Current;
                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            //saving the payapalredirect URL to which user will be redirected for payment  
                            paypalRedirectUrl = lnk.href;
                        }
                    }
                    // saving the paymentID in the key guid  
                    Session.Add(guid, createdPayment.id);
                    return Redirect(paypalRedirectUrl);
                }
                else
                {
                    // This function exectues after receving all parameters for the payment  
                    var guid = Request.Params["guid"];
                    var executedPayment = ExecutePayment(apiContext, payerId, Session[guid] as string);
                    //If executed payment failed then we will show payment failure message to user  
                    if (executedPayment.state.ToLower() != "approved")
                    {
                        return View("Khong du tien");
                    }
                }
            }
            catch (Exception ex)
            {
                return View("Khong du tien");
            }
            //on successful payment, show success page to user.  
            return View("SuccessView");
        }
        private PayPal.Api.Payment payment;
        private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution()
            {
                payer_id = payerId
            };
            this.payment = new Payment()
            {
                id = paymentId
            };
            return this.payment.Execute(apiContext, paymentExecution);
        }
       
        private Payment CreatePayment(APIContext apiContext, string redirectUrl)
        {
            //create itemlist and add item objects to it
            var loc = 10;
            var listSanPham = Session["Cart"] as Cart;
            var itemList = new ItemList()
            {
                items = new List<Item>()
            };
            //Adding Item Details like name, currency, price etc              
            foreach (var item in listSanPham.Items)
            {

        
                itemList.items.Add(new Item()
                {
                    
                    name = item._product.NamePro,
                    currency = "USD",
                    price = item._product.Price.ToString(),
                    quantity = item._quantity.ToString(),
                    sku = item._product.ProductID.ToString()
                });
            }
            
            var payer = new Payer()
            {
                payment_method = "paypal"
            };
            // Configure Redirect Urls here with RedirectUrls object  
            var redirUrls = new RedirectUrls()
            {
                cancel_url = redirectUrl + "&Cancel=true",
                return_url = redirectUrl
            };
            // Adding Tax, shipping and Subtotal details  
            var sum = numbers.Sum();
            var sum1 = sum.ToString();
            var details = new Details()
            {
                tax = "0",
                shipping = "0",
                subtotal = sum1
            };
            //Final amount with details  
  
            var amount = new Amount()
            {
                currency = "USD",
                total = sum1, // Total must be equal to sum of tax, shipping and subtotal.  
                details = details
            };
            var transactionList = new List<Transaction>();
            // Adding description about the transaction  
            var paypalOrderId = DateTime.Now.Ticks;
            transactionList.Add(new Transaction()
            {
                description = $"Invoice #{paypalOrderId}",
                invoice_number = paypalOrderId.ToString(), //Generate an Invoice No    
                amount = amount,
                item_list = itemList
            });
            this.payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
            };
            // Create a payment using a APIContext  
            return this.payment.Create(apiContext);
        }

    }
}
    
