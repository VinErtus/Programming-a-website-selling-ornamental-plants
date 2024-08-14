using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using NhomB4.Controllers.FactoryMethod;
using NhomB4.Controllers.Observer;
using NhomB4.Controllers.TemplateMethod;
using NhomB4.Models;
using PagedList;

namespace NhomB4.Controllers
{
    public class ProductsController : Controller
    {
        private DBCayCanhNhomB04Entities db = new DBCayCanhNhomB04Entities();
        // GET: Products
        public ActionResult Index(string category, string SearchString, decimal min = decimal.MinValue, decimal max = decimal.MaxValue)
        {
            var products = db.Products.Include(p => p.Category1);
            if (category != null)
            {
                products = products.Where(x => x.Category == category);
            }

            if (!String.IsNullOrEmpty(SearchString))
            {
                products = products.Where(s => s.NamePro.Contains(SearchString));
            }

            if (min >= 0 && max > 0)
            {
                products = products.Where(p => p.Price >= min && p.Price <= max);
            }

            // return View(products.OrderByDescending(x => x.NamePro).ToList());
            ConcreteProductView productView = new ConcreteProductView();
            productView.Products = products.OrderByDescending(x => x.NamePro).ToList(); // Gán danh sách sản phẩm cho ConcreteProductView
            productView.DisplayProducts(); // Hiển thị sản phẩm
            return View(productView);
        }
        public ActionResult Details_Admin(int id)
        {
            return View(db.Products.Where(c => c.ProductID == id).FirstOrDefault());
        }
        public ActionResult Index_2(string category)
        {
            if (category == null)
            {
                var productList = db.Products.OrderByDescending(x => x.NamePro);
                return View(productList);
            }
            else
            {
                var productList = db.Products.OrderByDescending(x => x.NamePro).Where(x => x.Category == category);
                return View(productList);
            }

        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            ViewBag.Category = new SelectList(db.Categories, "IDCate", "NameCate");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        /*public ActionResult AddProduct(string name, double price)
        {
            Product product = ProductFactory.CreateProduct(name, price);

            db.Products.Add(product);
            db.SaveChanges();

            return RedirectToAction("Index");
        }*/

        /*public ActionResult Create([Bind(Include = "ProductID,NamePro,DecriptionPro,Category,Status,Price,ImagePro")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index_2");
            }

            ViewBag.Category = new SelectList(db.Categories, "IDCate", "NameCate", product.Category);
            return View(product);
        }*/
        // tạo sản phẩm trên design factory method
        public ActionResult Create(string name, decimal price, string decriptionPro, string category, string imagePro, Product product)
        {
            if (ModelState.IsValid)
            {
               product = ProductFactory.CreateProduct(name, price,decriptionPro,category,imagePro);

                db.Products.Add(product);
                db.SaveChanges();

                return RedirectToAction("Index_2");
            }

            ViewBag.Category = new SelectList(db.Categories, "IDCate", "NameCate", product.Category);
            return View(product);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.Category = new SelectList(db.Categories, "IDCate", "NameCate", product.Category);
            return View(product);
            
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        private Product _product;
        private List<IProductObserver> _observers = new List<IProductObserver>();
        public void UpdateProduct(Product product)
        {
            _product = product;
            NotifyObservers();

            // Lưu dữ liệu vào cơ sở dữ liệu bằng Entity Framework
            using (var db = new DBCayCanhNhomB04Entities()) // Thay YourDbContext bằng tên DbContext của bạn
            {
                var existingProduct = db.Products.Find(product.ProductID);
                if (existingProduct != null)
                {
                    existingProduct.NamePro = product.NamePro;
                    existingProduct.Price = product.Price;
                    db.SaveChanges();
                }
            }
        }
        //dùng Observers để chinhr sửa thông tin
        private void NotifyObservers()
        {
            foreach (var observer in _observers)
            {
                observer.ProductUpdated(_product);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Edit([Bind(Include = "ProductID,NamePro,DecriptionPro,Category,Status,Price,ImagePro")] Product product)
        {

            if (ModelState.IsValid)
            {
                UpdateProduct(product);
                NotifyObservers();
                db.SaveChanges();
                return RedirectToAction("Index_2");
            }
            ViewBag.Category = new SelectList(db.Categories, "IDCate", "NameCate", product.Category);
            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index_2");
        }
        public ActionResult ProductList(int? page, string category, string SearchString, double min = double.MinValue, double max = double.MaxValue)
        { // Tạo Products và có tham chiếu đến Category
            var products = db.Products.Include(p => p.Category1);
            if (category == null)
            {
                products = db.Products.OrderByDescending(x => x.NamePro);
            }
            else
            {
                products = db.Products.OrderByDescending(x => x.Category).Where(x => x.Category == category);
            }
            //Tìm kiếm chuỗi truy vấn theo NamePro, nếu chuỗi truy vấn SearchString khác rỗng, null
            if (!String.IsNullOrEmpty(SearchString))
            {
                products = products.Where(s => s.NamePro.Contains(SearchString));
            }
            // Tìm kiếm chuỗi truy vấn theo đơn giá
            if (min >= 0 && max > 0)
            {
                products = db.Products.OrderByDescending(x => x.Price).Where(p => (double)p.Price >= min && (double)p.Price <= max);
            }
            // Khai báo mỗi trang 4 sản phẩm
            // Khai báo mỗi trang 4 sản phẩm
            int pageSize = 8;
            // Toán tử ?? trong C# mô tả nếu page khác null thì lấy giá trị page, còn
            // nếu page = null thì lấy giá trị 1 cho biến pageNumber.
            int pageNumber = (page ?? 1);
            // Nếu page = null thì đặt lại page là 1.
            if (page == null) page = 1;
            // Trả về các product được phân trang theo kích thước và số trang.
            return View(products.ToPagedList(pageNumber, pageSize));

        }
    }
}
