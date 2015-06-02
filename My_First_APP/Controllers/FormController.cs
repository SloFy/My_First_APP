using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using My_First_APP.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace My_First_APP.Controllers
{
    public class FormController : Controller
    {
        FITNESSEntities db = new FITNESSEntities();
        // GET: Input
        public ActionResult Index(ManageMessageId? message)
        {
            // Account user = null;
            string login = User.Identity.GetUserName();
            ViewBag.StatusMessage =
              message == ManageMessageId.CreateSuccess ? "Анкета успешно заполнена."
              : message == ManageMessageId.Error ? "Произошла ошибка."
              : "";
            if (login == "" || login == null)
                return View("~/Views/Account/Registration.cshtml");

            int blank_cnt = db.Blank.Count(b => b.Person.Account.Login == login);
            if (blank_cnt == 0)
            {
                var BlankTypes = db.BlankType;
                return View(BlankTypes);
            }
            else
            {
                var BlankTypes = db.BlankType.Where(m => m.Id != 1);
                return View(BlankTypes);
            }

        }

        // GET: Input/Create/2
        public ActionResult Create(int blank_type=2)
        {
            string login = User.Identity.GetUserName();
            Test first = new Test();
            first=first.Create_New_Blank(blank_type, login);
            return View(first);

        }

        // POST: Input/Create
        [HttpPost]
        public ActionResult Create(int blank_id,int current_question,List<bool> b_answers,List<string> s_answers)
        {
            try
            {
                
                Test current = new Test(blank_id, current_question, s_answers, b_answers);
                current.add_to_db();
                if (!current.Last())
                {
                    Test next = current.get_next(current);
                    ModelState.Clear();
                    return View(next);
                }
                else
                    return RedirectToAction("Index", new { Message = ManageMessageId.CreateSuccess });
            }
            catch
            {
                return View("ошибочка вышла");
            }
        }

        public enum ManageMessageId
        {
            CreateSuccess,
            Error
        }
    }
}
