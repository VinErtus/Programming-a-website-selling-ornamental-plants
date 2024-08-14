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
    public class CustomersController : Controller
    {
        private DBCayCanhNhomB04Entities db = new DBCayCanhNhomB04Entities();

        // GET: Customers
        public ActionResult Index(string _name)
        {
            if (_name == null)
                return View(db.Customers.ToList());
            else
            {
                return View(db.Customers.Where(s => s.NameCus.Contains(_name)).ToList());

            }
        }

        // GET: Customers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // GET: Customers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IDCus,NameCus,PhoneCus,EmailCus,Username,Password")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Customers.Add(customer);
                db.SaveChanges();
                //Sửa 
                return RedirectToAction("LoginCus", "Customers");
            }
            else
            {
                return RedirectToAction("Create");
            }

            return View(customer);
        }

        // GET: Customers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IDCus,NameCus,PhoneCus,EmailCus")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(customer);
        }

        // GET: Customers/Delete/5
        public ActionResult Delete(int id)
        {
            return View(db.Customers.Where(s => s.IDCus == id).FirstOrDefault());
        }
        [HttpPost]
        public ActionResult Delete(int id, Customer cate)
        {
            try
            {
                cate = db.Customers.Where(s => s.IDCus == id).FirstOrDefault();
                db.Customers.Remove(cate);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return Content("Error");
            }
        }
        // Tạo view cho khách hàng Login
        public ActionResult LoginCus()
        {
            return View();
        }
        // Xử lý tìm kiếm UserName, password trong Customer và thông báo
        [HttpPost]
        public ActionResult LoginAcountCus(Customer _cus)
        {
            // check là khách hàng cần tìm
            var check = db.Customers.Where(s => s.UserName == _cus.UserName && s.Password ==
           _cus.Password).FirstOrDefault();
            if (check == null) //không có KH
            {
                ViewBag.ErrorInfo = "Sai tài khoản hoặc mật khẩu";
                return View("LoginCus");
            }
            else
            { // Có tồn tại KH -> chuẩn bị dữ liệu đưa về lại ShowCart.cshtml
                db.Configuration.ValidateOnSaveEnabled = false;
                Session["IDCus"] = check.IDCus;
                Session["Password"] = check.Password;
                Session["NameCus"] = check.NameCus;
                Session["PhoneCus"] = check.PhoneCus;
                // Quay lại trang giỏ hàng với thông tin cần thiết
                return RedirectToAction("ProductList", "Products");
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