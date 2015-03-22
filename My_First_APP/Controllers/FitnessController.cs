
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using My_First_APP.Models;
using My_First_APP.Util;
using System.Data.Sql;

namespace My_First_APP.Controllers
{
    public class FitnessController : Controller
    {
        FITNESSEntities db = new FITNESSEntities();

        

            
        //
        // GET: /Fitness/
        public ActionResult Index()
        {
           
            return View();

        }
        public ActionResult Blank()
        {
          
            return View();
        }

   
	}
}