using NhomB4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NhomB4.Controllers
{
    public class CategoriesController : Controller
    {
        // GET: Categories
        // GET: Categories
        DBCayCanhNhomB04Entities database = new DBCayCanhNhomB04Entities();
        public ActionResult Index(string _name)

        {
            if (_name == null)
                return View(database.Categories.ToList());
            else
            {
                return View(database.Categories.Where(s => s.NameCate.Contains(_name)).ToList());

            }

        }
        public ActionResult Details(int id)
        {
            return View(database.Categories.Where(c => c.Id == id).FirstOrDefault());
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Category cate)
        {
            try
            {
                database.Categories.Add(cate);
                database.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return Content("Error");
            }
        }
        public ActionResult Edit(int id)
        {
            return View(database.Categories.Where(s => s.Id == id).FirstOrDefault());
        }
        [HttpPost]
        public ActionResult Edit(Category cate)
        {
            database.Entry(cate).State = System.Data.Entity.EntityState.Modified;
            database.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Delete(int id)
        {
            return View(database.Categories.Where(s => s.Id == id).FirstOrDefault());
        }
        [HttpPost]
        public ActionResult Delete(int id, Category cate)
        {
            try
            {
                cate = database.Categories.Where(s => s.Id == id).FirstOrDefault();
                database.Categories.Remove(cate);
                database.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return Content("Error");
            }
        }
        // Action PartialViewResult
        [ChildActionOnly]
        public PartialViewResult CategoryPartial()
        {
            var cateList = database.Categories.ToList();
            return PartialView(cateList);
        }

    }
}
