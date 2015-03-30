using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using System.Data;
using System.Data.SqlClient;
using My_First_APP.Models;
namespace My_First_APP.Controllers
{
    public class NewBlankController : Controller
    {
        //
        // GET: /NewBlank/

        public ActionResult Index()
        {
            // Account user = null;
             string login = User.Identity.GetUserName();
            FITNESSEntities db = new FITNESSEntities();

            int blank_cnt = db.Blank.Count(b => b.Person.Account.Login == login);
            if (blank_cnt == 0)
            {
                var BlankTypes = db.BlankType.Where(m => m.Id == 1);
                return View(BlankTypes);
            }
            else
            {
                var BlankTypes = db.BlankType.Where(m => m.Id != 1);
                return View(BlankTypes);
            }
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Input(int Id=0, int id2=0)
        {
            NewBlank New_Blank = new NewBlank();           
          // IEnumerable<Characterisitcs> new_blank = null;
            FITNESSEntities db = new FITNESSEntities();
            if (id2 == 1)
            {
              //  new_blank=Load_new_blank(Id,db);
            }
            var CH = db.Database.SqlQuery<Characterisitcs>("SELECT * FROM Characterisitcs WHERE id in (SELECT CharacterId FROM BlankTypeCharacteristic WHERE BalnkTypeId=" + Id + ") ORDER BY Id");
            Characterisitcs curr_question = CH.ElementAt(id2 - 1);
            New_Blank.Characteristics = CH.ElementAt(id2 - 1);       
            New_Blank.CharacteristicsValue = db.CharacteristicsValue.Where(c => c.CharID == curr_question.Id).ToList();
            New_Blank.s_values = new string[5];
            New_Blank.b_values = new bool[5];
            New_Blank.blank_id = Id;
            New_Blank.charachter_id = id2;
            curr_question.CharacteristicsValue = db.CharacteristicsValue.Where(c => c.CharID == curr_question.Id).ToList();
            return View(New_Blank);
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Press(int? Id=0,int? id2=0)
        {          
            return View();
        }
        public IEnumerable<Characterisitcs> Load_new_blank(int id, FITNESSEntities db)
        {
            var CH = db.Database.SqlQuery<Characterisitcs>("SELECT * FROM Characterisitcs WHERE id in (SELECT CharacterId FROM BlankTypeCharacteristic WHERE BalnkTypeId=" + id + ") ORDER BY Id");
            List<Characterisitcs> question_list = new List<Characterisitcs>(CH.ToList());
            Blank b = new Blank();            
            b.BlankType = db.BlankType.Find(id);
            b.CreateDate = DateTime.UtcNow.ToString();
            string login = User.Identity.GetUserName();
            b.Person = db.Person.Where(P => P.Account == db.Account.Where(A => A.Login == login).FirstOrDefault()).FirstOrDefault();
            
            Session["Questions"] = question_list;
            Session["Question_cnt"] = CH.Count();
            return CH;
        }

        public void Next_question()
        {
            
           
        }
        public void Prev_question()
        {
           // Session["Question_ID"] = (Convert.ToInt32(Session["Question_ID"]) - 1).ToString();
        }
        public void Reflesh()
        {
            FITNESSEntities db = new FITNESSEntities();
            if (Session["Question_cnt"] == null)
            {
                Session["Question_cnt"] = db.BlankTypeCharacteristic.Count(btc => btc.BalnkTypeId == 0);
            }
        }
        public void GenerateNewView()
        {
            
        }
    }
}