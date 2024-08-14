using NhomB4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NhomB4.Controllers
{
    public class LoginUsersController : Controller
    { // GET: LoginUsers
        DBCayCanhNhomB04Entities database = new DBCayCanhNhomB04Entities();
        public ActionResult LoginAccount()
        {
            return View();
        }
        [HttpPost]
        public ActionResult LoginAccount(AdminUser _user)
        {
            var check = database.AdminUsers.Where(s => s.NameUser == _user.NameUser && s.PasswordUser == _user.PasswordUser).FirstOrDefault();
            if (check == null)
            {
                ViewBag.ErrorInfo = "SaiInfo";
                return View("LoginAccount");
            }

            else
            {
                database.Configuration.ValidateOnSaveEnabled = false;
                Session["NameUser"] = _user.NameUser;
                Session["PasswordUser"] = _user.PasswordUser;
                return RedirectToAction("Index_2", "Products");
            }
        }
        // Regíter
        [HttpGet]
        public ActionResult RegisterUser()
        {
            return View();
        }
        [HttpPost]
        public ActionResult RegisterUser(AdminUser _user)
        {
            if (ModelState.IsValid)
            {
                database.Configuration.ValidateOnSaveEnabled = false;
                database.AdminUsers.Add(_user);
                database.SaveChanges();
                return RedirectToAction("LoginAccount","LoginUsers");
            }
            else
            {
                ViewBag.ErrorRegister = "ID này đã có rồi !!!";
            }
            return View();
        }
        public ActionResult LogOutUser()
        {
            Session.Abandon();
            return RedirectToAction("ProductList", "Products");
        }
    }
}