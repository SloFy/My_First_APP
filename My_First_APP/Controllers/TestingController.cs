using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using My_First_APP.Models;

namespace My_First_APP.Controllers
{
    public class TestingController : Controller
   {
        static List<Computer> comps = new List<Computer>();
        static TestingController()
        {
            comps.Add(new Computer { Id = 1, Name = "Apple II", Company = "Apple", Year = 1977 });
            comps.Add(new Computer { Id = 2, Name = "Macintosh", Company = "Apple", Year = 1983 });
            comps.Add(new Computer { Id = 3, Name = "IBM PC", Company = "IBM", Year = 1981 });
            comps.Add(new Computer { Id = 4, Name = "Altair", Company = "MITS", Year = 1975 });
        }
        public ActionResult Index()
        {
            return View(comps);
        }
        public ActionResult BootInput()
        {
            return View();
        }
        public ActionResult Details(int id)
        {
            Computer c = comps.FirstOrDefault(com => com.Id == id);
            if (c != null)
                return PartialView(c);
            return HttpNotFound();
        }
    }
}