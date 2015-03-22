using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.Mvc;
using System.Data.Entity;
using My_First_APP.Models;
using My_First_APP.Util;
using System.Data.Sql;


namespace My_First_APP.Controllers
{
    public class MyAccountController : Controller
    {
        FITNESSEntities db = new FITNESSEntities();
        //
        // GET: /MyAccount/
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]        
        public ActionResult Registration(My_First_APP.Models.RegisterViewModel model)
        {
            if (!(check_Login(model)))
            {
                if (model.Password == model.ConfirmPassword)
                {
                    Account new_Account = new Account();
                    new_Account.PasswordHash = model.Password.GetHashCode().ToString();
                    new_Account.Login = model.Login;
                    Add_Account(model);
                    return RedirectToAction("Card", "Fitness");
                }
                else
                {
                }
                return View();
            }
            else
            {
                return View();
            }
        }
        
       
        
        public void Add_Account(My_First_APP.Models.RegisterViewModel model)
        {
            Account new_Account = new Account();
            new_Account.PasswordHash = model.Password.GetHashCode().ToString();
            new_Account.Login = model.Login;
            new_Account.Id = (db.Account.Max(m => m.Id)) + 1;
            db.Account.Add(new_Account);
            db.SaveChanges();
        }
        bool check_Login(RegisterViewModel model)
        {
            int cnt = db.Account.Count(A => A.Login.ToUpper() == model.Login.ToUpper());
            if (cnt == 0)
                return false;
            else
                return true;
        }
        public ActionResult Registration()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(My_First_APP.Models.LoginViewModel model)
        {
            Account Login_Account = new Account();
            int curr_pass_hash = model.Password.ToString().GetHashCode();
            Login_Account = db.Account.Where(a => a.Login == model.Login).First();
            int pass_Hash = Convert.ToInt32(Login_Account.PasswordHash);
            if (curr_pass_hash == pass_Hash)
            {

            }
            else
            {

            }
            return View();
        }
        
        public ActionResult Registration1()
        {
            return View();
        }
	}
}