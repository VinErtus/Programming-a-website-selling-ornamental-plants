using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using NhomB4.Models;

namespace NhomB4.Controllers
{
    public class OrderProesController : Controller
    {
        private DBCayCanhNhomB04Entities db = new DBCayCanhNhomB04Entities();
        static int id;
        static int id1;
        // GET: OrderProes
        public ActionResult Index()
        {
            var orderProes = db.OrderProes.Include(o => o.Customer);
            return View(orderProes.ToList());
        }

        // GET: OrderProes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderPro orderPro = db.OrderProes.Find(id);
            if (orderPro == null)
            {
                return HttpNotFound();
            }
            return View(orderPro);
        }
        /*        public ActionResult LichSuDatHang(int? idcus)
                {
                    *//* return View(db.OrderProes.Where(c => c.IDCus == idcus).FirstOrDefault());*/
        /* var orderProes = db.OrderProes.Include(o => o.IDCus==8);
         if (orderProes == null)
         {
             return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }
         if (orderProes == null)
         {
             return HttpNotFound();
         }
         return RedirectToAction("LichSuDatHang", orderProes);*//*
        if (idcus == null)
        {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }
        var orderProes = db.OrderProes.Find(idcus);
        if (orderProes == null)
        {
            return HttpNotFound();
        }
        return View(orderProes.ToList());
    }
*/
        public ActionResult LichSuDatHang(int? idcus)
        {
            if (idcus == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Lấy danh sách đơn đặt hàng của khách hàng có ID là idcus
            var ordersForCustomer = db.OrderProes.Where(c => c.IDCus == idcus).ToList();

            // Kiểm tra nếu danh sách đơn đặt hàng là rỗng
            if (ordersForCustomer.Count == 0)
            {
                return HttpNotFound();
            }

            // Tạo một danh sách để chứa thông tin sản phẩm
            List<Product> productList = new List<Product>();

            // Duyệt qua từng đơn đặt hàng của khách hàng
            foreach (var order in ordersForCustomer)
            {
                // Lấy danh sách chi tiết đơn đặt hàng của đơn hàng hiện tại
                var orderDetails = db.OrderDetails.Where(od => od.IDOrder == order.ID).ToList();

                // Duyệt qua từng chi tiết đơn đặt hàng
                foreach (var orderDetail in orderDetails)
                {
                    // Lấy thông tin sản phẩm dựa trên IDProduct
                    var product = db.Products.FirstOrDefault(p => p.ProductID == orderDetail.IDProduct);

                    // Kiểm tra xem sản phẩm có tồn tại không trước khi thêm vào danh sách
                    if (product != null)
                    {
                        productList.Add(product);
                    }
                }
            }

            return View(productList);
        }
        // GET: OrderProes/Create
        public ActionResult Create()
        {
            ViewBag.IDCus = new SelectList(db.Customers, "IDCus", "NameCus");
            return View();
        }
       
        // POST: OrderProes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,DateOrder,IDCus,AddressDeliverry")] OrderPro orderPro)
        {
            if (ModelState.IsValid)
            {
                db.OrderProes.Add(orderPro);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IDCus = new SelectList(db.Customers, "IDCus", "NameCus", orderPro.IDCus);
            return View(orderPro);
        }

        // GET: OrderProes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderPro orderPro = db.OrderProes.Find(id);
            if (orderPro == null)
            {
                return HttpNotFound();
            }
            ViewBag.IDCus = new SelectList(db.Customers, "IDCus", "NameCus", orderPro.IDCus);
            return View(orderPro);
        }

        // POST: OrderProes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,DateOrder,IDCus,AddressDeliverry")] OrderPro orderPro)
        {
            if (ModelState.IsValid)
            {
                db.Entry(orderPro).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IDCus = new SelectList(db.Customers, "IDCus", "NameCus", orderPro.IDCus);
            return View(orderPro);
        }

        // GET: OrderProes/Delete/5
        public ActionResult Delete(int id)
        {
            return View(db.OrderProes.Where(s => s.ID == id).FirstOrDefault());
        }
        [HttpPost]
        public ActionResult Delete(int id, OrderPro cate)
        {
            try
            {
                cate = db.OrderProes.Where(s => s.ID == id).FirstOrDefault();
                db.OrderProes.Remove(cate);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return Content("Error");
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
