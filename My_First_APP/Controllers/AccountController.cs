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
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using System.Net;
using System.Net.Mail;
namespace My_First_APP.Controllers
{
    public class AccountController : Controller
    {
       
          private FITNESSEntities db = new FITNESSEntities();
        //
        // GET: /MyAccount/
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult Index()
        {
            return RedirectToAction("Manage");
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                
                // поиск пользователя в бд
                Account user = null;
                string Password_hash="123";
                
                    Password_hash = model.Password.GetHashCode().ToString();
                    user = db.Account.Where(A => A.Login == model.Login).FirstOrDefault();

                
                if (user != null && user.PasswordHash== Password_hash)
                {
                    FormsAuthentication.SetAuthCookie(model.Login, true);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Пользователя с таким логином и паролем нет");
                }
            }

            return View(model);
        }
        public ActionResult Registration()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Registration(My_First_APP.Models.RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                Account user1,user2 = null;                
                    user1 = db.Account.Where(A => A.Login == model.Login).FirstOrDefault();
                    user2 = db.Account.Where(A=> A.Mail == model.Mail).FirstOrDefault();
                
                if(user2 == null && user1== null )
                {
                    // создаем нового пользователя
                   
                        Account new_account = new Account();
                        new_account.Id = (db.Account.Max(m => m.Id) + 1);
                        new_account.Login = model.Login;                   

                        new_account.PasswordHash = model.Password.GetHashCode().ToString();
                        db.Account.Add(new_account);
                        db.SaveChanges();
                        user1 = db.Account.Where(A => A.Login == model.Login).FirstOrDefault();
                    
                    // если пользователь удачно добавлен в бд
                    if (user1 != null)
                    {
                        FormsAuthentication.SetAuthCookie(model.Login, true);
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    if (user1!=null)
                    ModelState.AddModelError("", "Пользователь с таким логином уже существует");
                    if(user2!=null)
                    ModelState.AddModelError("", "Пользователь с такой почтой уже существует");
                }
            }
            return View(model);
        }
        public ActionResult Logoff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
        public ActionResult ForgotPassword(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
               message == ManageMessageId.ChangePasswordSuccess ? "Ваш пароль изменен, проверьте свою почту."
               : message == ManageMessageId.Error ? "Произошла ошибка, Пользователя с такой почтой не существует"
               : "";
            ViewBag.ReturnUrl = Url.Action("ForgotPassword");
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(My_First_APP.Models.ForgotPasswordModel model)
        {
            if (ModelState.IsValid)
            {                
                Account user = new Account();
                          
                user = db.Account.Where(A => A.Mail == model.Mail).FirstOrDefault();
                if (user != null)
                {
                    string new_Password = GetPass(6);
                    SendMail(model.Mail, "Сброс пароля", "Ваш новый пароль: " + new_Password + ".Советуем его сменить его при первой же возможности.");
                    new_Password = new_Password.GetHashCode().ToString();
                    user.PasswordHash = new_Password;
                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("ForgotPassword", new { Message = ManageMessageId.ChangePasswordSuccess });
                }
                else
                    ModelState.AddModelError("", "Пользователя с такой почтой не существует.");                
            
            
            }
            model = null;
            return View();
        }
        public ActionResult Manage(ManageMessageId? message)
        {
            Account user = null;
            string curr_user=User.Identity.GetUserName();
                user = db.Account.Where(A => A.Login == curr_user).FirstOrDefault();
            if (user.Person.Count == 0)
                ViewBag.PersonEdit = "Заполнить профиль";
            else
                ViewBag.PersonEdit = "Редактировать профиль";
            
            ViewBag.StatusMessage =
               message == ManageMessageId.ChangePasswordSuccess ? "Ваш пароль изменен."               
               : message == ManageMessageId.Error ? "Произошла ошибка."
               : message == ManageMessageId.ChangeMailSuccess ? "Ваша почта изменена."
               :message==ManageMessageId.EditPersonSuccess ? "Профиль успешно отредактирован."
               :message==ManageMessageId.ManagePersonSuccess ? "Профиль успешно заполнен."
               : "";
            ViewBag.ReturnUrl = Url.Action("Manage");
            return View();
            
            
        }

        public ActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            Account user = null;
           
                string login = User.Identity.GetUserName();
                string old_password = model.OldPassword.GetHashCode().ToString();
                user = db.Account.Where(A => A.Login == login && A.PasswordHash == old_password).FirstOrDefault();
                if (user!= null && model.NewPassword.GetHashCode().ToString()==model.ConfirmNewPassword.GetHashCode().ToString())
                {
                    user.PasswordHash = model.NewPassword.GetHashCode().ToString();
                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
                }
                else
                {
                    ModelState.AddModelError("", "Произошла ошибка, старый пароль введен не верно.");
                   // return RedirectToAction("Manage", new { Message = ManageMessageId.Error });
                }
            
            return View("Manage");
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeMail(ChangeMailModel model)
        {
            Account user=null;
            
                string login = User.Identity.GetUserName();
                string password = model.Password.GetHashCode().ToString();
                user = db.Account.Where(A => A.Login == login && A.PasswordHash == password).FirstOrDefault();
                
                if (user == null )
                {
                    ModelState.AddModelError("", "Произошла ошибка,  пароль введен не верно.");
                  //  return RedirectToAction("Manage", new { Message = ManageMessageId.Error });

                }
                else if (model.NewMail == "" || (db.Account.Count(A => A.Mail == model.NewMail) != 0))
                {
                    ModelState.AddModelError("", "Произошла ошибка,  такой адрес уже используется.");
                   // return RedirectToAction("Manage", new { Message = ManageMessageId.Error });
                }
                else
                {
                    user.Mail = model.NewMail;
                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Manage", new { Message = ManageMessageId.ChangeMailSuccess });
                }
            
            return View("Manage");
        }
        public ActionResult ManagePerson()
        {
            string login = User.Identity.GetUserName();
            Person Current_person = db.Person.Where(p => p.Account.Login == login).FirstOrDefault();
            if (Current_person == null)
            {
                Current_person = new Person();
                Current_person.Sex = Current_person.SecondName = Current_person.FirstName = "";
                Current_person.BirthDate = DateTime.Now;
                Current_person.AccountId = db.Account.Where(a => a.Login == login).FirstOrDefault().Id;
                ViewBag.Action = "ManagePerson";
            }
            else
                ViewBag.Action = "EditPerson";
            return View(ViewBag.Action, Current_person);
        }

        
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ManagePerson(Person model)
        {
            
            db.Person.Add(model);
            db.SaveChanges();
            return RedirectToAction("Manage", new { Message = ManageMessageId.ManagePersonSuccess });
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult EditPerson(Person model)
        {
            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Manage", new { Message = ManageMessageId.EditPersonSuccess });
        }
      
        public static string GetPass(int x)
        {
            string pass = "";
            var r = new Random();
            while (pass.Length < x)
            {
                Char c = (char)r.Next(33, 125);
                if (Char.IsLetterOrDigit(c))
                    pass += c;
            }
            return pass;
        }
        public static void SendMail(string mailto, string caption, string message, string attachFile = null, string smtpServer = "smtp.mail.ru", string password = "black1488", string from = "black_flower_power@mail.ru")
        {
            try
            {

                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(from);
                mail.To.Add(new MailAddress(mailto));
                mail.Subject = caption;
                mail.Body = message;

                if (!string.IsNullOrEmpty(attachFile))
                    mail.Attachments.Add(new Attachment(attachFile));
                SmtpClient client = new SmtpClient();
                client.Host = smtpServer;
                client.Port = 587;
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential(from.Split('@')[0], password);
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Send(mail);
                mail.Dispose();
            }
            catch (Exception e)
            {
                throw new Exception("Mail.Send: " + e.Message);
            }
        }
        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            ChangeMailSuccess,
            RemoveLoginSuccess,
            EditPersonSuccess,
            ManagePersonSuccess,
            SaveSuccess,
            Error
        }
    
}
}

