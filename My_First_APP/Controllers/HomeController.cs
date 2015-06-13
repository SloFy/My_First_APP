using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using My_First_APP.Models;

namespace My_First_APP.Controllers
{
    public class HomeController : Controller
    {
        // создаем контекст данных
        

        public ActionResult Index()
        {
          //  Book b1=db.Books.Where(b=>b.Id==1).First();
           // ViewBag.Message = "Это вызов частичного представления из обычного";
        //    return View();
            return View();
        }

        [HttpGet]
        public ActionResult Buy(int id)
        {
            
            return View();
        }
        [HttpPost]
        public string Buy( )
        {
            return "Спасибо";
        }


        public FileResult GetFile()
        {
            // Путь к файлу
            string file_path = Server.MapPath("~/Files/Допуск.pdf");
            // Тип файла - content-type
            string file_type = "application/pdf";
            // Имя файла - необязательно
            string file_Login = "PDFIcon.pdf";
            return File(file_path, file_type, file_Login);
        }

        public ActionResult Partial()
        {
            ViewBag.Message = "Это частичное представление.";
            return PartialView();
        }
    }
}