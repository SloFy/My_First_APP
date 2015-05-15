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
        public ActionResult Test(int Id=2)
        {
            string name = User.Identity.GetUserName();
            FITNESSEntities db = new FITNESSEntities();
            FillingBlank Model = new FillingBlank();  
            //Создание нового бланка
            Blank blank = new Blank();
            blank.BlankType = db.BlankType.Where(b => b.Id == Id).FirstOrDefault();                   
            blank.Person = db.Person.Where(p => p.Account == (db.Account.Where(a => a.Login == name ).FirstOrDefault())).FirstOrDefault();
            blank.CreateDate = DateTime.Now.ToString();     
            blank.Id = (db.Blank.Max(m => m.Id) + 1);
            db.Blank.Add(blank);
            db.SaveChanges();
            Session["Currnet_Blank"] = blank.Id;
            var Questions = db.Database.SqlQuery<Characterisitcs>("SELECT * FROM Characterisitcs WHERE id in (SELECT CharacterId FROM BlankTypeCharacteristic WHERE BalnkTypeId=" + Id + ") ORDER BY Id").ToArray();
            
            for (int i = 0; i < Questions.Count(); i++)
            {
                Element AddNewElement = new Element();
                AddNewElement.Question = Questions.ElementAt(i);
                AddNewElement.Default_Answers = db.CharacteristicsValue.Where(c => c.CharID == AddNewElement.Question.Id).ToList();
                Model.Elements.Add(AddNewElement);
            }           
            return View(Questions);
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]      
        public ActionResult Part(New_Model model)
        {
            FITNESSEntities db=new FITNESSEntities();

            string Answer="";
            for(int i=0;i<model.Default_Answers.Count();i++)
            {
                if ((model.type_answers[i] == "CB")&&(model.b_answers[i]==true))
                {
                    Answer += model.Default_Answers[i].Value + "| ";
                }
                if (model.type_answers[i] == "TB")
                {
                    Answer += model.s_answers[i]+ "| ";
                }
                if ((model.type_answers[i]=="CB_TB")&&(model.b_answers[i]==true))
                {
                    Answer += model.Default_Answers[i].Value + "(" + model.s_answers[i] + ")" + "| ";
                }
            }
            Session["Current_Question"] = (Convert.ToInt32(Session["Current_Question"])) + 1;
            New_Model next = new New_Model();
            //Создание модели следующего вопроса.
            next.blank_id = model.blank_id;
            next.current_question = model.current_question + 1;
            next.questions = model.questions;
            next.Question = next.questions.ElementAt(next.current_question);
            next.Default_Answers=db.CharacteristicsValue.Where(c => c.CharID == next.Question.Id).ToList();
            for (int i = 0; i < next.Default_Answers.Count; i++)
            {
                next.b_answers.Add(false);
                next.s_answers.Add("");
                next.type_answers.Add("");
            }
            return View("_Answer",model);
        }
       // [HttpPost]
       //// [AllowAnonymous]
       // [ValidateAntiForgeryToken]
      //  public ActionResult Filling(FillingBlank model)
      //  {
      //      return View();
      //  }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ViewResult Filling(FillingBlank Model)
        {
           // ((FillingBlank)Session["FillingBlank"]).Elements = Model.Elements;
            Session["Current_Question"] = Convert.ToInt32(Session["Current_Question"]) + 1;
            return View("Test", Model);
        }

        // Старый код:
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Input(int Id=0, int id2=1)
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