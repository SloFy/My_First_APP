using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using My_First_APP.Models;
using My_First_APP.Util;
namespace My_First_APP.Controllers
{
    public class HomeController : Controller
    {
        // создаем контекст данных
        BookContext db = new BookContext();

        public ActionResult Index()
        {
           // ViewBag.Message = "Это вызов частичного представления из обычного";
        //    return View();
            return View(db.Books);
        }

        [HttpGet]
        public ActionResult Buy(int id)
        {
            ViewBag.BookId = id;
            return View();
        }
        [HttpPost]
        public string Buy(Purchase purchase)
        {
            purchase.Date = DateTime.Now;
            // добавляем информацию о покупке в базу данных
            db.Purchases.Add(purchase);
            // сохраняем в бд все изменения
            db.SaveChanges();
            return "Спасибо," + purchase.Person + ", за покупку!";
        }

        public ActionResult GetHtml()
        {
            return new HtmlResult("<h2>Привет мир!</h2>");
        }
        public ActionResult GetImage()
        {
            string path = "../Images/9T83tId5wws.jpg";
            return new ImageResult(path);
        }

        public FileResult GetFile()
        {
            // Путь к файлу
            string file_path = Server.MapPath("~/Files/Допуск.pdf");
            // Тип файла - content-type
            string file_type = "application/pdf";
            // Имя файла - необязательно
            string file_name = "PDFIcon.pdf";
            return File(file_path, file_type, file_name);
        }

        public ActionResult Partial()
        {
            ViewBag.Message = "Это частичное представление.";
            return PartialView();
        }
    }
}